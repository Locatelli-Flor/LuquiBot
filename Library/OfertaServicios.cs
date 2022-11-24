using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase OfertaServicios. 
    /// Patrones y/o Principios utilizados para implementar la clase:
    /// Singleton: Como la ListaDeServicios y la ListaDeCategorías son únicas, se implementó la clase Singleton y cada vez que se hace referencia a estas listas se utiliza Singleton<OfertaServicios>.Instance.NombreDeLaLista.
    /// Expert: Como las listas de servicios y categorías solo se encuentran en esta clase, OfertaServicios tiene la responsabilidad de filtrarlas.
    /// SRP: La única responsabilidad que tiene esta clase es filtrar la ListaDeServicios y la ListaDeCategorías
    /// </summary>
    public class OfertaServicios : IJsonConvertible
    {
        [JsonInclude]
        public List<Servicio> ListaDeServicios = new List<Servicio>();

        /// <summary>
        /// Método FiltrarUbicación. Hace uso de la fórmula para calcular la distancia entr dos puntos (La distancia actual y la de cada servicio en la lista de servicios). Retorna una Lista con todos los servicios ordenados de manera ascendente por ubicación.
        /// </summary>

        public List<Servicio> FiltrarUbicacion(Empleador empleador)
        {
            List<(Servicio servicio, double ubicacion)> ListaOrdenada = new List<(Servicio servicio, double ubicacion)>{};
            List<Servicio> ListaFiltrada = new List<Servicio>{};

            foreach(Servicio servicio in this.ListaDeServicios)
            { 
                double coordenadaX = servicio.Trabajador.Ubicacion.Item1;
                double coordenadaY = servicio.Trabajador.Ubicacion.Item2;
                
                double DiferenciaDistanciaX = coordenadaX - empleador.Ubicacion.Item1;
                double DiferenciaDistanciaY = coordenadaY - empleador.Ubicacion.Item2;

                double PotenciaX = Math.Pow(DiferenciaDistanciaX, 2);
                double PotenciaY = Math.Pow(DiferenciaDistanciaY, 2);

                double DistanciaFinal = Math.Sqrt(PotenciaX + PotenciaY);

                (Servicio servicio, double ubicacion) Tupla = new (servicio, DistanciaFinal);

                ListaOrdenada.Add(Tupla);
            }
            // Esta línea la sacamos de internet para poder ordenar una lista de tuplas según el segundo elemento.
            ListaOrdenada.Sort((x, y) => y.Item2.CompareTo(x.Item2));

            foreach((Servicio servicio, double ubicacion) tuple in ListaOrdenada)
            {
                ListaFiltrada.Add(tuple.servicio);
            }
            ListaFiltrada.Reverse();
        
            return ListaFiltrada;
        }
        
        /// <summary>
        /// Método FiltrarReputación. Ordena la Lista de servicios según la calificación total de cada trabajador y la retorna.
        /// </summary>

        public List<Servicio> FiltrarReputacion()
        {
            List<Servicio> listaFiltrada = Singleton<OfertaServicios>.Instance.ListaDeServicios.OrderByDescending(p => p.Trabajador.GetCalificacion()).ToList();   
            return listaFiltrada;
        }

        /// <summary>
        /// Método FiltrarCategoría. Crea una lista donde se van a agregar aquellos servicios que coincidan con la Categoría a filtrar y la retorna.
        /// </summary>

        public List<Servicio> FiltrarCategoria(Categoria CategoriaAFiltrar)
        {
            List<Servicio> ListaFiltrada = new List<Servicio>();

            foreach(Servicio servicio in ListaDeServicios)
            {
                if (servicio.Categoria == CategoriaAFiltrar)
                {
                    ListaFiltrada.Add(servicio);
                }
            }
            return ListaFiltrada;
        }
        public string ConvertToJson()
        {
            var opcion = new JsonSerializerOptions{WriteIndented = true};
            return JsonSerializer.Serialize(this, opcion);
        }

        public void LoadFromJson(string json)
        {
            OfertaServicios deserialized = JsonSerializer.Deserialize<OfertaServicios>(json);
            Singleton<OfertaServicios>.Instance.ListaDeServicios = deserialized.ListaDeServicios;
        }
    }

}