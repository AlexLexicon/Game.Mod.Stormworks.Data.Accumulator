namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class TileDefinition
{
    public required bool IsStartable { get; set; }
    public required bool IsIsland { get; set; }
    public required int? Type { get; set; }
    public required bool IsPurchasable { get; set; }
    public required int? PurchaseCost { get; set; }
}
