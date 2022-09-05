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

namespace SpatialDiscrimination
{
    /// <summary>
    /// Interaction logic for WindowSettingsTest.xaml
    /// </summary>
    public partial class WindowSettingsTest : Window
    {

        private readonly UserPreferences _defaults;
        private readonly UserPreferences _settings;

        public event EventHandler<TestSettingsChangedEventArgs>? OkButtonClicked;
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
            if (OkButtonClicked != null)
            {
                EventHandler<TestSettingsChangedEventArgs> handler = OkButtonClicked;
                TestSettingsChangedEventArgs args = new()
                {
                    UserPreferences = this._settings
                };
                handler(this, args);
            }
            Close();
        }


        /// <summary>
        /// Resets settings to established default values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void OnSettingsChanged()
        {
            if (this.IsLoaded)
            {
                // Blocks per Test
                if (this.BlocksPerTest_IntUpDown.Value != null)
                {
                    this._settings.BlocksPerTest = (int)this.BlocksPerTest_IntUpDown.Value;
                }
                else
                {
                    this._settings.BlocksPerTest = this._defaults.BlocksPerTest;
                }

                // Trials per Block
                if (this.TrialsPerBlock_IntUpDown.Value != null)
                {
                    this._settings.TrialsPerBlock = (int)this.TrialsPerBlock_IntUpDown.Value;
                }
                else
                {
                    this._settings.TrialsPerBlock = this._defaults.TrialsPerBlock;
                }

                // FixationShape Time Interval
                if (this.Fixation_IntUpDown.Value != null)
                {
                    this._settings.FixationIntervalTime = (int)this.Fixation_IntUpDown.Value;
                }
                else
                {
                    this._settings.FixationIntervalTime = this._defaults.FixationIntervalTime;
                }

                // Interstimulus Time Interval
                if (this.Interstimulus_IntUpDown.Value != null)
                {
                    this._settings.InterstimulusIntervalTime = (int)this.Interstimulus_IntUpDown.Value;
                }
                else
                {
                    this._settings.InterstimulusIntervalTime = this._defaults.InterstimulusIntervalTime;
                }

                // Targets Time Interval
                if (this.Targets_IntUpDown.Value != null)
                {
                    this._settings.TargetIntervalTime = (int)this.Targets_IntUpDown.Value;
                }
                else
                {
                    this._settings.TargetIntervalTime = this._defaults.TargetIntervalTime;
                }

                // Mask Time Interval
                if (this.Mask_IntUpDown.Value != null)
                {
                    this._settings.MaskIntervalTime = (int)this.Mask_IntUpDown.Value;
                }
                else
                {
                    this._settings.MaskIntervalTime = this._defaults.MaskIntervalTime;
                }

                // Feedback Time Interval
                if (this.Feedback_IntUpDown.Value != null)
                {
                    this._settings.FeedbackIntervalTime = (int)this.Feedback_IntUpDown.Value;
                }
                else
                {
                    this._settings.FeedbackIntervalTime = this._defaults.FeedbackIntervalTime;
                }

                // Intertrial Time Interval
                if (this.Intertrial_IntUpDown.Value != null)
                {
                    this._settings.IntertrialIntervalTime = (int)this.Intertrial_IntUpDown.Value;
                }
                else
                {
                    this._settings.IntertrialIntervalTime = this._defaults.IntertrialIntervalTime;
                }
            }
            
        }

        private void BlocksPerTest_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void TrialsPerBlock_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Fixation_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Interstimulus_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Targets_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Mask_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Feedback_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }

        private void Intertrial_IntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSettingsChanged();
        }
    }
}
