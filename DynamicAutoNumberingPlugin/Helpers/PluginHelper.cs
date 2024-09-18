using DynamicAutoNumberingPlugin.Entities;
using DynamicAutoNumberingPlugin.Services;
using Microsoft.Xrm.Sdk;
using System;

namespace DynamicAutoNumberingPlugin.Helpers
{
    public static class PluginHelper
    {
        public static Entity GetTargetEntity(IPluginExecutionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity)
            {
                return entity;
            }

            throw new InvalidPluginExecutionException("Target entity not found in context.");
        }

        public static void AssignAutoNumber(Entity targetEntity, IAutoNumberService autoNumberService, string fieldName, AutoNumberConfig config)
        {
            int newNumber = autoNumberService.AllocateNewNumber(config);
            targetEntity[fieldName] = newNumber.ToString();
        }
    }
}
