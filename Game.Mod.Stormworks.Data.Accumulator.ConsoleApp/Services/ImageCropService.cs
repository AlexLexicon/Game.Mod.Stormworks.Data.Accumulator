using System.Drawing;

namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Services;
public interface IIslandImageCropService
{
    Task CropAsync(string filePath);
}
public class ImageCropService : IIslandImageCropService
{
    public static bool IsHoverColor(Color color)
    {
        if (color.R is < 63 or > 64)
        {
            return false;
        }

        if (color.G is < 143 or > 145)
        {
            return false;
        }

        if (color.B is < 177 or > 179)
        {
            return false;
        }

        return true;
    }
    public static Color StormworksIslandAddonHoverRgbA { get; } = Color.FromArgb(64, 145, 179);
    public static Color StormworksIslandAddonHoverRgbB { get; } = Color.FromArgb(64, 144, 179);
    public static Color StormworksIslandAddonHoverRgbC { get; } = Color.FromArgb(64, 144, 178);

    public Task CropAsync(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);

        if (!fi.Exists || fi.DirectoryName is null)
        {
            throw new Exception("Invalid file path");
        }

        //you cannot override a bitmap file path so we create a temp copy to work from
        string tempFilePath = Path.Combine(fi.DirectoryName, $"temp.{DateTime.Now.Ticks}{fi.Extension}");
        File.Copy(filePath, tempFilePath);

        var image = new Bitmap(tempFilePath);

        Point? topLeft = null;
        Point? bottomRight = null;

        for (int row = 0; row < image.Height; row++)
        {
            Color previous = default;

            int? mapStartedColumn = null;
            bool firstBottomRight = false;
            for (int column = 0; column < image.Width; column++)
            {
                var rgb = image.GetPixel(column, row);

                bool isPreviousHover = IsHoverColor(previous);
                bool isNowHover = IsHoverColor(rgb);

                if (isPreviousHover && !isNowHover)
                {
                    mapStartedColumn = column;
                }

                if (mapStartedColumn is not null && isNowHover)
                {
                    int length = column - mapStartedColumn.Value;
                    if (length > 200)
                    {
                        topLeft ??= new Point(mapStartedColumn.Value, row);
                        if (!firstBottomRight && row < 450)
                        {
                            bottomRight = new Point(column, row + 1);
                            firstBottomRight = true;
                        }
                    }
                    else
                    {
                        mapStartedColumn = null;
                    }
                }

                previous = rgb;
            }
        }

        if (topLeft is not null && bottomRight is not null)
        {
            try
            {
                var newImage = image.Clone(new Rectangle
                {
                    X = topLeft.Value.X,
                    Y = topLeft.Value.Y,
                    Width = bottomRight.Value.X - topLeft.Value.X,
                    Height = bottomRight.Value.Y - topLeft.Value.Y,
                }, image.PixelFormat);

                newImage.Save(filePath);

                newImage.Dispose();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        image.Dispose();
        File.Delete(tempFilePath);

        return Task.CompletedTask;
    }
}
