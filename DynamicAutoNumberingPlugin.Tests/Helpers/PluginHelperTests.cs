using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using DynamicAutoNumberingPlugin.Helpers;
using DynamicAutoNumberingPlugin.Services;
using DynamicAutoNumberingPlugin.Entities;
using System;

namespace DynamicAutoNumberingPlugin.Tests.Helpers
{
    [TestClass]
    public class PluginHelperTests
    {
        [TestMethod]
        public void GetTargetEntity_ShouldReturnEntity_WhenTargetIsPresent()
        {
            // Arrange
            var targetEntity = new Entity("account");
            var context = new Mock<IPluginExecutionContext>();
            context.Setup(c => c.InputParameters).Returns(new ParameterCollection
            {
                { "Target", targetEntity }
            });

            // Act
            var result = PluginHelper.GetTargetEntity(context.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(targetEntity, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPluginExecutionException))]
        public void GetTargetEntity_ShouldThrowException_WhenTargetIsMissing()
        {
            // Arrange
            var context = new Mock<IPluginExecutionContext>();
            context.Setup(c => c.InputParameters).Returns(new ParameterCollection());

            // Act
            PluginHelper.GetTargetEntity(context.Object);
        }

        [TestMethod]
        public void AssignAutoNumber_ShouldAssignFormattedNumberToField()
        {
            // Arrange
            var targetEntity = new Entity("account");

            // Mock the interface instead of the concrete class
            var autoNumberServiceMock = new Mock<IAutoNumberService>();

            // Configure the mock to return 101 when AllocateNewNumber is called
            autoNumberServiceMock.Setup(s => s.AllocateNewNumber(It.IsAny<AutoNumberConfig>())).Returns(101);

            // Create a mock AutoNumberConfig
            var entityConfig = new Entity("ap_autonumberconfig")
            {
                ["ap_fieldname"] = "new_number",
                ["ap_currentnumber"] = 100
            };
            var config = new AutoNumberConfig(entityConfig);

            // Act
            PluginHelper.AssignAutoNumber(targetEntity, autoNumberServiceMock.Object, config.FieldName, config);

            // Assert
            Assert.AreEqual("101", targetEntity[config.FieldName]);
        }


    }
}
