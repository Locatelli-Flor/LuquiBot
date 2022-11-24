using Telegram.Bot.Types;
using System.Collections.Generic;
namespace Ucu.Poo.TelegramBot
{
    public class DiccionarioEstados
    {
        public Dictionary<long, int> Diccionario = new Dictionary<long, int>();
        public void ActualizarEstado(long id, int estado)
        {
            this.Diccionario[id] = estado;
        }

    }
}
