using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BusWrap
{
    /// <summary>
    /// Summary description for AzureBus
    /// </summary>
    public class AzureBus : IBusWrap
    {
        private static List<string> SubscribedTopics { get; set; }
        public AzureBus()
        {
            //
            // TODO: Add constructor logic here
            //
            SubscribedTopics = new List<string>();
        }
        public List<PubSubMsg> GetMessages(List<string> topics)
        {
            List<PubSubMsg> messages = new List<PubSubMsg>();
            messages.Add(new PubSubMsg("topic1","AzureBus msg 11"));
            messages.Add(new PubSubMsg("topic1", "AzureBus msg 12"));
            messages.Add(new PubSubMsg("topic2", "AzureBus msg 21"));
            messages.Add(new PubSubMsg("topic2", "AzureBus msg 22"));
            messages.Add(new PubSubMsg("topic3", "AzureBus msg 31"));
            messages.Add(new PubSubMsg("topic4", "AzureBus msg 41"));
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
        public List<string> GetTopicsSubscribed()
        {
            return SubscribedTopics;
        }

        public void Subscribe(string topic)
        {
            SubscribedTopics.Add(topic);
        }
        public void UnSubscribe(string topic)
        {
            SubscribedTopics.Remove(topic);
        }
        private delegate void m_eventHandler(PubSubMsg msg);
        private event m_eventHandler m_event;

        public void RegisterListener(IMsgListener listener)
        {
            m_event += new m_eventHandler(listener.OnMsg);
        }

        public void UnregisterListener(IMsgListener listener)
        {
            m_event -= new m_eventHandler(listener.OnMsg);
        }

        public void NotifyListeners()
        {
            if (null != m_event)
                m_event(new PubSubMsg("azuretopic", "azuremsg"));
        }

        public async void Run_Consume()
        {
            while (true)
            {
                await Task.Delay(10000);
                NotifyListeners();
            }
        }
    }
}
