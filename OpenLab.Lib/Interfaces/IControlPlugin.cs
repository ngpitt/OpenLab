namespace OpenLab.Lib
{
    public interface IControlPlugin
    {
        PinMode RequiredMode { get; }
        void Create(Control Control);
    }
}
