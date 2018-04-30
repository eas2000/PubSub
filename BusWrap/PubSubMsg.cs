using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusWrap
{
    public class PubSubMsg
    {
        private string _topic;
        private string _msg;
        public string Topic
        {
            get
            {
                return _topic;
            }
            set
            {
                _topic = value;
            }
        }
        public string Msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
            }
        }
        public PubSubMsg(string topic, string msg)
        {
            _topic = topic;
            _msg = msg;
        }
    }
}
