using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROMETEUS_LAST_EDITION.models.settings
{
    // модель данных для Settings
    public class SettingsData
    {
        public float SomeValue { get; set; }

        public static SettingsData Default => new SettingsData { SomeValue = 37.0f };
    }
}
