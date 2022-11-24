using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Chatbot
{
    public class ListaCategorias : IJsonConvertible
    {
        [JsonInclude]
        public List<Categoria> ListaDeCategorias = new List<Categoria>(); 

        public string Imprimir()
        {
            List<string> listaImprimir = new List<string>{};
            foreach(Categoria cat in ListaDeCategorias)
            {
                listaImprimir.Add(cat.Categ);
            }
            string CadenaCategorias = string.Join("\n", listaImprimir);
            return CadenaCategorias;
        }
        public string ConvertToJson()
        {
            var opcion = new JsonSerializerOptions{WriteIndented = true};
            Console.WriteLine(JsonSerializer.Serialize(ListaDeCategorias, opcion));
            return JsonSerializer.Serialize(ListaDeCategorias, opcion);
        }

        public void LoadFromJson(string json)
        {
            List<Categoria> deserialized = JsonSerializer.Deserialize<List<Categoria>>(json);
            Singleton<ListaCategorias>.Instance.ListaDeCategorias = deserialized;
        }

    }
}