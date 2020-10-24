using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Base;

namespace LibraryClient
{
    public class LibraryManager
    {
        private ServiceClient serviceClient;
        public LibraryManager()
        {
            serviceClient = new ServiceClient();
        }

        public void Run()
        {
            var exit = false;
            while (!exit)
            {
                var option = Menu("1 - Consultar livros\n" +
                             "2 - Criar Livro\n" +
                             "3 - Remover livro\n" +
                             "4 - Alterar livro\n" +
                             "5 - Consultar autores\n" +
                             "6 - Alterar autores\n" +
                             "7 - Criar autores\n" +
                             "8 - Remover autores\n" +
                             "9 - Sair\n");

                switch (option)
                {
                    case 1:
                        SearchBooks();
                        break;
                    case 2:
                        CreateBook();
                        break;
                    case 3:
                        RemoveBook();
                        break;
                    case 4:
                        EditBook();
                        break;
                    case 5:
                        SearchAuthor();
                        break;
                    case 6:
                        EditAuthor();
                        break;
                    case 7:
                        CreateAuthor();
                        break;
                    case 8:
                        RemoveAuthor();
                        break;
                    case 9:
                        exit = true;
                        break;

                }
            }
        }

        private void CreateAuthor()
        {
            var name = GetAnswer("Informe o nome do autor a ser inserido");
            serviceClient.CreateAuthor(name);
            DisplayResult($"Autor {name} criado");
        }

        private void EditAuthor()
        {
            var code = GetAnswer("Informe o código do autor a ser editado");
            if (!serviceClient.AuthorExistsByCode(code))
            {
                DisplayResult($"Autor {code} não encontrado");
                return;
            }
            var newName = GetAnswer("Informe o novo nome do autor");

            serviceClient.UpdateAuthorName(code, newName);
            DisplayResult($"Autor {code} agora se chama {newName}");
        }

        private void SearchAuthor()
        {
            var str = GetAnswer("Informe o nome do autor:");
            var authors = serviceClient.QueryAuthorByName(str);
            var resultStr = GetResults(authors);
            DisplayResult(resultStr);
        }

        private void RemoveAuthor()
        {
            var code = GetAnswer("Informe o código do autor a ser removido");
            if (!serviceClient.AuthorExistsByCode(code))
            {
                DisplayResult($"Autor {code} não encontrado");
                return;
            }

            if (serviceClient.QueryAuthorByCodeInBooks(code))
            {
                DisplayResult($"Autor {code} não pode ser removido, pois existem livros associados a ele");
                return;
            }

            serviceClient.DeleteAuthorByCode(code);
            DisplayResult($"Autor {code} Removido");
        }

        private void EditBook()
        {
            var code = GetAnswer("Informe o código do livro a ser editado");
            var books = serviceClient.QueryBookByCode(code);
            if (!books.Any())
            {
                DisplayResult($"Livro {code} não encontrado");
                return;
            }

            var validOption = false;
            while (!validOption)
            {
                var option = Menu($"Você está editando:\n{GetResults(books)}" +
                            $"1 - Titulo\n" +
                             "2 - Autor\n" +
                             "3 - Por ano e edição\n" +
                             "4 - Voltar");

                var resultStr = "Comando invalido";
                validOption = true;
                string str;
                switch (option)
                {
                    case 1:
                        str = GetAnswer("Informe o título do livro:");
                        serviceClient.UpdateBookName(code, str);
                        resultStr = $"Atualizado:{GetResults(serviceClient.QueryBookByCode(code))}";
                        break;
                    case 2:
                        str = GetAnswer("Informe os códigos dos autores entre vírgulas:");
                        var authors = str.Split(',').Select(a => int.Parse(a)).ToArray();
                        foreach (var authorcode in authors.Distinct())
                        {
                            if (!serviceClient.AuthorExistsByCode(authorcode.ToString()))
                            {
                                DisplayResult($"Autor {authorcode} não encontrado");
                                return;
                            }
                        }
                        serviceClient.UpdateBookAuthor(code, authors);
                        resultStr = $"Atualizado:{GetResults(serviceClient.QueryBookByCode(code))}";
                        break;
                    case 3:
                        resultStr = EditEdition(code);
                        break;
                    case 4:
                        resultStr = "Voltando ao menu Inicial";
                        break;
                    default:
                        validOption = false;
                        break;
                }
                DisplayResult(resultStr);
            }
        }

