using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicEditor
{
    public class StreamEventArgs: EventArgs
    {
        public String filePath;

        public StreamEventArgs(String filePath)
        {
            this.filePath = filePath;
        }
    }
}
