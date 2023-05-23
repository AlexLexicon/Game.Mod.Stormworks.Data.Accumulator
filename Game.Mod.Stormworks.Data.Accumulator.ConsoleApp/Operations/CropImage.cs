using Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Services;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;

namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Operations;
[TuiPage("Generate Package")]
[TuiTitle("Step 2: Crop all tile pngs")]
public class CropImage : ITuiOperation
{
    private readonly IIslandImageCropService _islandImageCropService;

    public CropImage(IIslandImageCropService islandImageCropService)
    {
        _islandImageCropService = islandImageCropService;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("This operation will crop all tile images from the addon menu in game with the mouse hover blue color.");
        Console.WriteLine();

        Console.WriteLine("Found possible output folders:");
        foreach (string subDirectory in Directory.GetDirectories(Environment.CurrentDirectory))
        {
            if (subDirectory.Contains("stormworks.island.data."))
            {
                Console.WriteLine(subDirectory);
            }
        }
        Console.WriteLine();

        string initalDirectory = $"{Environment.CurrentDirectory}\\stormworks.tiles";
        string outputFilePath = Consolex.ReadLine("Enter the directory you want to package the output to:", initalDirectory);

        Console.WriteLine();
        Console.WriteLine("[Starting] crops");
        foreach (string file in Directory.GetFiles(outputFilePath))
        {
            if (file.EndsWith(".png"))
            {
                Console.WriteLine($" > Cropping image: '{file}'");

                await _islandImageCropService.CropAsync(file);
            }
        }
        Console.WriteLine("[Finishing] crops");
    }
}
