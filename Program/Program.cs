//--------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//--------------------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Ucu.Poo.Locations.Client;
using Chatbot;

namespace Ucu.Poo.TelegramBot
{
    /// <summary>
    /// Un programa que implementa un bot de Telegram.
    /// </summary>
    public class Program
    {
        // La instancia del bot.
        private static TelegramBotClient Bot;

        // El token provisto por Telegram al crear el bot. Mira el archivo README.md en la raíz de este repo para
        // obtener indicaciones sobre cómo configurarlo.
        private static string token;

        // Esta clase es un POCO -vean https://en.wikipedia.org/wiki/Plain_old_CLR_object- para representar el token
        // secreto del bot.
        private class BotSecret
        {
            public string Token { get; set; }
        }

        // Una interfaz requerida para configurar el servicio que lee el token secreto del bot.
        private interface ISecretService
        {
            string Token { get; }
        }

        // Una clase que provee el servicio de leer el token secreto del bot.
        private class SecretService : ISecretService
        {
            private readonly BotSecret _secrets;

            public SecretService(IOptions<BotSecret> secrets)
            {
                _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
            }

            public string Token { get { return _secrets.Token; } }
        }

        // Configura la aplicación.
        private static void Start()
        {
            // Lee una variable de entorno NETCORE_ENVIRONMENT que si no existe o tiene el valor 'development' indica
            // que estamos en un ambiente de desarrollo.
            var developmentEnvironment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment =
                string.IsNullOrEmpty(developmentEnvironment) ||
                developmentEnvironment.ToLower() == "development";

            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // En el ambiente de desarrollo el token secreto del bot se toma de la configuración secreta
            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            var configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            // Mapeamos la implementación de las clases para  inyección de dependencias
            services
                .Configure<BotSecret>(configuration.GetSection(nameof(BotSecret)))
                .AddSingleton<ISecretService, SecretService>();

            var serviceProvider = services.BuildServiceProvider();
            var revealer = serviceProvider.GetService<ISecretService>();
            token = revealer.Token;
        }

