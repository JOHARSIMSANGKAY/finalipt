using Microsoft.Extensions.Hosting;
using Sangkay.Domain.Entities;
using System.Windows;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sangkay.Framework.Data;
using Sangkay.Framework.Interfaces;
using Sangkay.Framework.Repositories;

namespace Sangkay.Supplier
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // DbContext
                    var conn = context.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<SangkayDbContext>(options => options.UseSqlServer(conn));

                    // Repos
                    services.AddScoped<ISupplierRepository, SupplierRepository>();

                    // ViewModels and Views
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<SupplierViewModel>();
                })
                .Build();

            await _host.StartAsync();

            // Show main window
            var mw = _host.Services.GetRequiredService<MainWindow>();
            mw.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            base.OnExit(e);
        }
    }
}