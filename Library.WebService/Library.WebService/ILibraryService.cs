using System.Collections.Generic;

namespace Library.WebService
{
    public interface ILibraryService
    {
        void AddBookYearAndEdition(string code, string year, string edition);
        bool AuthorExistsByCode(string str);
        int CountEditionsByBook(string code);
        void CreateAuthor(string name);
        void CreateBook(string title, string[] authors, string year, string edition);
        void DeleteAuthorByCode(string code);
        void DeleteBook(string book);
        void DeleteBookEdition(string code, string number);
        string HelloWorld();
        bool QueryAuthorByCodeInBooks(string code);
        string QueryAuthorByName(string str);
        string QueryBookByAuthor(string str);
        string QueryBookByCode(string code);
        string QueryBookByCompleteName(string name);
        string QueryBookByName(string str);
        string QueryBookByYearAndEdition(string year, string edition);
        bool QueryEditionByNumber(string code, string number);
        void UpdateAuthorName(string code, string newName);
        void UpdateBookAuthor(string code, int[] str);
        void UpdateBookName(string code, string str);
        void UpdateBookYearAndEdition(string code, string year, string edition);
    }
}