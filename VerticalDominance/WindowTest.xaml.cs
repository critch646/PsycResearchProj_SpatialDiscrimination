﻿using System;
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
    /// Interaction logic for WindowTest.xaml
    /// </summary>
    public partial class WindowTest : Window
    {
        private UserPreferences _settings;
        private SpatialTest _test;

        public WindowTest(UserPreferences settings)
        {
            this._settings = settings;
            this._test = new SpatialTest(_settings.CurrentParticipantID, _settings.BlocksPerTest, _settings.TrialsPerBlock);


            InitializeComponent();
        }
    }
}
