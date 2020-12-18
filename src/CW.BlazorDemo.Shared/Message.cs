using System;

namespace CW.BlazorDemo.Shared
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string Payload { get; set; }
        public string Method { get; set; }
        public string SenderId { get; set; }
        public string Receiver { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public int PayloadLength { get; set; }
    }
}