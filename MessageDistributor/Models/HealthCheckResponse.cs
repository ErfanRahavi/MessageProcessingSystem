using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDistributor.Models
{
    public class HealthCheckResponse
    {
        public bool IsEnabled { get; set; } = true;
        public int NumberOfActiveClients { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
