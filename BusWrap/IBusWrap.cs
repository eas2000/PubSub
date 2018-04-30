using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusWrap
{
    public interface IBusWrap
    {
        List<string> GetTopics();
        List<PubSubMsg> GetMessages(List<string> topics);
        List<string> GetTopicsSubscribed();
        void Subscribe(string topic);
        void UnSubscribe(string topic);
        void RegisterListener(IMsgListener listener);
        void UnregisterListener(IMsgListener listener);
        void NotifyListeners();
        void Run_Consume();

    }
}
