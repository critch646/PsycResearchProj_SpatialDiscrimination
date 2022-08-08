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

        private readonly UserPreferences _defaults;
        private readonly UserPreferences _settings;
        public WindowSettingsTest(UserPreferences defaults, UserPreferences settings)
        {
            this._defaults = defaults;
            this._settings = settings;
            this.DataContext = this._settings;
            InitializeComponent();


            
        }

        private void WindowSettingsTest_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            this.BlocksPerTest_IntUpDown.Value = this._defaults.BlocksPerTest;
            this.TrialsPerBlock_IntUpDown.Value = this._defaults.TrialsPerBlock;
            this.Fixation_IntUpDown.Value = this._defaults.FixationIntervalTime;
            this.Interstimulus_IntUpDown.Value = this._defaults.InterstimulusIntervalTime;
            this.Targets_IntUpDown.Value = this._defaults.TargetIntervalTime;
            this.Mask_IntUpDown.Value = this._defaults.MaskIntervalTime;
            this.Feedback_IntUpDown.Value = this._defaults.FeedbackIntervalTime;
            this.Intertrial_IntUpDown.Value = this._defaults.IntertrialIntervalTime;
        }
    }
}
