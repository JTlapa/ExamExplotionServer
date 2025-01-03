using log4net;
using log4net.Config;
using System;
using System.ServiceModel;

namespace Host
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ServerService.ServiceImplementation)))
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Info("Server iniciado");
                host.Open();
                Console.WriteLine("Server is running");
                Console.ReadLine();
            }
        }
    }
}
