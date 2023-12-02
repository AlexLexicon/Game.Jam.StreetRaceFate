using Game.Jam.StreetRaceFate.Application;
using Game.Jam.StreetRaceFate.Application.Extensions;
using Game.Jam.StreetRaceFate.Engine;
using Game.Jam.StreetRaceFate.Engine.Extensions;
using Game.Jam.StreetRaceFate.Engine.Services;
using Game.Jam.StreetRaceFate.Windows;
using Lexicom.ConsoleApp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

var builder = ConsoleApplication.CreateBuilder();

builder.Services.AddEngine();
builder.Services.AddApplication();

builder.Services.AddSingleton<IWindowsGameService, WindowsGameService>();

var app = builder.Build();

var windowsGameService = app.Services.GetRequiredService<IWindowsGameService>();

windowsGameService.Initalize();

var xxx = app.Services.GetRequiredService<FramerateSpriteBatch>();
var ttt = app.Services.GetRequiredService<FramerateService>();
var scr = app.Services.GetRequiredService<Screen>();
var dsb = app.Services.GetRequiredService<DogsSpriteBatch>();
List<Dog> xx = new List<Dog>();
foreach (var x in Enumerable.Range(0, 1000))
{
    var myDog = app.Services.GetRequiredService<Dog>();

    xx.Add(myDog);  
}

windowsGameService.Run();
