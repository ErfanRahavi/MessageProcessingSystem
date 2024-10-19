using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDistributor.Services
{
    public class ClientManagerService
    {
        private readonly int _maxClients;
        private int _currentClients;
        private readonly object _lock = new object();

        public ClientManagerService(int maxClients)
        {
            _maxClients = maxClients;
            _currentClients = 0;
        }

        public bool TryAddClient()
        {
            lock (_lock)
            {
                if (_currentClients < _maxClients)
                {
                    _currentClients++;
                    return true;
                }
                return false;
            }
        }
        // حذف یک کلاینت از تعداد کلاینت‌های فعال
        public void RemoveClient()
        {
            lock (_lock)
            {
                if (_currentClients > 0)
                    _currentClients--;
            }
        }
        // بازگرداندن تعداد کلاینت‌های فعال
        public int GetActiveClients()
        {
            lock (_lock)
            {
                return _currentClients;
            }
        }
    }
}
