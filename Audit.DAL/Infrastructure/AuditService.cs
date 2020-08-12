using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Audit.DAL.Infrastructure
{
    public class AuditService
    {
        public static List<AuditEntry> ProcessChanges(IEnumerable<EntityEntry> entries)
        {
            List<AuditEntry> auditEntries = new List<AuditEntry>();

            foreach (EntityEntry entry in entries)
            {
                AuditEntry auditEntry = new AuditEntry(entry);

                auditEntries.Add(auditEntry);

                foreach (PropertyEntry property in entry.Properties)
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

        public static List<AuditEntry> ProcessChangesAfterSave(List<AuditEntry> auditEntries)
        {
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
