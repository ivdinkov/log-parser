using LogParser.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogParser.Service
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> GetAllLogsAsync();
        Task SaveAllLogsAsync(byte[] formFile);
        Task ClearLogsAsync();
        Task<string> GetDataAsync(string id);
    }
}
