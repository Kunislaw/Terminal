using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terminal
{
    class Frame
    {
        private string Name { get; set; }
        private DateTime LastModified { get; set; }
        private byte[] FrameStructure { get; set; }
    }
}
