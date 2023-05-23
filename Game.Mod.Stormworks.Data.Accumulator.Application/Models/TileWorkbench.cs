namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class TileWorkbench
{
    public required TransformPosition Position { get; set; }
    public required WorkbenchLookDirection LookDirection { get; set; }
    public required TileWorkbenchEditArea EditArea { get; set; }
}
