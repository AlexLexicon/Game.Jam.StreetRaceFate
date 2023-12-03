using Game.Jam.StreetRaceFate.Application.Extensions;
using Game.Jam.StreetRaceFate.Application.Scenes;
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

//var xxx = app.Services.GetRequiredService<FramerateSpriteBatch>();
//var ttt = app.Services.GetRequiredService<FramerateService>();

windowsGameService.Run<RaceScene>();
