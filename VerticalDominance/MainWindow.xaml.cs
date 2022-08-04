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

        public MainWindow()
        {
            InitializeComponent();

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

        private UserPreferences LoadPreferences(String preferencesFile)
        {
            if (!File.Exists(preferencesFile))
            {
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
                MessageBox.Show($"ERROR: {ex.Message}");
            }

            if (string.IsNullOrEmpty(jsonData))
            {
                return this._defaultPreferences;
            }

            UserPreferences? userPref = new UserPreferences();

            if (!string.IsNullOrEmpty(jsonData))
            {
                userPref = JsonConvert.DeserializeObject<UserPreferences>(jsonData);

            }

            if (userPref != null)
            {
                return userPref;
            }


            return this._defaultPreferences;
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            this._preferences = LoadPreferences(this._userPreferencesFilename);
        }

        private void IntegerUpDown_ParticipantID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuTestSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettingsTest windowSettingsTest = new WindowSettingsTest(this._defaultPreferences);
            windowSettingsTest.ShowDialog();
        }
    }
}
