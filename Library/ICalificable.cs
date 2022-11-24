using System;
using System.Collections.Generic;

namespace Chatbot
{
    /// <summary>
    /// Interfaz ICalificable. Implementamos estas interfaces en Trabajador y Empleador para cumplir con el principio ISP. Cada método de esta interfaz es usada para todos las clases que la implementan.
    /// </summary>
    public interface ICalificable
    {
        public double GetCalificacion();
    }
}