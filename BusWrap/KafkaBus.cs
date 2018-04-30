using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using RdKafka;

using Confluent.Kafka;

namespace BusWrap
{
    /// <summary>
    /// Summary description for KafkaBus
    /// </summary>
    public class KafkaBus : IBusWrap
    {
        private static List<string> SubscribedTopics { get; set; }
        private Dictionary<string, object> config;
        public KafkaBus()
        {
            //
            // TODO: Add constructor logic here
            //
            SubscribedTopics = new List<string>();

            config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "eas2w8:9092" },
                //{ "group.id", "foo" },
                { "default.topic.config", new Dictionary<string, object>
                    {
                        { "auto.offset.reset", "smallest" }
                    }
                }
            };

            //var section = (ConfigurationManager.GetSection("KafkaConfig") as System.Collections.Hashtable)
            //     .Cast<System.Collections.DictionaryEntry>()
            //     .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());

        }
        public List<PubSubMsg> GetMessages(List<string> topics)
        {
            List<PubSubMsg> messages = new List<PubSubMsg>();
            if (topics.Count < 1)
            {
                messages.Add(new PubSubMsg("kafkatopic1", "kafka msg 11"));
                messages.Add(new PubSubMsg("kafkatopic1", "kafka msg 12"));
                messages.Add(new PubSubMsg("kafkatopic2", "kafka msg 21"));
                messages.Add(new PubSubMsg("kafkatopic2", "kafka msg 22"));
                messages.Add(new PubSubMsg("kafkatopic3", "kafka msg 31"));
                messages.Add(new PubSubMsg("kafkatopic4", "kafka msg 41"));
            }
            else
            {
                foreach (string topic in topics)
                {
                    messages.Add(new PubSubMsg(topic, "kafka msg " + topic + "_1"));
                }
            }
            return messages;
        }
        public List<string> GetTopics()
        {
            List<string> topics = new List<string>();


            //using (var producer = new Producer("eas2w8:9092"))
            //{
            //    // Metadata meta = producer.Metadata().Wait();
            //    var task = Task.Run(async () => await producer.Metadata());
            //    task.Wait();
            //    var meta = task.Result;


            //}
            AdminClient adm = new AdminClient(config);
            Confluent.Kafka.Metadata meta = adm.GetMetadata(true, TimeSpan.FromSeconds(10));

            meta.Topics.ForEach(topic =>
            {
                topics.Add(topic.Topic);
            });
            return topics;
        }
        public List<string> GetTopicsSubscribed()
        {
            return SubscribedTopics;
        }

        public void Subscribe(string topic)
        {
            if (!SubscribedTopics.Contains(topic))
                SubscribedTopics.Add(topic);
        }
        public void UnSubscribe(string topic)
        {
            SubscribedTopics.Remove(topic);
        }
        private delegate void m_eventHandler(PubSubMsg msg);
        private  event m_eventHandler m_event;

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
                m_event(new PubSubMsg("kafkatopic", "kafkamsg-" + DateTime.Now));
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
