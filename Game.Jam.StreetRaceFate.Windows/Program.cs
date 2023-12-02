using Game.Jam.StreetRaceFate.Application;
using Game.Jam.StreetRaceFate.Application.Extensions;
using Game.Jam.StreetRaceFate.Engine.Extensions;
using Game.Jam.StreetRaceFate.Windows;
using Lexicom.ConsoleApp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var builder = ConsoleApplication.CreateBuilder();

builder.Services.AddEngine();
builder.Services.AddApplication();

builder.Services.AddSingleton<IWindowsGameService, WindowsGameService>();

var app = builder.Build();

var windowsGameService = app.Services.GetRequiredService<IWindowsGameService>();

windowsGameService.Initalize();

var dsb = app.Services.GetRequiredService<DogsSpriteBatch>();
var myDog = app.Services.GetRequiredService<Dog>();
var mydog2 = app.Services.GetRequiredService<Dog>();
var mydog3 = app.Services.GetRequiredService<Dog>();

windowsGameService.Run();
