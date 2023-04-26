using Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Services;
using Lexicom.AspNetCore.Controllers.Extensions;
using Lexicom.ConsoleApp.DependencyInjection;
using Lexicom.ConsoleApp.Tui.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = ConsoleApplication.CreateBuilder();

builder.Lexicom(options =>
{
    options.AddTui<Program>();
});

builder.Services.AddSingleton<IIslandImageCropService, ImageCropService>();

var app = builder.Build();

await app.RunLexicomTuiAsync();
