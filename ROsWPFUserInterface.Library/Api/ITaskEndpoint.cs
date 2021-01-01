using ROsWPFUserInterface.Library.Models;
using System.Threading.Tasks;

namespace ROsWPFUserInterface.Library.Api
{
    public interface ITaskEndpoint
    {
        Task<TaskModel> Get();
    }
}