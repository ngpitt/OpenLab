using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLab.Lib;
using OpenLab.Plugins.Controls;
using OpenLab.Plugins.Logging;
using System.Linq;

namespace OpenLab.Test
{
    [TestClass]
    public class ControlFormTests
    {
        [TestMethod]
        public void LoadPlugins()
        {
            // Arrange
            var form = new ControlForm();

            // Act
            form.LoadPlugins();

            // Assert
            Assert.IsTrue(form.ControlPlugins.OfType<Meter>().Any());
            Assert.IsTrue(form.ControlPlugins.OfType<Spinner>().Any());
            Assert.IsTrue(form.ControlPlugins.OfType<Toggle>().Any());
            Assert.IsTrue(form.LoggingPlugins.OfType<CSV>().Any());
        }

        [TestMethod]
        public void OpenConfig()
        {
            // Arrange
            var controlForm = GetControlForm();
            var controlFormFromConfig = GetControlForm();

            controlForm.Text = "Form";
            controlForm.Width = 100;
            controlForm.Height = 200;
            controlForm.Board = controlForm.Boards.First();
            controlForm.Controls.Add(GroupTests.GetGroup(controlForm));

            // Act
            controlFormFromConfig.FromConfig(
                controlForm.ToConfig("http://www.xphysics.net/OpenLab/Config"),
                "http://www.xphysics.net/OpenLab/Config");

            // Assert
            AssertIsCopy(controlForm, controlFormFromConfig);
        }

        public static ControlForm GetControlForm()
        {
            var form = new ControlForm();

            form.LoadPlugins();
            form.LoadBoards();

            return form;
        }

        public static void AssertIsCopy(ControlForm ControlForm, ControlForm ControlFormCopy)
        {
            var groups = ControlForm.Controls.OfType<Group>();
            var groupsCopy = ControlFormCopy.Controls.OfType<Group>();

            Assert.AreEqual(ControlForm.Text, ControlFormCopy.Text);
            Assert.AreEqual(ControlForm.Width, ControlFormCopy.Width);
            Assert.AreEqual(ControlForm.Height, ControlFormCopy.Height);
            Assert.AreEqual(ControlForm.Board.Name, ControlFormCopy.Board.Name);
            Assert.AreEqual(groups.Count(), groupsCopy.Count());
            
            for (var i = 0; i < groups.Count(); i++)
            {
                GroupTests.AssertIsCopy(groups.ElementAt(i), groupsCopy.ElementAt(i));
            }
        }
    }
}