        private string EditEdition(string code)
        {

            var validOption = false;
            while (!validOption)
            {
                var option = Menu($"1 - Adicionar Edição\n" +
                             "2 - Editar Edição\n" +
                             "3 - Remover Edição\n" +
                             "4 - Voltar");

                var resultStr = "Comando invalido";
                validOption = true;
                string edition;
                string year;
                switch (option)
                {
                    case 1:
                        year = GetAnswer("Informe o ano:");
                        edition = GetAnswer("Informe a edição:");

                        if (edition.Length > 1)
                        {
                            resultStr = $"Edição deve conter apenas 1 caracter";
                            break;
                        }
                        if (serviceClient.QueryEditionByNumber(code, edition))
                        {
                            resultStr = $"Edição {edition} já existe";
                            break;
                        }
                        serviceClient.AddBookYearAndEdition(code, year, edition);
                        resultStr = $"Edição {edition} adicionada";
                        break;
                    case 2:
                        edition = GetAnswer("Informe a edição:");
                        if (edition.Length > 1)
                        {
                            resultStr = $"Edição deve conter apenas 1 caracter";
                            break;
                        }
                        if (!serviceClient.QueryEditionByNumber(code, edition))
                        {
                            resultStr = $"Edição {edition} não existe";
                            break;
                        }
                        year = GetAnswer("Informe o ano:");
                        serviceClient.UpdateBookYearAndEdition(code, year, edition);
                        resultStr = $"Edição {edition} agora é do ano {year}";
                        break;

                    case 3:
                        edition = GetAnswer("Informe a edição:");
                        if (edition.Length > 1)
                        {
                            resultStr = $"Edição deve conter apenas 1 caracter";
                            break;
                        }
                        if (!serviceClient.QueryEditionByNumber(code, edition))
                        {
                            resultStr = $"Edição {edition} não existe";
                            break;
                        }
                        if (serviceClient.CountEditionsByBook(code) == 1)
                        {
                            resultStr = $"Edição {edition} é unica para esse livro e não pode ser removida";
                            break;
                        }
                        serviceClient.DeleteBookEdition(code, edition);
                        resultStr = $"Edição {edition} removida";
                        break;
                }
            }

            return $"Atualizado:{GetResults(serviceClient.QueryBookByCode(code))}";
        }

        private void RemoveBook()
        {
            string answer;
            var title = GetAnswer("Informe o título completo do livro a ser removido:");
            var books = serviceClient.QueryBookByCompleteName(title);
            if (!books.Any())
            {
                books = serviceClient.QueryBookByName(title);
                if (books.Any())
                {
                    var possible = GetResults(books);
                    answer = $"Nenhum livro removido, talvez você quis dizer:\n{possible}";
                }
                else
                {
                    answer = $"Livro não encontrado";
                }

                DisplayResult(answer);
            }

            else
            {
                var book = books.FirstOrDefault();
                serviceClient.DeleteBook(book);
                DisplayResult($"Livro removido:\n{book}");

            }

        }

        private void CreateBook()
        {
            var title = GetAnswer("Informe o título:");
            var author = GetAnswer("Informe o código dos autores entre vírgulas:");
            var authors = author.Split(',');

            foreach (var authorcode in authors)
            {
                if (!serviceClient.AuthorExistsByCode(authorcode))
                {
                    DisplayResult($"Autor {author} não encontrado");
                    return;
                }
            }

            var year = GetAnswer("Informe o ano de publicação:");
            var edition = GetAnswer("Informe a edição (1 caractere)");
            if (edition.Length > 1)
            {
                DisplayResult($"Edição deve ser apenas 1 caractere");
                return;
            }
            serviceClient.CreateBook(title, authors, year, edition);
        }

        private int Menu(string menuStr)
        {
            var answer = GetAnswer(menuStr);
            if (!int.TryParse(answer, out int n))
                n = -1;

            return n;
        }

        private string GetAnswer(string msg)
        {
            var str = string.Empty;
            Console.WriteLine(msg);

            str = Console.ReadLine();
            while (str == string.Empty)
            {
                Console.WriteLine("Valor vazio!");
                str = Console.ReadLine();
            }

            return str;
        }

        private void SearchBooks()
        {
            var validOption = false;
            while (!validOption)
            {
                var option = Menu("1 - Por Titulo\n" +
                             "2 - Por autor\n" +
                             "3 - Por ano e edição\n" +
                             "4 - Voltar");

                var resultStr = "Comando invalido";
                validOption = true;
                string str;
                List<Book> books;
                switch (option)
                {
                    case 1:
                        str = GetAnswer("Informe o título do livro:");
                        books = serviceClient.QueryBookByName(str);
                        resultStr = GetResults(books);
                        break;
                    case 2:
                        str = GetAnswer("Informe o nome do autor:");
                        books = serviceClient.QueryBookByAuthor(str);
                        resultStr = GetResults(books);
                        break;
                    case 3:
                        var year = GetAnswer("Informe o ano:");
                        var edition = GetAnswer("Informe a edição:");
                        books = serviceClient.QueryBookByYearAndEdition(year, edition);
                        resultStr = GetResults(books);
                        break;
                    case 4:
                        resultStr = "Voltando ao menu Inicial";
                        break;
                    default:
                        validOption = false;
                        break;
                }
                DisplayResult(resultStr);
            }

        }

        private void DisplayResult(string resultStr)
        {
            Console.WriteLine(resultStr + "\nPressione enter para continuar");
            Console.ReadLine();
        }

        private string GetResults(IEnumerable<IBook> books)
        {
            var str = string.Empty;
            if (!books.Any())
                str = "A busca não retornou resultados.";
            foreach (var book in books)
                str += $"{book.ToString()}\n";
            return str;
        }

        private string GetResults(Dictionary<string, string> authors)
        {
            var str = string.Empty;
            if (!authors.Any())
                str = "A busca não retornou resultados.";
            foreach (var author in authors)
                str += $"[{author.Key}] {author.Value}\n";
            return str;
        }
    }
}
