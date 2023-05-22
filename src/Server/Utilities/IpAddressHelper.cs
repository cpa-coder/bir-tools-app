using System.Net;
using System.Net.Sockets;

namespace BirToolsApp.Server.Utilities;

public static class IpAddressHelper
{
    public static string GetIpAddress()
    {
        var address = "127.0.0.1";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
            
            address = ip.ToString();
            break;
        }

        return address;
    }
}