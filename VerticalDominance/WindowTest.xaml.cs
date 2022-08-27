using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
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
        private SpatialTest _testResults;

        private readonly int DrawingPadding = 100;
        private readonly int MaskSize = 150;

        private bool _isTestRunning = false;
        private int _score = 0;
        private enums.Orientation _currentOrientation;

        private TrialBlock? _currentBlock = null;
        private SpatialTest? _currentTrial = null;

        private FixationShape? _fixation = null;
        private TargetShape? _targetShape1 = null;
        private TargetShape? _targetShape2 = null;
        private MaskShape? _maskShape1 = null;
        private MaskShape? _maskShape2 = null;

        private readonly DispatcherTimer _timerFixation;
        private readonly DispatcherTimer _timerInterstimulus;
        private readonly DispatcherTimer _timerTargets;
        private readonly DispatcherTimer _timerMask;
        private readonly DispatcherTimer _timerFeedback;
        private readonly DispatcherTimer _timerIntertrial;

        private Stopwatch _stopWatch;

        public WindowTest(UserPreferences settings)
        {
            InitializeComponent();

            this._settings = settings;
            this._test = new SpatialTest(_settings.CurrentParticipantID, _settings.BlocksPerTest, _settings.TrialsPerBlock);

            this._testResults = this._test;

            _currentOrientation = enums.Orientation.vertical;

            _timerFixation = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.FixationIntervalTime) };
            _timerInterstimulus = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.InterstimulusIntervalTime) };
            _timerTargets = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.TargetIntervalTime) };
            _timerMask = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.MaskIntervalTime) };
            _timerFeedback = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.FeedbackIntervalTime) };
            _timerIntertrial = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(_settings.IntertrialIntervalTime) };

            _timerFixation.Tick += TimerFixation_Tick;
            _timerInterstimulus.Tick += TimerInterstimulus_Tick;
            _timerTargets.Tick += TimerTargets_Tick;
            _timerMask.Tick += TimerMask_Tick;
            _timerFeedback.Tick += TimerFeedback_Tick;
            _timerIntertrial.Tick += TimerIntertrial_Tick;

            _stopWatch = new Stopwatch();

            this._fixation = new FixationShape($"{nameof(FixationShape)}1");
            this._maskShape1 = new MaskShape($"{nameof(MaskShape)}1", MaskSize);
            this._maskShape2 = new MaskShape($"{nameof(MaskShape)}2", MaskSize);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void TimerFixation_Tick(object? sender, EventArgs e)
        {
            _timerFixation.Stop();
            _timerInterstimulus.Start();

            RemoveShapes(nameof(FixationShape));
            
        }



        private void TimerInterstimulus_Tick(object? sender, EventArgs e)
        {
            _timerInterstimulus.Stop();


            this._targetShape1 = new TargetShape($"{nameof(TargetShape)}1", StimSize.XS);
            this._targetShape2 = new TargetShape($"{nameof(TargetShape)}2", StimSize.XL);


            AddShapes(this._targetShape1.Shape, this._targetShape2.Shape);
            _timerTargets.Start();
        }

        private void TimerTargets_Tick(object? sender, EventArgs e)
        {
            _timerTargets.Stop();

            // Remove targets
            RemoveShapes(nameof(TargetShape));

            // Create and add masks
            this._maskShape1 = new MaskShape($"{nameof(MaskShape)}1", MaskSize);
            this._maskShape2 = new MaskShape($"{nameof(MaskShape)}2", MaskSize);

            AddShapes(this._maskShape1.Shape, this._maskShape2.Shape);

            _timerMask.Start();
            _stopWatch.Restart();
        }

        private void TimerMask_Tick(object? sender, EventArgs e)
        {
            _timerMask.Stop();
            _stopWatch.Stop();

            // Remove masks
            RemoveShapes(nameof(MaskShape));

            // Show feedback
            ShowFeedback(false);

            _timerFeedback.Start();
        }

        private void TimerFeedback_Tick(object? sender, EventArgs e)
        {
            _timerFeedback.Stop();

            // Remove feedback
            RemoveShapes(nameof(RichTextBox));

            _timerIntertrial.Start();
        }

        private void TimerIntertrial_Tick(object? sender, EventArgs e)
        {
            _timerIntertrial.Stop();

            if (_isTestRunning)
            {
                if (this._currentOrientation == enums.Orientation.horizontal)
                {
                    this._currentOrientation = enums.Orientation.vertical;
                }
                else
                {
                    this._currentOrientation = enums.Orientation.horizontal;
                }
                _timerFixation.Start();
                DrawFixation();


            }

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

        private void AddShapes(Shape shape1, Shape shape2)
        {
            CanvasTest.Children.Add(shape1);
            CanvasTest.Children.Add(shape2);

            // Display targets horizontally
            if (this._currentOrientation == enums.Orientation.horizontal)
            {
                // Set first target to the left side of the canvas.
                Canvas.SetLeft(shape1, DrawingPadding - (shape1.Width / 2));
                //Canvas.SetLeft(_targetShape1.Shape, DrawingPadding);
                Canvas.SetTop(shape1, (CanvasTest.Height / 2) - (shape1.Height / 2));

                // Set first target to the right side of the canvas.
                Canvas.SetRight(shape2, DrawingPadding - (shape2.Width / 2));
                //Canvas.SetRight(_targetShape2.Shape, DrawingPadding);
                Canvas.SetTop(shape2, (CanvasTest.Height / 2) - (shape2.Height / 2));

            }

            // Display targets vertically
            else
            {
                // Set first target to the left side of the canvas.
                Canvas.SetLeft(shape1, (CanvasTest.Width / 2) - (shape1.Width / 2));
                //Canvas.SetLeft(_targetShape1.Shape, DrawingPadding);
                Canvas.SetTop(shape1, DrawingPadding - (shape1.Width / 2));

                // Set first target to the right side of the canvas.
                Canvas.SetLeft(shape2, (CanvasTest.Width / 2) - (shape2.Width / 2));
                //Canvas.SetRight(_targetShape2.Shape, DrawingPadding);
                Canvas.SetBottom(shape2, DrawingPadding - (shape2.Width / 2));
            }
        }

        private void DrawFixation()
        {
            CanvasTest.Children.Add(_fixation.Shape);
            Canvas.SetLeft(_fixation.Shape, (CanvasTest.Width / 2) - (_fixation.Width / 2));
            Canvas.SetTop(_fixation.Shape, (CanvasTest.Height / 2) - (_fixation.Height / 2));
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Spacebar pressed and test is not running.
            if (e.Key == Key.Space && !_isTestRunning)
            {
                RunTest();
            }




            // Check if Mask timer is enabled, as we only want to capture user's response only during the mask phase.
            if (this._timerMask.IsEnabled)
            {

                bool validResponse = false;

                if (this._currentOrientation == enums.Orientation.horizontal)
                {
                    // TODO: Do we want to capture keys for the incorrect orientation and record the response?
                    // Check for Left/Right Arrow keys
                    if (e.Key == Key.Left || e.Key == Key.Right)
                    {
                        validResponse = true;
                        System.Diagnostics.Debug.WriteLine($"Key \"{e.Key}\" pressed");
                    }


                }
                else
                {
                    // Check for Up/Down Arrow keys
                    if (e.Key == Key.Up || e.Key == Key.Down)
                    {
                        validResponse = true;
                        System.Diagnostics.Debug.WriteLine($"Key \"{e.Key}\" pressed");
                    }
                }

                if (validResponse)
                {

                    this._stopWatch.Stop();
                    this._timerMask.Stop();

                    RemoveShapes(nameof(MaskShape));
                    
                    // TODO: Evaluate user's response with current spatial trial
                    
                    
                    ShowFeedback(true);
                    this._timerFeedback.Start();
                }

            }

        }

        private void RunTest()
        {
            _isTestRunning = true;



            // Instructions
            this.Instructions.Visibility = Visibility.Collapsed;

            // Setup test
            //this._currentBlock = this._test.TrialBlocks[0];



            // Fixation start
            _timerFixation.Start();
            DrawFixation();


        }


        private void CanvasTest_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (CanvasTest.IsMouseOver)
            {
                Cursor = Cursors.None;
            }
        }

        private void ShowFeedback(bool correct)
        {
            FlowDocument feedback = new FlowDocument();
            feedback.Name = "Feedback_FlowDocument";
            feedback.TextAlignment = TextAlignment.Center;


            Run feedbackText = new Run("Feedback");

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(feedbackText);


            feedback.Blocks.Add(paragraph);

            RichTextBox feedbackRichText = new RichTextBox(feedback);

            feedbackRichText.Width = 500;
            feedbackRichText.Height = 500;
            feedbackRichText.BorderThickness = new Thickness(0);
            feedbackRichText.FontSize = 48;

            feedbackRichText.Uid = nameof(RichTextBox);

            CanvasTest.Children.Add(feedbackRichText);

            Canvas.SetLeft(feedbackRichText, (CanvasTest.Width / 2) - (feedbackRichText.Width / 2));
            Canvas.SetTop(feedbackRichText, (CanvasTest.Height / 2) - (feedbackRichText.Height / 2));
        }

        
    }
}
