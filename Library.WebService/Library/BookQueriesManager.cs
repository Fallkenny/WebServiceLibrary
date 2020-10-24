using Library.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BookQueriesManager
    {
        private static BookQueriesManager _instance = null;
        NpgsqlConnection _connection = null;
        public static BookQueriesManager Instance => _instance ?? (_instance = new BookQueriesManager());

        private BookQueriesManager()
        {
            _connection = new NpgsqlConnection($"Server={DatabaseParameters.DATABASE_ADRESS};User Id={DatabaseParameters.DATABASE_USER}; " +
           $"Password={DatabaseParameters.DATABASE_PASSWORD};Database={DatabaseParameters.DATABASE_NAME};");
        }

        public List<Book> QueryBookByName(string name)
        {
            var query = $"SELECT livros.codigo, livros.titulo, edicao.numero, edicao.ano " +
                $"FROM livros " +
                $"INNER JOIN edicao ON livros.codigo = edicao.codigolivro " +
                $"WHERE LOWER(titulo) like LOWER('%{name}%') ";
            return QueryBooks(query);
        }

        public void UpdateBookName(string code, string name)
        {
            _connection.Open();

            var query = $"UPDATE livros " +
                $"SET titulo = '{name}' " +
                $"WHERE codigo = {code}";
            ExecuteQuery(query);
            _connection.Close();

        }

        public void UpdateBookAuthor(string bookcode, int[] authorcodes)
        {
            _connection.Open();
            var query = $"DELETE FROM livroautor " +
                     $"WHERE codigolivro={bookcode}";
            ExecuteQuery(query);

            foreach (var author in authorcodes.Distinct())
            {
                query = $"insert into livroautor (codigolivro, codigoautor) VALUES ({bookcode}, {author});";
                ExecuteQuery(query);
            }
            _connection.Close();
        }

        public void UpdateBookYearAndEdition(string code, string year, string edition)
        {
            _connection.Open();

            var query = $"UPDATE edicao " +
                        $"SET ano = {year} " +
                        $"WHERE codigolivro = {code} " +
                        $"AND numero='{edition}'";
            ExecuteQuery(query);
            _connection.Close();

        }
        public void AddBookYearAndEdition(string code, string year, string edition)
        {
            _connection.Open();

            var query = $"INSERT INTO edicao (codigolivro, numero, ano) " +
                        $"VALUES ({code}, '{edition}', {year});";
            ExecuteQuery(query);
            _connection.Close();
        }


        public bool QueryEditionByNumber(string code, string number)
        {
            _connection.Open();
            var query = $"SELECT * " +
                        $"FROM edicao " +
                        $"WHERE codigolivro = {code} " +
                        $"AND numero = '{number}'";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var exists = reader.HasRows;
            reader.Close();
            _connection.Close();
            return exists;
        }

        public Dictionary<string, string> QueryAuthorByName(string str)
        {
            _connection.Open();

            var queryAuthor = "SELECT TRIM(autor.nome) as nome, autor.codigo " +
             "FROM autor " +
             $"WHERE LOWER(nome) like LOWER('%{str}%')";

            var command = new NpgsqlCommand(queryAuthor, _connection);
            var reader = command.ExecuteReader();

            var authors = new Dictionary<string, string>();
            while (reader.Read())
                authors.Add(reader["codigo"].ToString(), reader["nome"].ToString());
            _connection.Close();
            return authors;
        }

        public void UpdateAuthorName(string code, string newName)
        {
            _connection.Open();

            var query = $"UPDATE autor " +
                $"SET nome = '{newName}' " +
                $"WHERE codigo = {code}";
            ExecuteQuery(query);
            _connection.Close();
        }

        public bool QueryAuthorByCodeInBooks(string code)
        {
            _connection.Open();
            var query = $"SELECT * " +
                        $"FROM livroautor " +
                        $"WHERE codigoautor = {code} ";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var exists = reader.HasRows;
            reader.Close();
            _connection.Close();
            return exists;
        }

        public void DeleteAuthorByCode(string code)
        {
            _connection.Open();
            var query = $"DELETE FROM autor " +
                  $"WHERE codigo={code}";
            ExecuteQuery(query);
            _connection.Close();
        }

        public List<Book> QueryBookByCompleteName(string name)
        {
            var query = $"SELECT livros.codigo, livros.titulo, autor.nome, edicao.numero, edicao.ano " +
                $"FROM livros " +
                $"INNER JOIN livroautor ON livros.codigo = livroautor.codigolivro " +
                $"INNER JOIN autor ON livroautor.codigoautor = autor.codigo " +
                $"INNER JOIN edicao ON livros.codigo = edicao.codigolivro " +
                $"WHERE TRIM(titulo) = '{name}' ";
            return QueryBooks(query);
        }

        private List<Book> QueryBooks(string query)
        {
            _connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();

            var list = new List<Book>();
            while (reader.Read())
            {
                var book = new Book
                {
                    //Author = reader["nome"].ToString().Replace("  ", ""),
                    Code = int.Parse(reader["codigo"].ToString().Replace("  ", "")),
                    Title = reader["titulo"].ToString().Replace("  ", ""),
                    PublicationYear = int.Parse(reader["ano"].ToString().Replace("  ", "")),
                    Edition = int.Parse(reader["numero"].ToString().Replace("  ", ""))
                };
                list.Add(book);
            }
            reader.Close();
            foreach (var book in list)
            {
                var queryAuthor = "SELECT livroautor.codigolivro, TRIM(autor.nome) as nome " +
                    "FROM livroautor " +
                    "INNER JOIN autor ON livroautor.codigoautor = autor.codigo " +
                    $"WHERE livroautor.codigolivro = {book.Code}";

                command = new NpgsqlCommand(queryAuthor, _connection);
                reader = command.ExecuteReader();

                var authors = new List<string>();
                while (reader.Read())
                    authors.Add(reader["nome"].ToString());
                reader.Close();
                book.Author = string.Join(", ", authors.ToArray());
            }
            _connection.Close();

            return list;
        }

        public void DeleteBook(Book book)
        {
            _connection.Open();
            var query = $"DELETE FROM edicao " +
                  $"WHERE edicao.codigolivro={book.Code}";
            ExecuteQuery(query);

            query = $"DELETE FROM livroautor " +
                        $"WHERE livroautor.codigolivro ={book.Code}";
            ExecuteQuery(query);
            query = $"DELETE FROM livros " +
                       $"WHERE livros.codigo ={book.Code}";
            ExecuteQuery(query);
            _connection.Close();

        }

        private void ExecuteQuery(string query)
        {
            var command = new NpgsqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public bool AuthorExistsByCode(string author)
        {
            _connection.Open();
            var query = $"SELECT * " +
                        $"FROM autor " +
                        $"WHERE codigo = {int.Parse(author)} ";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var exists = reader.HasRows;
            reader.Close();
            _connection.Close();
            return exists;
        }

        public int CountEditionsByBook(string code)
        {
            _connection.Open();
            var query = $"SELECT count(*) " +
                        $"FROM edicao " +
                        $"WHERE codigolivro = {code} ";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var count = int.Parse(reader["count"].ToString());
            reader.Close();
            _connection.Close();
            return count;
        }

        public void DeleteBookEdition(string code, string number)
        {
            _connection.Open();
            var query = $"DELETE FROM edicao " +
                  $"WHERE codigolivro={code} " +
                  $"AND numero = '{number}'";
            ExecuteQuery(query);
            _connection.Close();
        }

        public List<Book> QueryBookByCode(string code)
        {
            var query = $"SELECT livros.codigo, livros.titulo, edicao.numero, edicao.ano " +
                $"FROM livros " +
                $"INNER JOIN edicao ON livros.codigo = edicao.codigolivro " +
                $"WHERE livros.codigo = {code} ";
            return QueryBooks(query);
        }

        public void CreateAuthor(string name)
        {
            _connection.Open();
            var query = $"SELECT MAX(codigo) FROM autor ";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var code = int.Parse(reader["max"].ToString()) + 1;
            reader.Close();

            query = $"INSERT INTO autor (codigo, nome) VALUES ({code}, '{name}');";
            ExecuteQuery(query);

            _connection.Close();
        }


        public void CreateBook(string title, string[] authors, string year, string edition)
        {
            _connection.Open();
            var query = $"SELECT MAX(codigo) FROM livros ";
            var command = new NpgsqlCommand(query, _connection);
            var reader = command.ExecuteReader();
            reader.Read();
            var code = int.Parse(reader["max"].ToString()) + 1;
            reader.Close();

            query = $"INSERT INTO livros (codigo, titulo) VALUES ({code}, '{title}');";
            ExecuteQuery(query);


            foreach (var author in authors.Distinct())
            {
                query = $"INSERT INTO livroautor (codigolivro, codigoautor) VALUES ({code}, {int.Parse(author)});";
                ExecuteQuery(query);
            }

            query = $"INSERT INTO edicao (codigolivro, numero, ano) VALUES ({code}, '{edition}', {int.Parse(year)});";
            ExecuteQuery(query);

            _connection.Close();
        }

        public List<Book> QueryBookByAuthor(string str)
        {
            var query = $"SELECT livros.codigo, livros.titulo, autor.nome, edicao.numero, edicao.ano  " +
                        $"FROM livros " +
                        $"INNER JOIN livroautor ON livros.codigo = livroautor.codigolivro " +
                        $"INNER JOIN autor ON livroautor.codigoautor = autor.codigo " +
                        $"INNER JOIN edicao ON livros.codigo = edicao.codigolivro " +
                        $"WHERE LOWER(nome) like LOWER('%{str}%') ";
            return QueryBooks(query);
        }

        public List<Book> QueryBookByYearAndEdition(string year, string edition)
        {
            var query = $"SELECT livros.codigo, livros.titulo, edicao.numero, edicao.ano  " +
                        $"FROM livros " +
                        $"INNER JOIN edicao ON livros.codigo = edicao.codigolivro " +
                        $"WHERE ano = {year} AND numero = '{edition}'" +
                        $"ORDER BY livros.titulo";
            return QueryBooks(query);
        }


    }
}
