using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Services;
public interface IIslandImageCropService
{
    Task CropAsync(string filePath);
}
public class ImageCropService : IIslandImageCropService
{
    private static Rgb StormworksIslandAddonHoverRgb { get; } = new Rgb
    {
        R = 64,
        G = 145,
        B = 179
    };

    public Task CropAsync(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);

        if (!fi.Exists || fi.DirectoryName is null)
        {
            throw new Exception("Invalid file path");
        }

        //you cannot override a bitmap file path so we create a temp copy to work from
        string tempFilePath = Path.Combine(fi.DirectoryName, $"temp.{fi.Extension}");
        File.Copy(filePath, tempFilePath);

        var image = new Bitmap(tempFilePath);

        // Lock the bitmap's bits.  
        Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
        BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        // Get the address of the first line.
        IntPtr ptr = bmpData.Scan0;

        // Declare an array to hold the bytes of the bitmap.
        int bytes = bmpData.Stride * image.Height;
        byte[] rgbValues = new byte[bytes];
        byte[] r = new byte[bytes / 3];
        byte[] g = new byte[bytes / 3];
        byte[] b = new byte[bytes / 3];

        // Copy the RGB values into the array.
        Marshal.Copy(ptr, rgbValues, 0, bytes);

        int count = 0;
        int stride = bmpData.Stride;

        Pos? topLeft = null;
        Pos? bottomRight = null;

        for (int column = 0; column < bmpData.Width; column++)
        {
            var previous = new Rgb
            {
                R = default,
                G = default,
                B = default,
            };
            Pos? possibleTopLeftForColumn = null;
            bool hasStartedInColumn = false;
            bool firstBottomRightInColumn = false;
            for (int row = 0; row < bmpData.Height; row++)
             {
                var rgb = new Rgb
                {
                    R = rgbValues[row * stride + column * 3 + 2],
                    G = rgbValues[row * stride + column * 3 + 1],
                    B = rgbValues[row * stride + column * 3 + 0],
                };

                if (rgb.Equals(StormworksIslandAddonHoverRgb))
                {

                }

                if (previous.Equals(StormworksIslandAddonHoverRgb) && !rgb.Equals(StormworksIslandAddonHoverRgb) && possibleTopLeftForColumn is null)
                {
                    hasStartedInColumn = true;
                    possibleTopLeftForColumn = new Pos
                    {
                        X = column,
                        Y = row,
                    };
                }

                if (!previous.Equals(StormworksIslandAddonHoverRgb) && rgb.Equals(StormworksIslandAddonHoverRgb))
                {
                    if (topLeft is null && possibleTopLeftForColumn is not null)
                    {
                        topLeft = possibleTopLeftForColumn;
                    }

                    if (!firstBottomRightInColumn && hasStartedInColumn)
                    {
                        firstBottomRightInColumn = true;
                        bottomRight ??= new Pos();

                        bottomRight.X = column;
                        bottomRight.Y = row;
                    }
                }

                previous = rgb;
                count++;
            }
        }

        if (topLeft is not null && bottomRight is not null)
        {
            var newImage = image.Clone(new Rectangle
            {
                X = topLeft.X,
                Y = topLeft.Y,
                Width = bottomRight.X - topLeft.X + 1,
                Height = bottomRight.Y - topLeft.Y,
            }, image.PixelFormat);

            newImage.Save(filePath);

            newImage.Dispose();
        }

        image.Dispose();
        File.Delete(tempFilePath);

        return Task.CompletedTask;
    }

    private class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    private readonly struct Rgb
    {
        public required byte R { get; init; }
        public required byte G { get; init; }
        public required byte B { get; init; }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Rgb other)
            {
                return other.R == R && other.G == G && other.B == B;
            }

            return false;
        }

        public override int GetHashCode() => R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
    }
}
