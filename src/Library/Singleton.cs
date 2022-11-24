using System;

namespace Chatbot
{
    /// <summary>
    /// Clase Singleton: La implementamos para poder utilizar el patrón Singleton en las clases.
    /// </summary>
    public class Singleton<T> where T : new()
    {
        private Singleton()
        {
        }
        private static T instance;
        public static T Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }
}