namespace ControlPlugins
{
    public interface Label
    {
        string text { get; }
        int width { get; }
        int height { get; }
        int x_pos { get; }
        int y_pos { get; }
        void set(string value);
    }

    public interface Button
    {
        string text { get; }
        int width { get; }
        int height { get; }
        int x_pos { get; }
        int y_pos { get; }
        void click();
    }

    public interface IControl
    {
        string name { get; }
        Label[] labels { get; }
        Button[] buttons { get; }
    }
}