using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase Trabajador.
    /// Principios y/o Patrones utilizados para implementar la clase:
    /// Creator: Trabajador contiene objetos de clase Contrato, por lo que tiene la responsabilidad de confirmar la solicitud y crear el contrato
    /// ISP: Los tipos que implementa la clase los utiliza. Por ejemplo: ICalificable y los métodos Calificar y GetCalificacion.
    /// </summary>
    public class Trabajador : IUsuario, ICalificable, IJsonConvertible
    {
        public string Nombre {get;set;}
        public string Email {get;set;}
        public string Contraseña {get;set;}
        public string Telefono {get;set;}
        public Tuple<double, double> Ubicacion {get;set;}


        /// <summary>
        /// Esta lista de Solicitudes tiene un registro de aquellos empleadores que estén interesados en contratar un servicio del trabajador.
        /// </summary>
        public List<SolicitudContratacion> Solicitudes = new List<SolicitudContratacion>();

        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        
        [JsonConstructor]
        public Trabajador(string nombre, string email, string telefono, Tuple<double,double> Ubicacion)
        {
            this.Nombre = nombre;
            this.Email = email;
            this.Telefono = telefono;
            this.Ubicacion = Ubicacion;
        }

        public Trabajador(string json)
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
            this.Telefono = deserialized.Telefono;
            this.Ubicacion = deserialized.Ubicacion;

        }


        /// <summary>
        /// Método GetCategorías. Retorna la única lista de categorías existente.
        /// </summary>

        public List<Categoria> GetCategorias()
        {
            List<Categoria> lista = Singleton<ListaCategorias>.Instance.ListaDeCategorias;
            return lista;
        }

        /// <summary>
        /// Método GetCalificación. Itera sobre todos los contratos del trabajador y si estos todavía no fueron calificados no los tiene en cuenta. En el caso contrario, los suma al total de calificaciones y aumenta el contador de calificaciones. Luego retorna el promedio de calificaciones.
        /// </summary>

        public double GetCalificacion()
        {
            double calificacion = 0;
            int sumaCalificaciones = 0;

            foreach (Contrato contrato in Singleton<CatalogoContrato>.Instance.ListaContratos)
            {
                if (contrato.CalificacionTrabajador != -1)
                {
                    calificacion += contrato.CalificacionTrabajador;
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
        /// Método CrearServicio. Crea una instancia de Servicio con los parámetros dados y lo agrega a la única lista de servicios.
        /// </summary>
        public void CrearServicio(string descripcion, Categoria categoria, int precio)
        {
            Servicio servicioCreado = new Servicio(descripcion, categoria, this, precio);
            Singleton<OfertaServicios>.Instance.ListaDeServicios.Add(servicioCreado);
        }

        /// <summary>
        /// Método QuitarServicio. Primero se asegura que el servicio a remover sea de propiedad del trabajador que llama al método. Si esto se cumple, lo elimina de la única lista de servicios.
        /// </summary>

        public void QuitarSuServicio(Servicio servicio)
        {
            if (this == servicio.Trabajador)
            {
                Singleton<OfertaServicios>.Instance.ListaDeServicios.Remove(servicio); 
            }
        }

        /// <summary>
        /// Método AceptarSolicitud. Entra como parámetro un objeto de la clase SolicitudContratacion. Este contiene el servicio interesado y el empleador interesado. Si el trabajador decide aceptar la solicitud, se crea un Contrato con los datos de la solicitud y el trabajador, y luego lo añade a la lista de contratos del empleador interesado. Por último, la solicitud se elimina de la lista de solicitudes pendientes del trabajador.
        /// </summary>

        public void AceptarSolicitud(SolicitudContratacion solicitud, bool decision)
        {
            if(decision == true)
            {
                Contrato contrato = new Contrato(this, solicitud.EmpleadorInteresado, solicitud.ServicioInteresado);
                Singleton<CatalogoContrato>.Instance.ListaContratos.Add(contrato);
            }
            this.Solicitudes.Remove(solicitud);
        }

    }
}