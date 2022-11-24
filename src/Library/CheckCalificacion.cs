namespace Chatbot
{
    /// <summary>
    /// Clase CheckCalificacion. Clase que hereda de Exception. Se creó para asegurar que las calificaciones dadas al momento de calificar un contrato sean válidas.
    /// </summary>

    [System.Serializable]
    public class CheckCalificacion : System.Exception
    {
        public CheckCalificacion()
        {

        }
        public CheckCalificacion(string message)
        : base(message)
        {
            
        }
        public CheckCalificacion(string message, System.Exception inner)
        : base(message, inner)
        {

        }
        protected CheckCalificacion(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context) 
        {

        }

        
    }
}