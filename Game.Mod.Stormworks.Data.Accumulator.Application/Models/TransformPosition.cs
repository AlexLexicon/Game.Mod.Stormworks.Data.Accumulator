namespace Game.Mod.Stormworks.Data.Accumulator.Application.Models;
public class TransformPosition
{
    public TransformPosition(
        double x,
        double y,
        double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double Xml_30 { get; set; }
    public double Xml_31 { get; set; }
    public double Xml_32 { get; set; }

    public double EastToWest
    {
        get => Xml_30;
        set => Xml_30 = value;
    }
    public double NorthToSouth
    {
        get => Xml_32;
        set => Xml_32 = value;
    }
    public double UpToDown
    {
        get => Xml_31;
        set => Xml_31 = value;
    }

    public double X
    {
        get => EastToWest;
        set => EastToWest = value;
    }
    public double Y
    {
        get => UpToDown;
        set => UpToDown = value;
    }
    public double Z
    {
        get => NorthToSouth;
        set => NorthToSouth = value;
    }
}
