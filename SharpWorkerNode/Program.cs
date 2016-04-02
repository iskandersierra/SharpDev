using System;
using Akka.Configuration;
using NLog;
using SharpDev;
using StackExchange.Exceptional;
using StackExchange.Exceptional.Stores;
using Topshelf;

namespace SharpWorkerNode
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.Load();
            config = config.WithFallback(GetFallbackConfig());

            LogManager.GetCurrentClassLogger().Trace(config);

            ConfigExceptionHandling(config);

            HostFactory.Run(h =>
            {
                h.UseNLog();

                h.Service<WorkerService>(s =>
                {
                    // Use DI
                    s.ConstructUsing(() => new WorkerService(config));
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());
                    //s.WhenPaused((service, control) => service.Pause());
                    //s.WhenContinued((service, control) => service.Continue());
                    //s.WhenShutdown((service, control) => service.Shutdown());
                });

                //h.EnablePauseAndContinue();
                //h.EnableShutdown();
                h.SetServiceName(config.GetString("service.name"));
                h.SetDisplayName(config.GetString("service.display"));
                h.SetInstanceName(config.GetString("service.instance"));
                h.SetDescription(config.GetString("service.description"));
                h.RunAsLocalService();
                h.StartAutomatically();
                h.EnableServiceRecovery(r =>
                {
                    r.RestartService(0);
                    r.RestartService(1);
                    r.RestartService(10);
                    r.SetResetPeriod(1);
                });

            });
        }

        [ThreadStatic]
        private static bool isLoggingException = false;
        private static void ConfigExceptionHandling(Config config)
        {
            ErrorStore.Setup(config.GetString("service.name"),
                new JSONErrorStore(config.GetString("service.exceptions-path"), 200));

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (isLoggingException) return;
                using (new DelegateDisposable(() => isLoggingException = true, () => isLoggingException = false))
                {
                    var ex = e.ExceptionObject as Exception;
                    if (ex != null)
                        ErrorStore.LogExceptionWithoutContext(ex);
                }
            };

            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (isLoggingException) return;
                using (new DelegateDisposable(() => isLoggingException = true, () => isLoggingException = false))
                {
                    var ex = e.Exception;
                    if (ex != null)
                        ErrorStore.LogExceptionWithoutContext(ex);
                }
            };
        }

        private static Config GetFallbackConfig()
        {
            return ConfigurationFactory.ParseString(@"
            {
                service {
                    name = shwrksvc
                    display = Sharp worker service
                    instance = shwrksvc
                    description = Worker service for SharpDev framework
                    wait-stop = 10
                    actor-system-name = SharpActors
                    exceptions-path = errors
                }

                akka.loggers = [""Akka.Loggers.NLog.NLogLogger, Akka.Logger.NLog""]
            }
                ");
        }
    }
}
