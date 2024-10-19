using System.Linq;
using System.Net.NetworkInformation;

namespace MessageProcessor.Utilities
{
    public static class UniqueIdGenerator
    {
        public static string GenerateIdFromMacAddress()
        {
            var macAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            return macAddress ?? Guid.NewGuid().ToString(); 
        }
    }
}
