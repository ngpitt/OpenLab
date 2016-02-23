using System;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLab.Lib;
using OpenLab.Plugins.Controls;

namespace OpenLab.Test
{
    [TestClass]
    public class ControlTests
    {
        [TestMethod]
        public void FromLocation()
        {
            // Arrange
            var plugin = ControlFormTests.GetControlForm().ControlPlugins.First();
            var location = new Point(1, 2);

            // Act
            var control = Control.FromLocation(plugin, location, false);

            // Assert
            Assert.AreSame(plugin, control.ControlPlugin);
            Assert.AreEqual(location, control.Location);
        }

        [TestMethod]
        public void FromConfig()
        {
            // Arrange
            var control = GetControl(ControlFormTests.GetControlForm());

            // Act
            var controlFromConfig = Control.FromConfig(control.ControlPlugin, control.GetConfig(), control.Log);

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
            var control = Control.FromLocation(ControlForm.ControlPlugins.First(), new Point(1, 2), false);

            control.Text = "Control";

            return control;
        }

        public static void AssertIsCopy(Control Control, Control ControlCopy)
        {
            Assert.AreNotSame(Control, ControlCopy);
            Assert.AreSame(Control.ControlPlugin.GetType(), ControlCopy.ControlPlugin.GetType());
            Assert.AreEqual(Control.Text, ControlCopy.Text);
            Assert.AreEqual(Control.Location, ControlCopy.Location);
            Assert.AreNotSame(Control.Settings, ControlCopy.Settings);
            Assert.IsTrue(Control.Settings.All(x => ControlCopy.Settings.Contains(x)));
        }
    }
}
