using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase Categoría.
    /// </summary>
    public class Categoria : IJsonConvertible
    {
        public string Categ {get;set;}
        
        /// <summary>
        /// El constructor de la clase.
        /// </summary>

        [JsonConstructor]
        public Categoria(string categ)
        {
            this.Categ = categ;
        }
        
        public static Categoria CategoriaJson(string json)
        {
            Categoria deserialized = JsonSerializer.Deserialize<Categoria>(json);
            return deserialized;
        }
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void LoadFromJson(string json)
        {
            Categoria deserialized = JsonSerializer.Deserialize<Categoria>(json);
            this.Categ = deserialized.Categ;

        }
    }
}