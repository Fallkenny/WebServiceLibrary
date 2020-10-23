using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            localhost.LibraryService libraryService = new localhost.LibraryService();

            Console.WriteLine(libraryService.HelloWorld());

            Console.ReadKey();

        }
    }
}
