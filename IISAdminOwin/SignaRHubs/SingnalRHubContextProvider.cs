using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IISAdmin.Interfaces;
using Microsoft.AspNet.SignalR;

namespace IISAdmin.Owin.SignaRHubs
{
    public class SingnalRHubContextProvider:IHubContextProvider
    {
        public IHubContext GetConnectionContext(string hubName) {
            return GlobalHost.ConnectionManager.GetHubContext(hubName);
        }
    }
}