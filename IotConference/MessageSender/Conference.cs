using System;
using System.Collections.Generic;

namespace IotConference.MessageSender
{
    public class Conference
    {
        public Guid Id { get; set; }

        public Guid[] CallIds { get; set; }
    }
}