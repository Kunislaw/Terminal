using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class Time
    {
        public long getMillitsFrom1970()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long ms = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;
            return ms;
        }
    }
}
