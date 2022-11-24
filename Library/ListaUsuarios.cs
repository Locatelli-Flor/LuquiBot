using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    public class ListaUsuarios : IJsonConvertible
    {
        [JsonInclude]
        public List<Trabajador> ListaTrabjadores = new List<Trabajador>();

        [JsonInclude]
        public List<Empleador> ListaEmpleadores = new List<Empleador>();

        [JsonInclude]
        public List<Administrador> ListaAdmins = new List<Administrador>();

        public string ConvertToJson()
        {
            var opcion = new JsonSerializerOptions{WriteIndented = true};
            return JsonSerializer.Serialize(this, opcion);
        }

        public void LoadFromJson(string json)
        {
            ListaUsuarios deserialized = JsonSerializer.Deserialize<ListaUsuarios>(json);

            Singleton<ListaUsuarios>.Instance.ListaAdmins = deserialized.ListaAdmins;
            Singleton<ListaUsuarios>.Instance.ListaEmpleadores = deserialized.ListaEmpleadores;
            Singleton<ListaUsuarios>.Instance.ListaTrabjadores = deserialized.ListaTrabjadores;
        }
    }
}