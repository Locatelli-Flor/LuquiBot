using System;

namespace Chatbot
{
    /// <summary>
    /// Clase SolicitudContratacion.
    /// </summary>
    public class SolicitudContratacion
    {
        public Empleador EmpleadorInteresado {get;set;}
        public Servicio ServicioInteresado {get;set;}

        /// <summary>
        /// El constructor de la clase.
        /// </summary>

        public SolicitudContratacion(Empleador empleadorInteresado, Servicio servicioInteresado)
        {
            this.EmpleadorInteresado = empleadorInteresado;
            this.ServicioInteresado = servicioInteresado;
        }

    }
}