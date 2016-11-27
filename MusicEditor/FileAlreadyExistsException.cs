using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicEditor
{
    class FileAlreadyExistsException :Exception
    {
        public FileAlreadyExistsException()
        {
        }

        public FileAlreadyExistsException(string message): base(message)
        {
        }

        public FileAlreadyExistsException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
