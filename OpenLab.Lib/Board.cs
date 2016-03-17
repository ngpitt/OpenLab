using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenLab.Lib
{
    public enum PinMode
    {
        DIGITAL_INPUT,
        DIGITAL_INPUT_PULLUP,
        DIGITAL_OUTPUT,
        ANALOG_INPUT,
        ANALOG_OUTPUT,
    }

    public class Pin
    {
        public string Name;
        public ICollection<PinMode> Modes = new List<PinMode>();
        public bool Assigned = false;
    }

    public class Board
    {
        public string Name { get; private set; }
        public ICollection<Pin> Pins { get; private set; } = new List<Pin>();

        public static Board FromConfig(XElement Config, XNamespace Ns)
        {
            var board = new Board();
            var configVersion = Convert.ToInt32(
                Config.Element(Ns + "version").Value.Split('.')[0]);

            if (configVersion != typeof(Board).Assembly.GetName().Version.Major)
            {
                throw new Exception(string.Format("This config requires OpenLab v{0}.",
                    configVersion));
            }

            board.Name = Config.Element(Ns + "name").Value;

            foreach (var pinConfig in Config.Descendants(Ns + "pin"))
            {
                var pin = new Pin
                {
                    Name = pinConfig.Element(Ns + "name").Value
                };

                foreach (var mode in pinConfig.Element(Ns + "modes").Value.Split(' '))
                {
                    pin.Modes.Add((PinMode)Enum.Parse(typeof(PinMode), mode));
                }

                board.Pins.Add(pin);
            }

            return board;
        }
    }
}
