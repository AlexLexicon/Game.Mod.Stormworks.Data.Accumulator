namespace Game.Mod.Stormworks.Data.Accumulator.ConsoleApp.Models;
public class Tile
{
    public required string? Name { get; set; }
    public required string? Image { get; set; }
    public required string? Xml { get; set; }
    public required int? Type { get; set; }
    public required bool IsPurchasable { get; set; }
    public required int? PurchaseCost { get; set; }
    public required bool IsIsland { get; set; }
    public required bool IsStartable { get; set; }
}
