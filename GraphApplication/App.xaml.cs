﻿using GraphApplication.Algorithms;
using GraphApplication.Algorithms.Contracts;
using GraphApplication.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GraphApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>();

            services.AddScoped<IBFSAlgorithm, BFSAlgorithm>();
            services.AddScoped<IDFSAlgorithm, DFSAlgorithm>();
            services.AddScoped<IMinSpanningTreeAlgorithm, PrimAlgorithm>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
