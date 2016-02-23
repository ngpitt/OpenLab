using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenLab.Lib;

namespace OpenLab.Test
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        public void FromLocation()
        {
            // Arrange
            var controlPlugins = ControlFormTests.GetControlForm().ControlPlugins;
            var location = new Point(1, 2);

            // Act
            var group = Group.FromLocation(controlPlugins, location);

            // Assert
            Assert.AreSame(controlPlugins, group.ControlPlugins);
            Assert.AreEqual(location, group.Location);
        }

        [TestMethod]
        public void FromConfig()
        {
            // Arrange
            var group = GetGroup(ControlFormTests.GetControlForm());

            // Act
            var groupCopy = Group.FromConfig(group.ControlPlugins, group.GetConfig(), false);

            // Assert
            AssertIsCopy(group, groupCopy);
        }

        [TestMethod]
        public void Copy()
        {
            // Arrange
            var group = GetGroup(ControlFormTests.GetControlForm());

            // Act
            var groupCopy = Group.Copy(group);

            // Assert
            AssertIsCopy(group, groupCopy);
        }

        public static Group GetGroup(ControlForm ControlForm)
        {
            var group = Group.FromLocation(ControlForm.ControlPlugins, new Point(1, 2));
            var control = ControlTests.GetControl(ControlForm);

            group.Text = "Group";
            group.Controls.Add(control);

            return group;
        }

        public static void AssertIsCopy(Group Group, Group GroupCopy)
        {
            var controls = Group.Controls.OfType<Control>();
            var controlsCopy = GroupCopy.Controls.OfType<Control>();

            Assert.AreNotSame(Group, GroupCopy);
            Assert.IsTrue(Group.ControlPlugins.All(x => GroupCopy.ControlPlugins.Any(y => y.GetType() == x.GetType())));
            Assert.AreEqual(Group.Text, GroupCopy.Text);
            Assert.AreEqual(Group.Location, GroupCopy.Location);
            Assert.AreEqual(controls.Count(), controlsCopy.Count());
            
            for (var i = 0; i < controls.Count(); i++)
            {
                ControlTests.AssertIsCopy(controls.ElementAt(i), controlsCopy.ElementAt(i));
            }
        }
    }
}
