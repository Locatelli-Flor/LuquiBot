using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase Administrador.
    /// Principios y/o Patrones utilizados para implementar la clase:
    /// Creator: Es la única clase que tiene la responsabilidad de crear y borrar categorías.
    /// SRP: La única responsabilidad de la clase es administrar las categorías y servicios. Además.
    /// ISP: Los tipos que implementa la clase los utiliza. Por ejemplo: IUsuario y los Atributos Nombre y Email que se usan para crear una instancia de Administrador.
    /// </summary>
    public class Administrador : IUsuario, IJsonConvertible
    {
        public string Nombre {get;set;}
        public string Email {get;set;}
        
        /// <summary>
        /// El constructor de la clase.
        /// </summary>

        [JsonConstructor]
        public Administrador(string nombre, string email)
        {
            this.Nombre = nombre;
            this.Email = email;
        }
        
        public Administrador(string json)
        {
            this.LoadFromJson(json);
        }
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void LoadFromJson(string json)
        {
            Trabajador deserialized = JsonSerializer.Deserialize<Trabajador>(json);
            this.Nombre = deserialized.Nombre;
            this.Email = deserialized.Email;
        }

        /// <summary>
        /// Método AgregarCategoría. Crea una categoría y la añade a la lista de categorías.
        /// </summary>

        public void AgregarCategoria(Categoria categoria)
        {
            Singleton<ListaCategorias>.Instance.ListaDeCategorias.Add(categoria);
        }

        /// <summary>
        /// Método QuitarCategoría. Quita una categoría de la lista de categorías.
        /// </summary>

        public void QuitarCategoria(Categoria categoria)
        {
            Singleton<ListaCategorias>.Instance.ListaDeCategorias.Remove(categoria);
        }

        /// <summary>
        /// Método QuitarServicio. Quita un servicio de la lista de servicios.
        /// </summary>

        public void QuitarServicio(Servicio servicio)
        {
            Singleton<OfertaServicios>.Instance.ListaDeServicios.Remove(servicio);
        }
    }
}