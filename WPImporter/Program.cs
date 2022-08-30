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
services.AddDbContext<ImporterDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("WPIDatabase")));

// 3. Buduje "worek" zależności
var serviceProvider = services.BuildServiceProvider();

// 4. Pobieram context
var context = serviceProvider.GetService<ImporterDbContext>();

// 5. Tworzę obiekt aplikacji i przkazuje context
var app = new App(context);

// 6. Uruchamiam aplikację
app.StartImport().Wait();

Console.WriteLine("WP IMPORTER - END");