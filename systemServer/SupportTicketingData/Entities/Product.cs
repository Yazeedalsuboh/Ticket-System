using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportTicketingData.Entities
{
    public class Product: EntityBase
    {   
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
    }
}
