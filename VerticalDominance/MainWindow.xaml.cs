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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VerticalDominance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {

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
            WindowSettingsTest windowSettingsTest = new WindowSettingsTest();
            windowSettingsTest.ShowDialog();
        }
    }
}
