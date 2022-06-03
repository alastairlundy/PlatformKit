namespace PlatformKit.Windows.Models;

public class NetworkCard
{
    public string Name { get; set; }
    
    public string ConnectionName { get; set; }
    
    public bool DhcpEnabled { get; set; }
    
    public string DhcpServer { get; set; }

    public string[] IpAddresses { get; set; }
}