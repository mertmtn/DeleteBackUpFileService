using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DeleteBackUpFileService
{
    public class Worker : BackgroundService
    {
        private int _serviceRunInDays;
        private string _backUpFilePath;

        private IServiceScopeFactory _serviceScopeFactory;

        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var configuration = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            _backUpFilePath = configuration["FileConfigSettings:BackUpFilePath"];
            _serviceRunInDays = Convert.ToInt32(configuration["FileConfigSettings:RunIntervalInDays"]);
            return base.StartAsync(cancellationToken);
        }

        public void ClearBackUpFile()
        {
            string[] files = Directory.GetFiles(_backUpFilePath, "*.bak*");
            Array.ForEach(files, File.Delete);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ClearBackUpFile();
                await Task.Delay(TimeSpan.FromDays(_serviceRunInDays), stoppingToken);
            }
        }
    }
}
