using System;
using System.Collections.Generic;
using System.Linq;

using Audit.DAL.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

namespace Audit.DAL.Infrastructure
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            AuditLog audit = new AuditLog()
            {
                TableName = TableName,
                Date = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
            };

            return audit;
        }
    }

    public class AuditService
    {
        public static List<AuditEntry> ProcessChanges(IEnumerable<EntityEntry> entries)
        {
            List<AuditEntry> auditEntries = new List<AuditEntry>();

            foreach (EntityEntry entry in entries)
            {

                //AuditLog audit = new AuditLog()
                //{
                //    TableName = entry.Entity.GetType().Name,
                //    PrimaryKey = entry.Metadata.FindPrimaryKey()?.ToString()
                //};
                //var key = entry.Metadata.FindPrimaryKey();

                //foreach (PropertyEntry property in entry.Properties)
                //{
                //    if
                //}

                AuditEntry auditEntry = new AuditEntry(entry);

                auditEntry.TableName = entry.Metadata.GetType().Name;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            return auditEntries;
        }

        public static List<AuditEntry> AfterProcessChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
            {
                return new List<AuditEntry>();
            }

            foreach (AuditEntry auditEntry in auditEntries)
            {
                foreach (PropertyEntry prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
            }

            return auditEntries;
        }
    }


}
