using System;

namespace Chatbot
{
    /// <summary>
    /// Interfaz IUsuario. Implementamos estas interfaces en Trabajador, Empleador y Administración para cumplir con el principio ISP. Cada propiedad de esta interfaz es usada para todos las clases que la implementan.
    /// </summary>
    public interface IUsuario
    {
        public string Nombre {get;set;}
        public string Email {get;set;}

    }
}