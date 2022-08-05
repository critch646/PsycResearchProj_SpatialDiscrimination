using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VerticalDominance
{
    /// <summary>
    /// Interaction logic for WindowSettingsTest.xaml
    /// </summary>
    public partial class WindowSettingsTest : Window
    {

        private UserPreferences _defaults;
        public WindowSettingsTest(UserPreferences defaults)
        {
            InitializeComponent();

            this._defaults = defaults;

            
        }

        private void WindowSettingsTest_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DefaultsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
