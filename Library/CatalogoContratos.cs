using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chatbot
{
    public class CatalogoContrato : IJsonConvertible
    {
        [JsonInclude]
        public List<Contrato> ListaContratos = new List<Contrato>();


        public double GetCalificacion(IUsuario usuario)
        {
            double calificacion = 0;
            int sumaCalificaciones = 0;

            foreach (Contrato contrato in this.ListaContratos)
            {
                if (contrato.CalificacionEmpleador != null && usuario == contrato.Empleador)
                {
                    calificacion += contrato.CalificacionEmpleador;
                    sumaCalificaciones += 1;
                }
                else if (contrato.CalificacionTrabajador != null && usuario == contrato.Trabajador)
                {
                    calificacion += contrato.CalificacionTrabajador;
                    sumaCalificaciones += 1;
                }
            }

            try
            {
                calificacion = calificacion / sumaCalificaciones;
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("No hay calificaciones");
                calificacion = 0;
            }
            return calificacion;
        }    

        public List<Contrato> GetPendientes(IUsuario usuario)
        {
            List<Contrato> ListaPendientes = new List<Contrato>();
            foreach (Contrato contrato in this.ListaContratos)
            {
                if (contrato.CalificacionEmpleador == -1 || contrato.CalificacionTrabajador == -1 && contrato.Empleador == usuario || contrato.Trabajador == usuario)
                {
                    ListaPendientes.Add(contrato);
                }
            }
            return ListaPendientes;
        }  

        public void CalificarTrabajador(Contrato contrato, double calificacion)
        {
           List<Contrato> ListaPendientes = this.GetPendientes(contrato.Trabajador);
            if (ListaPendientes.Contains(contrato))
            {
                if(calificacion < 0 || calificacion > 5)
                {
                    throw new CheckCalificacion("Calificación inválida, debe ser entre 0 y 5");
                }
                else
                {
                    contrato.CalificacionTrabajador = calificacion;
                }
            }
        }
        public void CalificarEmpleador(Contrato contrato, double calificacion)
        {
            List<Contrato> ListaPendientes = this.GetPendientes(contrato.Empleador);
            if (ListaPendientes.Contains(contrato))
            {
                if(calificacion < 0 || calificacion > 5)
            {
                throw new CheckCalificacion("Calificación inválida, debe ser entre 0 y 5");
            }
            else
            {
                contrato.CalificacionEmpleador = calificacion;
            }
            }
        }

        public string ConvertToJson()
        {
            var opcion = new JsonSerializerOptions{WriteIndented = true};
            return JsonSerializer.Serialize(ListaContratos, opcion);
        }

        public void LoadFromJson(string json)
        {
            List<Contrato> deserialized = JsonSerializer.Deserialize<List<Contrato>>(json);
            Singleton<CatalogoContrato>.Instance.ListaContratos = deserialized;
        }
    }
}
