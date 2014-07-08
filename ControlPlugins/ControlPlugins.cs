namespace ControlPlugins
{

    public interface Label
    {
        string text { get; }
        ushort width { get; }
        ushort height { get; }
        ushort xPos { get; }
        ushort yPos { get; }
        string command { get; }
        void set(string value);
    }

    public interface Button
    {
        string text { get; }
        ushort width { get; }
        ushort height { get; }
        ushort xPos { get; }
        ushort yPos { get; }
        string command { get; }
        void click();
    }

    public interface IControl
    {
        string name { get; }
        Label[] labels { get; }
        Button[] buttons { get; }
    }
}
