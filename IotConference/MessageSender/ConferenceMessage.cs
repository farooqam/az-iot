using System;

namespace IotConference.MessageSender
{
    public class ConferenceMessage
    {
        public DateTime MessageCreatedUtcTime { get; set; }
        public int StatusCode { get; set; }
    }
}