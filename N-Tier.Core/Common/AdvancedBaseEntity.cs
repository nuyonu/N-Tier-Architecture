using System;

namespace N_Tier.Core.Common
{
    public class AdvancedBaseEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CretatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
