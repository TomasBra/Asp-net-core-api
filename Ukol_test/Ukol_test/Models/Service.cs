using System;
using System.Collections.Generic;

#nullable disable

namespace Ukol_test.Models
{
    public partial class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ServiceTypeId { get; set; }
        public bool? SpecificationTextRequired { get; set; }

        public virtual ServiceType ServiceType { get; set; }
    }
}
