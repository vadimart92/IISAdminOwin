using Microsoft.AspNet.SignalR;

namespace IISAdmin.Interfaces
{
    public interface IHubContextProvider {
        IHubContext GetConnectionContext(string hubName);
    }
}
