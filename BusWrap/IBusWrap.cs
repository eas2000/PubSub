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
        List<string> GetMessages();
  
    }
}
