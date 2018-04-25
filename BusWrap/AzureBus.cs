using System;
using System.Collections.Generic;

namespace BusWrap
{
    /// <summary>
    /// Summary description for AzureBus
    /// </summary>
    public class AzureBus : IBusWrap
    {
        public AzureBus()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            messages.Add("AzureBus msg 1");
            messages.Add("AzureBus msg 3");
            messages.Add("AzureBus msg 2");
            return messages;
        }
        public List<string> GetTopics()
        {
            List<string> topics = new List<string>();
            topics.Add("AzureBus topic 3");
            topics.Add("AzureBus topic 2");
            topics.Add("AzureBus topic 1");
            return topics;
        }
    }
}
