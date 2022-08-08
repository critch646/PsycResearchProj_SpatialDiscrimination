using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;

using Newtonsoft.Json;

namespace VerticalDominance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private UserPreferences _preferences;
        private UserPreferences _defaultPreferences;
        private string _userPreferencesFilename;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();

            log4net.Config.XmlConfigurator.Configure();

            this._defaultPreferences = new UserPreferences { 
                AutoIncrement = false,
                BlocksPerTest = 4,
                TrialsPerBlock = 25,
                FixationIntervalTime = 500,
                InterstimulusIntervalTime = 200,
                TargetIntervalTime = 200,
                MaskIntervalTime = 2000,
                FeedbackIntervalTime = 500,
                IntertrialIntervalTime = 500,
                SpreadsheetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            this._userPreferencesFilename = $"{nameof(UserPreferences)}.json";


        }

        /// <summary>
        /// MainWindow1 loaded event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            this._preferences = LoadPreferences(this._userPreferencesFilename);
            log.Info($"App loaded successfully.");


            // Events

        }


        /// <summary>
        /// Load user preferences from file.
        /// </summary>
        /// <param name="preferencesFile">Target file to read from.</param>
        /// <returns>UserPreferences object.</returns>
        private UserPreferences LoadPreferences(String preferencesFile)
        {
            if (!File.Exists(preferencesFile))
            {
                log.Error($"Preferences file does not exist.");
                return this._defaultPreferences;
            }

            string jsonData = string.Empty;

            try
            {
                using var stream = File.OpenText(preferencesFile);
                jsonData = stream.ReadToEnd();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                log.Error(ex);
            }

            if (string.IsNullOrEmpty(jsonData))
            {
                return this._defaultPreferences;
            }

            UserPreferences? userPref = new UserPreferences();

            if (!string.IsNullOrEmpty(jsonData))
            {
                userPref = JsonConvert.DeserializeObject<UserPreferences>(jsonData);

                if (userPref != null)
                {
                    return userPref;
                }
            }

            return this._defaultPreferences;
        }

        private void SavePreferences()
        {
            if (this._preferences != null)
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(this._preferences);
                    File.WriteAllText($"{nameof(UserPreferences)}.json", jsonData);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }




        private void IntegerUpDown_ParticipantID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this._preferences != null && IntegerUpDown_ParticipantID.Value != null)
            {
                this._preferences.CurrentParticipantID = (int)IntegerUpDown_ParticipantID.Value;
            }
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuTestSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettingsTest windowSettingsTest = new(this._defaultPreferences, this._preferences);
            windowSettingsTest.OkButtonClicked += WindowSettingsTest_OkButtonClicked;

            windowSettingsTest.ShowDialog();
        }

        private void WindowSettingsTest_OkButtonClicked(object? sender, TestSettingsChangedEventArgs e)
        {
            if (e != null)
            {
                this._preferences.BlocksPerTest = e.UserPreferences.BlocksPerTest;
                this._preferences.TrialsPerBlock = e.UserPreferences.TrialsPerBlock;
                this._preferences.FixationIntervalTime = e.UserPreferences.FixationIntervalTime;
                this._preferences.InterstimulusIntervalTime = e.UserPreferences.InterstimulusIntervalTime;
                this._preferences.TargetIntervalTime = e.UserPreferences.TargetIntervalTime;
                this._preferences.MaskIntervalTime = e.UserPreferences.MaskIntervalTime;
                this._preferences.FeedbackIntervalTime = e.UserPreferences.FeedbackIntervalTime;
                this._preferences.IntertrialIntervalTime = e.UserPreferences.IntertrialIntervalTime;

                SavePreferences();
            }


        }
    }
}
