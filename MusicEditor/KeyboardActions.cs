using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicEditor
{
    public class KeyboardActions
    {
        public delegate void KeyboardSwitcher(); //Delegate
        public KeyboardActions() { }

        public static void Perform(KeyboardSwitcher action)
        {
            action();
        }
    }
}
