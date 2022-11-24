using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase servicio.
    /// </summary>
    public class Servicio : IJsonConvertible
    {
        public string Descripcion{get;set;}
        public Categoria Categoria{get;set;}
        public Trabajador Trabajador{get;set;}
        public int Precio{get;set;}

        /// <summary>
        /// El constructor de la clase.
        /// </summary>

        [JsonConstructor]
        public Servicio(string descripcion, Categoria categoria, Trabajador trabajador, int precio)
        {
            this.Descripcion = descripcion;
            this.Categoria = categoria;
            this.Trabajador = trabajador;
            this.Precio = precio;
        }

        public Servicio(string json)
        {
            this.LoadFromJson(json);
        }
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void LoadFromJson(string json)
        {
            Servicio deserialized = JsonSerializer.Deserialize<Servicio>(json);
            this.Descripcion = deserialized.Descripcion;
            this.Categoria = deserialized.Categoria;
            this.Trabajador = deserialized.Trabajador;
            this.Precio = deserialized.Precio;
        }

    }
}