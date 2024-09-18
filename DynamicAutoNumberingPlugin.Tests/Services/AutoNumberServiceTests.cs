using DynamicAutoNumberingPlugin.Entities;
using DynamicAutoNumberingPlugin.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System;

namespace DynamicAutoNumberingPlugin.Tests.Services
{
    [TestClass]
    public class AutoNumberServiceTests
    {
        private Mock<IOrganizationService> _mockService;
        private Mock<ITracingService> _mockTracingService;
        private AutoNumberService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IOrganizationService>();
            _mockTracingService = new Mock<ITracingService>();
            _service = new AutoNumberService(_mockService.Object, _mockTracingService.Object);
        }

        [TestMethod]
        public void GetAutoNumberConfig_ShouldReturnConfig_WhenFound()
        {
            // Arrange
            var config = new Entity("AutoNumberConfig")
            {
                Id = Guid.NewGuid(),
                ["ap_entityname"] = "account",
                ["ap_fieldname"] = "new_number",
                ["ap_currentnumber"] = 100,
                ["ap_preallocationcount"] = 10
            };

            _mockService.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
                        .Returns(new EntityCollection(new List<Entity> { config }));

            // Act
            var result = _service.GetAutoNumberConfig("account");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("new_number", result.FieldName);
        }

        [TestMethod]
        public void AllocateNewNumber_ShouldIncreaseNumber()
        {
            // Arrange
            var entityConfig = new Entity("ap_autonumberconfig")
            {
                ["ap_preallocationcount"] = 100,
                ["ap_currentnumber"] = 5
            };
            var config = new AutoNumberConfig(entityConfig);

            // Act
            var newNumber = _service.AllocateNewNumber(config);

            // Assert
            Assert.AreEqual(6, newNumber); // Beklenen sonuç 6 olmalı çünkü başlangıçta 5'ti
        }

    }
}
