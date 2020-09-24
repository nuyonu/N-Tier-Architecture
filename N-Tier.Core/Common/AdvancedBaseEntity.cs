using System;

namespace N_Tier.Core.Common
{
    public abstract class AdvancedBaseEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
