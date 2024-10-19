namespace ManagementSystem.Models
{
    public class HealthCheckModel
    {
        public string? Id { get; set; }
        public DateTime SystemTime { get; set; }
        public int NumberofConnectedClients { get; set; }
    }
}
