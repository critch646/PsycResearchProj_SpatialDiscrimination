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
using System.Windows.Threading;
using VerticalDominance.enums;

namespace VerticalDominance
{
    /// <summary>
    /// Interaction logic for WindowTest.xaml
    /// </summary>
    public partial class WindowTest : Window
    {
        private UserPreferences _settings;
        private SpatialTest _test;
        private readonly int DrawingPadding = 100;

        private bool _isTestRunning = false;
        private int _score = 0;
        private enums.Orientation _currentOrientation;

        private TrialBlock _currentBlock;
        private SpatialTest _currentTrial;

        private FixationShape _fixation;
        private TargetShape _targetShape1;
        private TargetShape _targetShape2;

        private DispatcherTimer _timerFixation;
        private DispatcherTimer _timerInterstimulus;
        private DispatcherTimer _timerTargets;
        private DispatcherTimer _timerMask;
        private DispatcherTimer _timerFeedback;
        private DispatcherTimer _timerIntertrial;

        public WindowTest(UserPreferences settings)
        {
            InitializeComponent();

            this._settings = settings;
            this._test = new SpatialTest(_settings.CurrentParticipantID, _settings.BlocksPerTest, _settings.TrialsPerBlock);

            _currentOrientation = enums.Orientation.vertical;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timerFixation = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.FixationIntervalTime) };
            _timerInterstimulus = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.InterstimulusIntervalTime) };
            _timerTargets = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.TargetIntervalTime) };
            _timerMask = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.MaskIntervalTime) };
            _timerFeedback = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.FeedbackIntervalTime) };
            _timerIntertrial = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.IntertrialIntervalTime) };

            _timerFixation.Tick += _timerFixation_Tick;
            _timerInterstimulus.Tick += _timerInterstimulus_Tick;
            _timerTargets.Tick += _timerTargets_Tick;
            _timerMask.Tick += _timerMask_Tick;
            _timerFeedback.Tick += _timerFeedback_Tick;
            _timerIntertrial.Tick += _timerIntertrial_Tick;

            this._fixation = new FixationShape($"{nameof(FixationShape)}1");
        }

        private void _timerFixation_Tick(object? sender, EventArgs e)
        {
            _timerFixation.Stop();
            _timerInterstimulus.Start();

            RemoveShapes(nameof(FixationShape));
            
        }



        private void _timerInterstimulus_Tick(object? sender, EventArgs e)
        {
            _timerInterstimulus.Stop();
            _timerTargets.Start();

            this._targetShape1 = new TargetShape($"{nameof(TargetShape)}1", StimSize.XS);
            this._targetShape2 = new TargetShape($"{nameof(TargetShape)}2", StimSize.XL);

            CanvasTest.Children.Add(_targetShape1.Shape);
            CanvasTest.Children.Add(_targetShape2.Shape);

            // Display targets horizontally
            if (this._currentOrientation == enums.Orientation.horizontal)
            {
                // Set first target to the left side of the canvas.
                Canvas.SetLeft(_targetShape1.Shape, DrawingPadding - (_targetShape1.Shape.Width / 2));
                //Canvas.SetLeft(_targetShape1.Shape, DrawingPadding);
                Canvas.SetTop(_targetShape1.Shape, (CanvasTest.Height / 2) - (_targetShape1.Shape.Height /2));

                // Set first target to the right side of the canvas.
                Canvas.SetRight(_targetShape2.Shape, DrawingPadding - (_targetShape2.Shape.Width / 2));
                //Canvas.SetRight(_targetShape2.Shape, DrawingPadding);
                Canvas.SetTop(_targetShape2.Shape, (CanvasTest.Height / 2) - (_targetShape2.Shape.Height / 2));

            } 
            
            // Display targets vertically
            else
            {
                // Set first target to the left side of the canvas.
                Canvas.SetLeft(_targetShape1.Shape, (CanvasTest.Width / 2) - (_targetShape1.Shape.Width / 2));
                //Canvas.SetLeft(_targetShape1.Shape, DrawingPadding);
                Canvas.SetTop(_targetShape1.Shape, DrawingPadding - (_targetShape1.Shape.Width / 2));

                // Set first target to the right side of the canvas.
                Canvas.SetLeft(_targetShape2.Shape, (CanvasTest.Width / 2) - (_targetShape2.Shape.Width / 2));
                //Canvas.SetRight(_targetShape2.Shape, DrawingPadding);
                Canvas.SetBottom(_targetShape2.Shape, DrawingPadding - (_targetShape2.Shape.Width / 2));
            }

        }

        private void _timerTargets_Tick(object? sender, EventArgs e)
        {
            _timerTargets.Stop();

            // Remove targets
            RemoveShapes(nameof(TargetShape));

            // Add mask

            _timerMask.Start();
        }

        private void _timerMask_Tick(object? sender, EventArgs e)
        {
            _timerMask.Stop();
            _timerFeedback.Start();
        }

        private void _timerFeedback_Tick(object? sender, EventArgs e)
        {
            _timerFeedback.Stop();
            _timerIntertrial.Start();
        }

        private void _timerIntertrial_Tick(object? sender, EventArgs e)
        {
            _timerIntertrial.Stop();

        }


        private void RemoveShapes(string victimName)
        {
            List<UIElement> itemstoremove = new List<UIElement>();

            foreach (UIElement ui in CanvasTest.Children)
            {
                if (ui.Uid.StartsWith(victimName))
                {
                    itemstoremove.Add(ui);
                }
            }

            foreach (UIElement ui in itemstoremove)
            {
                CanvasTest.Children.Remove(ui);
            }

        }




        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !_isTestRunning)
            {
                System.Diagnostics.Debug.WriteLine("Spaebar pressed");
                RunTest();
            }
        }

        private void RunTest()
        {
            _isTestRunning = true;



            // Instructions
            this.Instructions.Visibility = Visibility.Collapsed;

            // FixationShape
            _timerFixation.Start();

            CanvasTest.Children.Add(_fixation.Shape);
            Canvas.SetLeft(_fixation.Shape, (CanvasTest.Width / 2) - (_fixation.Width / 2));
            Canvas.SetTop(_fixation.Shape, (CanvasTest.Height / 2) - (_fixation.Height / 2));

            // Interstimulus

            // Targets

            // Mask

            // Feedback

            // Intertrial

        }


    }
}
