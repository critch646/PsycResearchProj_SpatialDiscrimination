using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialDiscriminationApp
{
    public class TestSettingsChangedEventArgs : EventArgs
    {
        public UserPreferences UserPreferences { get; set; }
    }
}
