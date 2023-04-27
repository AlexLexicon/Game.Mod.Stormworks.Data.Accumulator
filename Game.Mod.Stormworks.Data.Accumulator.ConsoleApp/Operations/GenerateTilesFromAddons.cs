using Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Models;
using Lexicom.ConsoleApp.Amenities;
using Lexicom.ConsoleApp.Tui;
using System.Text.Json;
using System.Xml;

namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Operations;
[TuiTitle("Step 1: Generate basic tile data from addons")]
public class GenerateTilesFromAddons : ITuiOperation
{
    public async Task ExecuteAsync()
    {
        Console.WriteLine("This operation will generate a json file containing basic tile data from missions (addons).");
        Console.WriteLine("also this will copy images from the missions locations with matching names to the mission folder.");
        Console.WriteLine();
        Console.WriteLine("For example:");
        Console.WriteLine("[...]\\AppData\\Roaming\\Stormworks\\data\\missions");
        Console.WriteLine("|--- a0");
        Console.WriteLine("     |--- playlist.xml");
        Console.WriteLine("     |--- script.lua");
        Console.WriteLine("|--- a0.png");
        Console.WriteLine("|--- b0");
        Console.WriteLine("     |--- playlist.xml");
        Console.WriteLine("     |--- script.lua");
        Console.WriteLine("|--- b0.png");
        Console.WriteLine();

        string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        string missionsDirectoryPath = Consolex.ReadLine("Enter the path to the directory containing missions to assemble the data from:", $"{userFolderPath}\\AppData\\Roaming\\Stormworks\\data\\missions");
        Console.WriteLine();

        string gameTilesDirectoryPath = Consolex.ReadLine("Enter the path to the stormworks tiles directory:", @"C:\Steam\steamapps\common\Stormworks\rom\data\tiles");
        Console.WriteLine();

        string[] subDirectories = Directory.GetDirectories(missionsDirectoryPath);
        string[] rootImages = Directory
            .GetFiles(missionsDirectoryPath)
            .Where(p => p.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        var tiles = new List<Tile>();
        foreach (string subDirectory in subDirectories)
        {
            string[] filePaths = Directory.GetFiles(subDirectory);

            string? playlistFilePath = filePaths.FirstOrDefault(p => p.EndsWith("playlist.xml"));

            string? name = null;
            string? xmlFileName = null;
            if (playlistFilePath is not null)
            {
                XmlDocument playlist = new XmlDocument();
                playlist.Load(playlistFilePath);

                XmlNodeList? nodeList = playlist.SelectNodes("playlist/locations/locations/l");
                if (nodeList is not null)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        name = node.Attributes?["name"]?.Value;
                        string? path = node.Attributes?["tile"]?.Value;

                        xmlFileName = Path.GetFileNameWithoutExtension(path);

                        break;
                    }
                }
            }

            string? imageFilePath = rootImages.FirstOrDefault(ip =>
            {
                string name = Path.GetFileNameWithoutExtension(ip);

                return subDirectory.EndsWith(name);
            });

            string[] gameTileFilePaths = Directory.GetFiles(gameTilesDirectoryPath);
            string? tileFilePath = gameTileFilePaths.FirstOrDefault(t => Path.GetFileNameWithoutExtension(t) == xmlFileName);

            int? type = null;
            bool isIsland = false;
            bool isPurchasable = false;
            int? purchaseCost = null;
            if (tileFilePath is not null)
            {
                //stormwork tile xml have elements with attributes that start with numbers so this doesnt work
                //XmlDocument playlist = new XmlDocument();
                //playlist.Load(tileFilePath);

                string[] rawXml = await File.ReadAllLinesAsync(tileFilePath);
                string? definitionLine = rawXml.FirstOrDefault(r => r.StartsWith("<definition"));
                try
                {
                    if (definitionLine is not null)
                    {
                        string? typeString = GetSlice(definitionLine, "tile_type=\"", "\"");
                        if (typeString is not null && int.TryParse(typeString, out int typeResult))
                        {
                            type = typeResult;
                        }
                        string? isIslandString = GetSlice(definitionLine, "is_island=\"", "\"");
                        if (isIslandString is not null && bool.TryParse(isIslandString, out bool isIslandResult))
                        {
                            isIsland = isIslandResult;
                        }
                        string? isPurchasableString = GetSlice(definitionLine, "is_purchasable=\"", "\"");
                        if (isPurchasableString is not null && bool.TryParse(isPurchasableString, out bool isPurchasableResult))
                        {
                            isPurchasable = isPurchasableResult;
                        }
                        string? purchaseCostString = GetSlice(definitionLine, "purchase_cost=\"", "\"");
                        if (purchaseCostString is not null && int.TryParse(purchaseCostString, out int purchaseCostResult))
                        {
                            purchaseCost = purchaseCostResult;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            tiles.Add(new Tile
            {
                Name = name?.ToLowerInvariant(),
                Image = imageFilePath,
                Xml = xmlFileName,
                Type = type,
                IsIsland = isIsland,
                IsPurchasable = isPurchasable,
                PurchaseCost = purchaseCost,
                IsStartable = false,
            });
        }

        string jsonFileName = $"tiles.json";
        string initalDirectory = $"{Environment.CurrentDirectory}\\stormworks.tiles\\{jsonFileName}";

        string outputFilePath = Consolex.ReadLine("Enter the directory you want to package the output to:", new ReadLineSettings
        {
            DefaultInput = jsonFileName,
            InitalInput = initalDirectory,
        });

        Console.WriteLine();
        Console.WriteLine("[Started] Generating output");

        FileInfo fi = new FileInfo(outputFilePath);

        if (fi.Directory is not null && fi.DirectoryName is not null && !fi.Directory.Exists)
        {
            Console.WriteLine($"  > The directory '{fi.DirectoryName}' did not exist so we are creating it");
            Directory.CreateDirectory(fi.DirectoryName);
        }

        foreach (Tile tile in tiles)
        {
            if (tile.Name is not null && tile.Image is not null && fi.DirectoryName is not null)
            {
                Console.WriteLine($"  > Copying the image '{tile.Image}' to the output");

                string imageName = tile.Name.Replace(" ", "");

                string destFilePath = Path.Combine(fi.DirectoryName, $"{imageName}.png");

                File.Copy(tile.Image, destFilePath);

                tile.Image = Path.GetFileName(destFilePath);
            }
        }

        Console.WriteLine("  > Generating the output json data");

        string jsonIslandData = JsonSerializer.Serialize(tiles);
        await File.WriteAllTextAsync(outputFilePath, jsonIslandData);

        Console.WriteLine("[Finished] Generating output");
    }

    private string? GetSlice(string input, string startString, string endString)
    {
        int start = input.IndexOf(startString);

        if (start < 0)
        {
            return null;
        }

        int begin = start + startString.Length;
        int end = input.IndexOf(endString, begin);

        if (end > 0 && end > begin)
        {
            return input[begin..end];
        }

        return null;
    }
}
