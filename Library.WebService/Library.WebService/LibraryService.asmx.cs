using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Library;
using Library.Base;

namespace Library.WebService
{
    /// <summary>
    /// Summary description for LibraryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]    
    public class LibraryService : System.Web.Services.WebService,ILibraryService
    {

        LibrarySerializer serializer;

        public LibraryService()
        {
            serializer = new LibrarySerializer();
        }


        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public string QueryBookByName(string str)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            return serializer.Serialize(BookQueriesManager.Instance.QueryBookByName(str));

        }


        [WebMethod]
        public string QueryBookByAuthor(string str)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            return serializer.Serialize(BookQueriesManager.Instance.QueryBookByAuthor(str));

        }


        [WebMethod]
        public string QueryBookByYearAndEdition(string year, string edition)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            return serializer.Serialize(BookQueriesManager.Instance.QueryBookByYearAndEdition(year, edition));

        }


        [WebMethod]
        public bool AuthorExistsByCode(string str)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            return BookQueriesManager.Instance.AuthorExistsByCode(str);

        }


        //[WebMethod]
        //public void CreateBookObj(Book book)
        //{
        //    //    BookQueriesManager.Instance.QueryBookByName(str);

        //    BookQueriesManager.Instance.CreateBook(book.Title, new string[] { book.Author },
        //        book.PublicationYear.ToString(), book.Edition.ToString());

        //}


        [WebMethod]
        public void UpdateBookName(string code, string str)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            BookQueriesManager.Instance.UpdateBookName(code, str);

            //return true;
        }


        [WebMethod]
        public void UpdateBookAuthor(string code, int[] str)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            BookQueriesManager.Instance.UpdateBookAuthor(code, str);

            //return true;
        }


        [WebMethod]
        public void UpdateBookYearAndEdition(string code, string year, string edition)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            BookQueriesManager.Instance.UpdateBookYearAndEdition(code, year, edition);

            //return true;
        }


        [WebMethod]
        public void AddBookYearAndEdition(string code, string year, string edition)
        {
            BookQueriesManager.Instance.AddBookYearAndEdition(code, year, edition);
        }


        [WebMethod]
        public bool QueryEditionByNumber(string code, string number)
        {
            //    BookQueriesManager.Instance.QueryBookByName(str);

            return BookQueriesManager.Instance.QueryEditionByNumber(code, number);

        }

        [WebMethod]
        public int CountEditionsByBook(string code)
        {
            return BookQueriesManager.Instance.CountEditionsByBook(code);
        }


        [WebMethod]
        public void CreateAuthor(string name)
        {
            BookQueriesManager.Instance.CreateAuthor(name);
        }

        [WebMethod]
        public void CreateBook(string title, string[] authors, string year, string edition)
        {
            BookQueriesManager.Instance.CreateBook(title, authors, year, edition);
        }


        [WebMethod]
        public void DeleteAuthorByCode(string code)
        {
            BookQueriesManager.Instance.DeleteAuthorByCode(code);
        }


        [WebMethod]
        public void DeleteBook(string book)
        {
            BookQueriesManager.Instance.DeleteBook(serializer.Desserialize<Book>(book));
        }

        [WebMethod]
        public void DeleteBookEdition(string code, string number)
        {
            BookQueriesManager.Instance.DeleteBookEdition(code, number);
        }

        [WebMethod]
        public bool QueryAuthorByCodeInBooks(string code)
        {
            return BookQueriesManager.Instance.QueryAuthorByCodeInBooks(code);
        }

        [WebMethod]
        public string QueryAuthorByName(string str)
        {
            return new LibrarySerializer().Serialize(BookQueriesManager.Instance.QueryAuthorByName(str));
        }

        [WebMethod]
        public string QueryBookByCode(string code)
        {
            return serializer.Serialize(BookQueriesManager.Instance.QueryBookByCode(code));
        }

        [WebMethod]
        public string QueryBookByCompleteName(string name)
        {
            return serializer.Serialize(BookQueriesManager.Instance.QueryBookByCompleteName(name));
        }

        [WebMethod]
        public void UpdateAuthorName(string code, string newName)
        {
            BookQueriesManager.Instance.UpdateAuthorName(code, newName);
        }

    }
}