        private static IHandler firstHandler;
        public static void Main()
        {

           /*  Administrador admin = new Administrador("León", "leon.salvo@correo.ucu.edu.uy");
            Console.WriteLine("Se registro un nuevo Administrador");
            Singleton<ListaUsuarios>.Instance.ListaAdmins.Add(admin);

            Categoria jardineria = new Categoria("Jardineria");
            Console.WriteLine("Se crea la categoria Jardinería");
            Categoria mecanica = new Categoria("Mecanica");
            Console.WriteLine("Se crea la categoria Mecánica");
            Categoria plomeria = new Categoria("Plomeria");
            Console.WriteLine("Se crea la categoria Plomeria");

            admin.AgregarCategoria((jardineria));
            admin.AgregarCategoria(mecanica);
            admin.AgregarCategoria(plomeria);
            Console.WriteLine("El admin agrega las categorias a la lista de categorias");
            
            
            Trabajador trabajador1 = new Trabajador("Pedro", "Pedro@correo.com", "094999999", new Tuple<double, double>(33.548,54.895));
            Trabajador trabajador2 = new Trabajador("Mariana", "Mariana@correo.com", "094999888", new Tuple<double, double>(33.548,54.895));
            Console.WriteLine("Se registro un nuevo trabajador");
            Console.WriteLine("Se registro un nuevo trabajador");
            Singleton<ListaUsuarios>.Instance.ListaTrabjadores.Add(trabajador1);
            Singleton<ListaUsuarios>.Instance.ListaTrabjadores.Add(trabajador2);


            trabajador2.CrearServicio("Jardinera de pasion", jardineria,5500);
            trabajador1.CrearServicio("Mecanico con 10 años de experiencia",mecanica, 8000);
            Console.WriteLine($"{trabajador1.Nombre} creo un nuevo servicio de {mecanica.Categ}");
            Console.WriteLine($"{trabajador2.Nombre} creo un nuevo servicio de {jardineria.Categ}");

            Empleador empleador1 = new Empleador("Marcelo","Marcelo@correo.com", "099888999",new Tuple<double, double>(35.268,57.235));
            Empleador empleador2 = new Empleador("Jorge","Jorge@correo.com", "099887599",new Tuple<double, double>(32.268,57.235));
            Console.WriteLine("Se registro un nuevo empleador");
            Console.WriteLine("Se registro un nuevo empleador");
            Singleton<ListaUsuarios>.Instance.ListaEmpleadores .Add(empleador1);
            Singleton<ListaUsuarios>.Instance.ListaEmpleadores.Add(empleador2);


            List<Servicio> listaServicios = empleador1.GetServicios();
            Console.WriteLine($"{empleador1.Nombre} mira los servicios disponibles");

            empleador1.Contactar(listaServicios[1]);
            Console.WriteLine($"{empleador1.Nombre} se pone en contacto con {listaServicios[1].Trabajador.Nombre}");

            empleador1.ContratarServicio(listaServicios[1]);
            trabajador1.AceptarSolicitud(trabajador1.Solicitudes[0], true);
            Console.WriteLine($"{empleador1.Nombre} contrata a {trabajador1.Nombre}");

            empleador1.Calificar(empleador1.Contratos[0], 4.5);
            trabajador1.Calificar(empleador1.Contratos[0], 5);
            Console.WriteLine("Despues de terminar el trabajo se califican");

            List<Servicio> listaFiltrada = empleador2.GetServiciosPorReputacion();
            Console.WriteLine($"{empleador2.Nombre} busca servicios por reputacion");
            empleador2.Contactar(listaFiltrada[0]);
            Console.WriteLine($"{empleador2.Nombre} contacta con el trabajador con mejor reputación");
            empleador2.Contactar(listaFiltrada[0]);
            empleador2.ContratarServicio(listaFiltrada[0]);
            trabajador1.AceptarSolicitud(trabajador1.Solicitudes[0], true);
            Console.WriteLine($"{empleador2.Nombre} contrata a {listaFiltrada[0].Trabajador.Nombre}");

            empleador2.Calificar(empleador2.Contratos[1], 3);
            trabajador1.Calificar(trabajador1.Contratos[1], 4.5);
            Console.WriteLine("Despues de terminar el trabajo se califican");*/


            Start();
            LocationApiClient client = new LocationApiClient();

            Bot = new TelegramBotClient(token);
            
            string jsonListaCat = System.IO.File.ReadAllText(@"..\..\serializacion\ListaDeCategorias.json");
            Singleton<ListaCategorias>.Instance.LoadFromJson(jsonListaCat);

            string jsonListaContratos = System.IO.File.ReadAllText(@"..\..\serializacion\ListaDeContratos.json");
            //Singleton<CatalogoContrato>.Instance.LoadFromJson(jsonListaContratos);

            firstHandler =
                new HelpHandler(
                new HelloHandler(
                new AgregarCatHandler(
                new VerCategoriaHandler(
                new RegistroHandler(
                new PhotoHandler(null,
                new GoodByeHandler(
                new QuitarCatHandler(
                new AddressHandler(new AddressFinder(client),
                new DistanceHandler(new DistanceCalculator(client), null)
                )))))))));

            var cts = new CancellationTokenSource();



            // Comenzamos a escuchar mensajes. Esto se hace en otro hilo (en background). El primer método
            // HandleUpdateAsync es invocado por el bot cuando se recibe un mensaje. El segundo método HandleErrorAsync
            // es invocado cuando ocurre un error.
            Bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cts.Token
            );

            Console.WriteLine($"Bot is up!");

            // Esperamos a que el usuario aprete Enter en la consola para terminar el bot.
            Console.ReadLine();

            // Terminamos el bot.
            cts.Cancel();
        }

        /// <summary>
        /// Maneja las actualizaciones del bot (todo lo que llega), incluyendo mensajes, ediciones de mensajes,
        /// respuestas a botones, etc. En este ejemplo sólo manejamos mensajes de texto.
        /// </summary>
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                // Sólo respondemos a mensajes de texto
                if (update.Type == UpdateType.Message)
                {
                    await HandleMessageReceived(botClient, update.Message);
                }
            }
            catch(Exception e)
            {
                await HandleErrorAsync(botClient, e, cancellationToken);
            }
        }

        /// <summary>
        /// Maneja los mensajes que se envían al bot a través de handlers de una chain of responsibility.
        /// </summary>
        /// <param name="message">El mensaje recibido</param>
        /// <returns></returns>
        private static async Task HandleMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Received a message from {message.From.FirstName} saying: {message.Text}");

            string response = string.Empty;

            firstHandler.Handle(message, out response);

            if (!string.IsNullOrEmpty(response))
            {
                await Bot.SendTextMessageAsync(message.Chat.Id, response);
            }
        }

        /// <summary>
        /// Manejo de excepciones. Por ahora simplemente la imprimimos en la consola.
        /// </summary>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.Message);
            return Task.CompletedTask;
        }
    }
}
