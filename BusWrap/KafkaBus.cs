using System;
using System.Collections.Generic;

namespace BusWrap
{
    /// <summary>
    /// Summary description for KafkaBus
    /// </summary>
    public class KafkaBus : IBusWrap
    {
        public KafkaBus()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public List<string> GetMessages()
        {
            List<string> messages = new List<string>();
            messages.Add("kafka msg 1");
            messages.Add("kafka msg 3");
            messages.Add("kafka msg 2");
            return messages;
        }
        public List<string> GetTopics()
        {
            List<string> topics = new List<string>();
            topics.Add("kafka topic 3");
            topics.Add("kafka topic 2");
            topics.Add("kafka topic 1");
            return topics;
        }
    }
}
