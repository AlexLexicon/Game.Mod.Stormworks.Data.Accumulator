namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class WorkbenchLookDirection
{
    public WorkbenchLookDirection North { get; } = new WorkbenchLookDirection("North", 0, 1, -1, 0);
    public WorkbenchLookDirection East { get; } = new WorkbenchLookDirection("East", -1, 0, 0, -1);
    public WorkbenchLookDirection South { get; } = new WorkbenchLookDirection("South", 0, -1, 1, 0);
    public WorkbenchLookDirection West { get; } = new WorkbenchLookDirection("West", 1, 0, 0, -1);

    private WorkbenchLookDirection(
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
