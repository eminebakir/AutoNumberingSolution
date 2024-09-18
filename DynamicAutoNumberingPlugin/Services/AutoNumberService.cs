using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicAutoNumberingPlugin.Entities;
using DynamicAutoNumberingPlugin.Helpers;
using System.IdentityModel.Metadata;

namespace DynamicAutoNumberingPlugin.Services
{
    public class AutoNumberService :IAutoNumberService
    {
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;

        public AutoNumberService(IOrganizationService service, ITracingService tracingService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _tracingService = tracingService ?? throw new ArgumentNullException(nameof(tracingService));
        }

        public AutoNumberConfig GetAutoNumberConfig(string entityName)
        {
            var query = new QueryExpression("ap_autonumberconfig")
            {
                ColumnSet = new ColumnSet("ap_entityname", "ap_fieldname", "ap_currentnumber", "cr923_preallocationcount"),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("ap_entityname", ConditionOperator.Equal, entityName) }
                }
            };

            var configs = _service.RetrieveMultiple(query);

            if (configs.Entities.Any())
            {
                return new AutoNumberConfig(configs.Entities.First());
            }
            return null;
        }

        public int AllocateNewNumber(AutoNumberConfig config)
        {
            int newNumber = config.CurrentNumber + 1;
            config.UpdateCurrentNumber(newNumber);

            // Numara havuzu kontrolü
            if (newNumber >= config.CurrentNumber + config.PreallocationCount)
            {
                _tracingService.Trace("Preallocation limit reached, updating current number in database.");
                UpdateAutoNumberConfig(config);
            }

            return newNumber;
        }

        public void UpdateAutoNumberConfig(AutoNumberConfig config)
        {
            var update = new Entity("ap_autonumberconfig", config.Id)
            {
                ["ap_currentnumber"] = config.CurrentNumber
            };

            _service.Update(update);
        }
    }


}
