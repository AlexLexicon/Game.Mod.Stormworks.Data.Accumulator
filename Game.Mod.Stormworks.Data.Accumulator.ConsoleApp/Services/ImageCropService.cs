using System.Drawing;

namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Services;
public interface IIslandImageCropService
{
    Task CropAsync(string filePath);
}
public class ImageCropService : IIslandImageCropService
{

    public static Color StormworksIslandAddonHoverRgbA { get; } = Color.FromArgb(64, 145, 179);
    public static Color StormworksIslandAddonHoverRgbB { get; } = Color.FromArgb(64, 144, 179);
    public static Color StormworksIslandAddonHoverRgbC { get; } = Color.FromArgb(64, 144, 178);

    public Task CropAsync(string filePath)
    {

    }
}
