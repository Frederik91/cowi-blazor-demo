using System.Collections.Generic;

namespace CW.BlazorDemo.Client.Models
{
    public class Session : ISession
    {
        public List<int> AvailablePorts { get; set; } = new List<int>();
        public int SelectedPort { get; set; }
    }
}