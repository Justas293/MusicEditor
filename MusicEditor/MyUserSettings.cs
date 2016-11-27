using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Drawing;

namespace MusicEditor
{
    class MyUserSettings: ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue("Green")]
        public Color BackgroundColor
        {
            get
            {
                return ((Color)this["BackgroundColor"]);
            }
            set
            {
                this["BackgroundColor"] = (Color)value;
            }
        }
    }
    
}
