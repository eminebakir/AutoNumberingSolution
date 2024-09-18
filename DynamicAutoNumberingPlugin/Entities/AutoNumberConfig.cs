
using Microsoft.Xrm.Sdk;
using System;

namespace DynamicAutoNumberingPlugin.Entities
{
    public class AutoNumberConfig
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public int CurrentNumber { get; set; }
        public int PreallocationCount { get; set; }

        public AutoNumberConfig(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Id = entity.Id;
            EntityName = entity.GetAttributeValue<string>("ap_entityname");
            FieldName = entity.GetAttributeValue<string>("ap_fieldname");
            CurrentNumber = entity.GetAttributeValue<int>("ap_currentnumber");
            PreallocationCount = entity.GetAttributeValue<int>("ap_preallocationcount");
        }

        public void UpdateCurrentNumber(int newNumber)
        {
            CurrentNumber = newNumber;
        }
    }
}

