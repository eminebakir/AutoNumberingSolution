using DynamicAutoNumberingPlugin.Entities;
using System;

namespace DynamicAutoNumberingPlugin.Services
{
    public interface IAutoNumberService
    {
        /// <summary>
        /// Retrieves the auto-number configuration for a given entity name.
        /// </summary>
        /// <param name="entityName">The name of the entity for which the auto-number configuration is requested.</param>
        /// <returns>An instance of <see cref="AutoNumberConfig"/> containing the configuration details, or null if no configuration is found.</returns>
        AutoNumberConfig GetAutoNumberConfig(string entityName);

        /// <summary>
        /// Allocates a new auto-number based on the given configuration.
        /// </summary>
        /// <param name="config">The auto-number configuration to use for generating a new number.</param>
        /// <returns>The newly allocated number.</returns>
        int AllocateNewNumber(AutoNumberConfig config);

        /// <summary>
        /// Updates the auto-number configuration in the database.
        /// </summary>
        /// <param name="config">The auto-number configuration to update.</param>
        void UpdateAutoNumberConfig(AutoNumberConfig config);
    }
}
