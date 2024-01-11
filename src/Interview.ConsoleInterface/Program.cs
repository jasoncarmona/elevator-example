using CommandLine;
using Interview.ConsoleInterface;
using Interview.Infrastructure.Files;
using Interview.Infrastructure.Services;
using Interview.Application.Common.Interfaces;
using Interview.Application.Elevator;
using Interview.Domain;
using Autofac;
using static System.Formats.Asn1.AsnWriter;


namespace Interview.Application {
   public class Program {

      //private static IContainer _container;
      private static IContainer? Container { get; set; }


      static void Main(string[] args) {

         Container = ConfigureDependencies();

         using (var scope = Container.BeginLifetimeScope()) {
            var logger = scope.Resolve<ILogger>();
            
            logger.Log("teseting resolution");
         }

         CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
      }

      private static IContainer ConfigureDependencies() {
         // Register default dependencies in the application container.
         var builder = new ContainerBuilder();
         builder.RegisterType<Logger>().As<ILogger>().InstancePerDependency();
         builder.RegisterType<CsvFileGenerator>().As<ICsvFileGenerator>().SingleInstance();
         builder.RegisterType<CsvFileReader>().As<ICsvFileReader>().InstancePerDependency();
         builder.RegisterType<ElevatorController>().As<IElevatorController>().InstancePerDependency();

         var appContainer = builder.Build();

         return appContainer;
      }


      static void RunOptions(CommandLineOptions opts) {
         using (var scope = Container!.BeginLifetimeScope()) {
            var logger = scope.Resolve<ILogger>();
            var csvFileReader = scope.Resolve<ICsvFileReader>();
            var elevatorController = scope.Resolve<IElevatorController>();
            var csvFileGenerator = scope.Resolve<ICsvFileGenerator>();
            csvFileGenerator.PathToFile = opts.OutputFilePath;

            logger.Log($"Input file: {opts.InputFilePath}");
            var elevatorRequests = csvFileReader.ReadElevatorRequestsFile(opts.InputFilePath);

            elevatorController.RawElevatorRequests = new Queue<ElevatorRequest>(elevatorRequests!);
            elevatorController.CsvFileGenerator = csvFileGenerator;
            elevatorController.Logger = logger;
            elevatorController.ProcessRequests();

            //foreach (var request in elevatorRequests!) logger.Log(request.ToString());
         }
      }

      static void HandleParseError(IEnumerable<Error> errs) {
         //handle errors
      }
   }
}