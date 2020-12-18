using System.Collections.Generic;

namespace CW.BlazorDemo.Client.Models
{
    public interface ISession
    {
        List<int> AvailablePorts { get; set; }
        int SelectedPort { get; set; }
    }
}