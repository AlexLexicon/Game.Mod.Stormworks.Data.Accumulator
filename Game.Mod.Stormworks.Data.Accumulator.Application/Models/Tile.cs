namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class Tile
{
    public required string? Name { get; set; }
    public required string? Image { get; set; }
    public required string? Xml { get; set; }

    public required TileDefinition Definition { get; set; }
    public required IEnumerable<TileWorkbench> Workbenches { get; set; }


    public required StaticLocation StaticLocation { get; set; }
}
