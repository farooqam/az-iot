using System;
using System.Collections.Generic;
using System.Linq;

namespace IotConference.MessageSender
{
    public class MessageService
    {
        private readonly int[] _statusCodes = {200, 400, 500};
        
        public ConferenceMessage CreateMessage()
        {
            var random = new Random();
            
            return new ConferenceMessage
            {
                MessageCreatedUtcTime = DateTime.UtcNow,
                StatusCode = _statusCodes[random.Next(0, _statusCodes.Length -1)]
            };
        }
    }
}