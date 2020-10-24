using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base
{
    [Serializable]
    public class Book : IBook
    {
        public string Title { get; set; }

        public int Code { get; set; }

        public int PublicationYear { get; set; }

        public int Edition { get; set; }

        public string Author { get; set; }

        //public byte[] ToByteArray()
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, this);
        //        return ms.ToArray();
        //    }
        //}

        public override string ToString()
        {
            return $"[{Code}] {Title} - {Author}, Edição: {Edition} - {PublicationYear}";
        }

        //public static Book FromBytes(byte[] arrBytes)
        //{
        //    MemoryStream memStream = new MemoryStream();
        //    BinaryFormatter binForm = new BinaryFormatter();
        //    memStream.Write(arrBytes, 0, arrBytes.Length);
        //    memStream.Seek(0, SeekOrigin.Begin);
        //    Book obj = (Book)binForm.Deserialize(memStream);

        //    return obj;
        //}
    }
}
