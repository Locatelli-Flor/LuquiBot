using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    /// <summary>
    /// Clase Contrato.
    /// </summary>
    public class Contrato : IJsonConvertible
    {
        public Trabajador Trabajador {get;set;}
        public Empleador Empleador {get;set;}
        public Servicio Servicio {get;set;}
        public double CalificacionEmpleador {get;set;}
        public double CalificacionTrabajador {get;set;}
        public DateTime Fecha {get;set;}

        /// <summary>
        /// El constructor de la clase.
        /// </summary>
        
        [JsonConstructor]
        public Contrato (Trabajador trabajador, Empleador empleador, Servicio servicio)
        {
            this.Trabajador = trabajador;
            this.Empleador = empleador;
            this.Servicio = servicio;
            this.Fecha = DateTime.Now;
            this.CalificacionEmpleador = -1;
            this.CalificacionTrabajador = -1;
        }

        public Contrato(string json)
        {
            this.LoadFromJson(json);
        }
        public string ConvertToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public void LoadFromJson(string json)
        {
            Contrato deserialized = JsonSerializer.Deserialize<Contrato>(json);
            this.Trabajador = deserialized.Trabajador;
            this.Empleador = deserialized.Empleador;
            this.Servicio = deserialized.Servicio;
            this.Fecha = deserialized.Fecha;
            this.CalificacionEmpleador = deserialized.CalificacionEmpleador;
            this.CalificacionTrabajador = deserialized.CalificacionTrabajador;

        }
    }
}