using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Base
{
    public class LibrarySerializer
    {
        public LibrarySerializer()
        {
            _serializer = new JsonSerializer();
        }


        private JsonSerializerSettings CreateBasicSerializerSettings() =>
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

        protected JsonSerializerSettings SerializerSettings => CreateBasicSerializerSettings();
        private Newtonsoft.Json.JsonSerializer _serializer;

        public T Desserialize<T>(string fileDataAsString)
        {
            try
            {
                using (var stringReader = new StringReader(fileDataAsString))
                {
                    var reader = stringReader;

                    using (var jsonReader = new JsonTextReader(reader))
                        return _serializer.Deserialize<T>(jsonReader);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        public string Serialize(object value, string filePath)
        {
            try
            {
                using (var streamWriter = new StringWriter())
                using (var writer = new JsonTextWriter(streamWriter))
                {

                    _serializer.Serialize(writer, value);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Serialize(object value)
        {
            try
            {
                var settings = CreateBasicSerializerSettings();
                return JsonConvert.SerializeObject(value, settings);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
