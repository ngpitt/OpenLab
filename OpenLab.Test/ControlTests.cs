using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLab.Lib;
using OpenLab.Plugins.Controls;
using System.Drawing;
using System.Linq;

namespace OpenLab.Test
{
    [TestClass]
    public class ControlTests
    {
        [TestMethod]
        public void FromLocation()
        {
            // Arrange
            var controlForm = ControlFormTests.GetControlForm();
            var plugin = controlForm.ControlPlugins.First();
            var board = controlForm.Boards.First();
            var location = new Point(10, 20);

            // Act
            var control = Control.FromLocation(plugin, board, false, location);

            // Assert
            Assert.AreSame(plugin, control.ControlPlugin);
            Assert.AreEqual(location, control.Location);
        }

        [TestMethod]
        public void FromConfig()
        {
            // Arrange
            var controlForm = ControlFormTests.GetControlForm();
            var board = controlForm.Boards.First();
            var control = GetControl(controlForm);

            // Act
            var controlFromConfig = Control.FromConfig(control.ControlPlugin, board, control.Log,
                control.ToConfig("http://www.xphysics.net/OpenLab/Config"), "http://www.xphysics.net/OpenLab/Config");

            // Assert
            AssertIsCopy(control, controlFromConfig);
        }

        [TestMethod]
        public void Copy()
        {
            // Arrange
            var control = GetControl(ControlFormTests.GetControlForm());

            // Act
            var controlCopy = Control.Copy(control);

            // Assert
            AssertIsCopy(control, controlCopy);
        }

        public static Control GetControl(ControlForm ControlForm)
        {
            var control = Control.FromLocation(ControlForm.ControlPlugins.OfType<Toggle>().First(),
                ControlForm.Boards.First(), false, new Point(10, 20));

            control.Pin = "0";
            control.Log = true;
            control.Text.Set("Control");

            return control;
        }

        public static void AssertIsCopy(Control Control, Control ControlCopy)
        {
            Assert.AreNotSame(Control, ControlCopy);
            Assert.AreSame(Control.ControlPlugin.GetType(), ControlCopy.ControlPlugin.GetType());
            Assert.AreEqual(Control.Log, ControlCopy.Log);
            Assert.AreEqual(Control.Pin, ControlCopy.Pin);
            Assert.AreEqual(Control.Text.Get(), ControlCopy.Text.Get());
            Assert.AreEqual(Control.Location, ControlCopy.Location);
            Assert.AreNotSame(Control.Settings, ControlCopy.Settings);
            Assert.IsTrue(Control.Settings.All(x => ControlCopy.Settings.Contains(x)));
        }
    }
}
