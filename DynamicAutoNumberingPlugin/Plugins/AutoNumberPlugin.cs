using DynamicAutoNumberingPlugin.Helpers;
using DynamicAutoNumberingPlugin.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Transactions;

namespace DynamicAutoNumberingPlugin.Plugins
{
    public class AutoNumberPlugin : IPlugin
    {
        private IAutoNumberService _autoNumberService;

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("AutoNumberPlugin started.");

            try
            {
                var targetEntity = PluginHelper.GetTargetEntity(context);
                var entityName = targetEntity.LogicalName;

                _autoNumberService = new AutoNumberService(service, tracingService);

                var config = _autoNumberService.GetAutoNumberConfig(entityName);

                if (config == null)
                {
                    return;
                }
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    PluginHelper.AssignAutoNumber(targetEntity, _autoNumberService, config.FieldName, config);
                    tracingService.Trace("Auto-number assigned successfully.");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("Error: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("An error occurred in AutoNumberPlugin.", ex);
            }
        }
    }
}
