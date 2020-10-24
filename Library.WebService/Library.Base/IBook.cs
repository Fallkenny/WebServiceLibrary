using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base
{
    public interface IBook
    {
        string Title { get; set; }

        int Code { get; set; }

        int PublicationYear { get; set; }

        int Edition { get; set; }

        string Author { get; set; }
    }
}
