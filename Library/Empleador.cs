using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase Empleador.
    /// Patrones y/o Principios utilizados para implementar la clase:
    /// Creator: En el método ContratarServicio se crea un objeto de la clase SolicitudContratacion. El que tiene la responsabilidad de hacer esto es Empleador porque es el que le interesa contratar a alguien.
    /// ISP: Los tipos que implementa la clase los utiliza. Por ejemplo: ICalificable y los métodos Calificar y GetCalificacion.
    /// </summary>

    public class Empleador : IUsuario, ICalificable, IJsonConvertible
    {
        public string Nombre {get;set;}
        public string Email {get;set;}
        public string Telefono {get;set;}
        public Tuple<double, double> Ubicacion {get;set;}
        
        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        
        [JsonConstructor]
        public Empleador(string nombre, string email, string telefono, Tuple<double, double> Ubicacion)
        {
            this.Nombre = nombre;
            this.Email = email;
            this.Telefono = telefono;
            this.Ubicacion = Ubicacion;
        }

        public Empleador(string json)
        {
            this.LoadFromJson(json);
        }
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void LoadFromJson(string json)
        {
            Empleador deserialized = JsonSerializer.Deserialize<Empleador>(json);
            this.Nombre = deserialized.Nombre;
            this.Email = deserialized.Email;
            this.Telefono = deserialized.Telefono;
            this.Ubicacion = deserialized.Ubicacion;

        }

        /// <summary>
        /// Método GetCalificacion. Itera sobre la lista de contratos y si ya fueron calificados, los suma al total. Luego intenta hacer el total, teniendo en cuenta que si el empleador no tiene ninguna calificacion va a salir la excepción DivideByZeroError.
        /// </summary>

        public double GetCalificacion()
        {
            double calificacion = 0;
            int sumaCalificaciones = 0;

            foreach (Contrato contrato in Singleton<CatalogoContrato>.Instance.ListaContratos)
            {
                if (contrato.CalificacionEmpleador != -1)
                {
                    calificacion += contrato.CalificacionEmpleador;
                    sumaCalificaciones += 1;
                }
                
            }

            try
            {
                calificacion = calificacion / sumaCalificaciones;
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("No hay calificaciones");
                calificacion = 0;
            }
            return calificacion;

        }

        /// <summary>
        /// Método ContratarServicio. Crea una solicitud de contratación y la agrega a la lista de solicitudes del trabajador dueño del servicio interesado.
        /// </summary>

        public void ContratarServicio(Servicio servicio)
        {
            SolicitudContratacion solicitud = new SolicitudContratacion(this, servicio);
            servicio.Trabajador.Solicitudes.Add(solicitud);
        }

        /// <summary>
        /// Método GetServicios. Retorna la lista de servicios.
        /// </summary>

        public List<Servicio> GetServicios()
        {
            List<Servicio> lista = Singleton<OfertaServicios>.Instance.ListaDeServicios;
            return lista;
        }

        /// <summary>
        /// Método GetCategoría. Retorna la lista de categorías.
        /// </summary>

        public List<Categoria> GetCategorias()
        {
            List<Categoria> lista = Singleton<ListaCategorias>.Instance.ListaDeCategorias;
            return lista;
        }

        /// <summary>
        /// Método GetServiciosPorCategoría. Llama al método FiltrarCategoría de la clase OfertaServicio, donde se ordenan los servicios según la categoría a fitrar.
        /// </summary>

        public List<Servicio> GetServiciosPorCategoria(Categoria categoria)
        {
            List<Servicio> lista = Singleton<OfertaServicios>.Instance.FiltrarCategoria(categoria);
            return lista;
        }

        /// <summary>
        /// Método GetServiciosPorReputacion. Llama al método FiltrarReputacion de la clase OfertaServicio, donde se ordenan los servicios según la reputación de los trabajadores.
        /// </summary>

        public List<Servicio> GetServiciosPorReputacion()
        {
            List<Servicio> lista = Singleton<OfertaServicios>.Instance.FiltrarReputacion();
            return lista;
        }

        /// <summary>
        /// Método GetServiciosPorUbicacion. Llama al método FiltrarUbicacion de la clase OfertaServicio, donde se ordenan los servicios según la ubicación en orden ascendente.
        /// </summary>

        public List<Servicio> GetServiciosPorUbicacion()
        {
            List<Servicio> lista = Singleton<OfertaServicios>.Instance.FiltrarUbicacion(this);
            return lista;
        }

        /// <summary>
        /// Método Contactar. Retorna el teléfono del trabajador.
        /// </summary>

        public string Contactar(Servicio servicio)
        {
            return servicio.Trabajador.Telefono;
        }
    }
}