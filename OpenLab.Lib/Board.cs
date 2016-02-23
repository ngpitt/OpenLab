using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class Board
    {
        public string Type { get; private set; }
        public Dictionary<int, List<PinMode>> Pins { get; private set; } = new Dictionary<int, List<PinMode>>();
    }
}
