using System;
using Akka.Actor;
using Akka.Configuration;
using NLog;

namespace SharpWorkerNode
{
    public class WorkerService
    {
        private static ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly Config _config;

        private ActorSystem _system;

        public WorkerService(Config config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;
        }

        public bool Start()
        {
            _system = ActorSystem.Create(_config.GetString("actor-system-name"), _config);

            return true;
        }

        public bool Stop()
        {
            if (_system != null)
            {
                _system.Dispose();
                _system.WhenTerminated.Wait(TimeSpan.FromSeconds(_config.GetInt("service.wait-stop")));
            }

            return true;
        }

        public bool Pause()
        {
            return false;
        }

        public bool Continue()
        {
            return false;
        }

        public bool Shutdown()
        {
            return false;
        }
    }
}