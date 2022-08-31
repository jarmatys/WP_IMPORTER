using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WPImporter;
using WPImporter.Database;

Console.WriteLine("WP IMPORTER - START");

// 1. Pobieram appsettings
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var services = new ServiceCollection();

// 2. Przekazuje context bazy danych do Entity Frameworka
services.AddSingleton<App>();

services.AddDbContext<IImportedDbContext, ImporterDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("WPIDatabase")));

// 3. Buduje "worek" zależności
var serviceProvider = services.BuildServiceProvider();

// 4. Pobieram context
var app = serviceProvider.GetService<App>();

if (app != null)
{
    app.StartApplication();
}

Console.WriteLine("WP IMPORTER - END");