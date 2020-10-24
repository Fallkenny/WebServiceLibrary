using System.Collections.Generic;
using Library.Base;

namespace Library
{
    public interface IBookQueriesManager
    {
        void AddBookYearAndEdition(string code, string year, string edition);
        bool AuthorExistsByCode(string author);
        int CountEditionsByBook(string code);
        void CreateAuthor(string name);
        void CreateBook(string title, string[] authors, string year, string edition);
        void DeleteAuthorByCode(string code);
        void DeleteBook(Book book);
        void DeleteBookEdition(string code, string number);
        bool QueryAuthorByCodeInBooks(string code);
        Dictionary<string, string> QueryAuthorByName(string str);
        List<IBook> QueryBookByAuthor(string str);
        List<IBook> QueryBookByCode(string code);
        List<IBook> QueryBookByCompleteName(string name);
        List<IBook> QueryBookByName(string name);
        List<IBook> QueryBookByYearAndEdition(string year, string edition);
        bool QueryEditionByNumber(string code, string number);
        void UpdateAuthorName(string code, string newName);
        void UpdateBookAuthor(string bookcode, int[] authorcodes);
        void UpdateBookName(string code, string name);
        void UpdateBookYearAndEdition(string code, string year, string edition);
    }
}