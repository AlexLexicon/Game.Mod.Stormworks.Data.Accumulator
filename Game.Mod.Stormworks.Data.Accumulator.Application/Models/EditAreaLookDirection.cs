namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class EditAreaLookDirection
{
    public EditAreaLookDirection North { get; } = new EditAreaLookDirection("North", 0, 0, 0, 0);
    public EditAreaLookDirection East { get; } = new EditAreaLookDirection("East", 0, -1, 1, 0);
    public EditAreaLookDirection South { get; } = new EditAreaLookDirection("South", -1, 0, 0, -1);
    public EditAreaLookDirection West { get; } = new EditAreaLookDirection("West", 0, 1, -1, 0);

    private EditAreaLookDirection(
        string name,
        double xml_00,
        double xml_02,
        double xml_20,
        double xml_22)
    {
        Name = name;
        Xml_00 = xml_00;
        Xml_02 = xml_02;
        Xml_20 = xml_20;
        Xml_22 = xml_22;
    }

    public string Name { get; }
    public double Xml_00 { get; }
    public double Xml_02 { get; }
    public double Xml_20 { get; }
    public double Xml_22 { get; }
}
