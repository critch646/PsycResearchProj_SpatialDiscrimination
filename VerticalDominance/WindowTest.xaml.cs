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
        private readonly UserPreferences _settings;
        private SpatialTest _test;

        private readonly int DrawingPadding = 100;
        private readonly int MaskSize = 150;

        private bool _isTestRunning = false;
        private int _score = 0;

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

        public EventHandler<TestEventArgs>? TestWindowClosing;

        private Stopwatch _stopWatch;

        public WindowTest(UserPreferences settings)
        {
            InitializeComponent();

            this._settings = settings;
            this._test = new SpatialTest(_settings.CurrentParticipantID, _settings.BlocksPerTest, _settings.TrialsPerBlock);

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
            // TODO: Add Luke's instructions

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

            (StimSize, StimSize) targets = this._test.GetTrialTargets();

            this._targetShape1 = new TargetShape($"{nameof(TargetShape)}1", targets.Item1);
            this._targetShape2 = new TargetShape($"{nameof(TargetShape)}2", targets.Item2);


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
            bool responseCorrect = this._test.EvaluateResponse(-1);

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
                bool testFinished = this._test.BumpTest();

                if (!testFinished)
                {
                    _timerFixation.Start();
                    DrawFixation();
                } else
                {
                    _isTestRunning = false;
                    this.FinishTest();
                }

            }
        }


        /// <summary>
        /// Key Down event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Spacebar pressed and test is not running.
            if (e.Key == Key.Space && !_isTestRunning)
            {
                StartTest();
            }

            long responseTime = this._stopWatch.ElapsedMilliseconds;
            bool responseCorrect = false;

            // Check if Mask timer is enabled, as we only want to capture user's response only during the mask phase.
            if (this._timerMask.IsEnabled)
            {

                bool validResponse = false;

                if (this._test.GetOrientation() == enums.Orientation.horizontal)
                {
                    // TODO: Do we want to capture keys for the incorrect orientation and record the response?
                    // Check for Left/Right Arrow keys
                    if (e.Key == Key.Left || e.Key == Key.Right)
                    {
                        validResponse = true;
                        responseCorrect = this._test.EvaluateResponse(responseTime, e.Key);
                    }


                }
                else
                {
                    // Check for Up/Down Arrow keys
                    if (e.Key == Key.Up || e.Key == Key.Down)
                    {
                        validResponse = true;
                        responseCorrect = this._test.EvaluateResponse(responseTime, e.Key);
                    }
                }

                if (validResponse)
                {

                    this._stopWatch.Stop();
                    this._timerMask.Stop();

                    RemoveShapes(nameof(MaskShape));

                    ShowFeedback(responseCorrect);
                    this._timerFeedback.Start();
                }

            }

        }


        /// <summary>
        /// Starts the spatial discrimination test.
        /// </summary>
        private void StartTest()
        {
            _isTestRunning = true;

            // Hide instructions from view.
            this.Instructions.Visibility = Visibility.Collapsed;

            // Fixation start
            _timerFixation.Start();
            DrawFixation();
        }

        /// <summary>
        /// Removes the named shapes from the canvas.
        /// </summary>
        /// <param name="victimName">Looks fore all shapes beginning with this.</param>
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

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape1"></param>
        /// <param name="shape2"></param>
        private void AddShapes(Shape shape1, Shape shape2)
        {
            CanvasTest.Children.Add(shape1);
            CanvasTest.Children.Add(shape2);

            // Display targets horizontally
            if (this._test.GetOrientation() == enums.Orientation.horizontal)
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

        
        /// <summary>
        /// Draws the fixation shape on the canvas.
        /// </summary>
        private void DrawFixation()
        {
            CanvasTest.Children.Add(_fixation.Shape);
            Canvas.SetLeft(_fixation.Shape, (CanvasTest.Width / 2) - (_fixation.Width / 2));
            Canvas.SetTop(_fixation.Shape, (CanvasTest.Height / 2) - (_fixation.Height / 2));
        }


        /// <summary>
        /// Hides the mouse cursor when over the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasTest_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Make it so it actually hides the cursor.
            if (CanvasTest.IsMouseOver)
            {
                Cursor = Cursors.None;
            }
        }

        /// <summary>
        /// Shows user feedback on their response. 
        /// </summary>
        /// <param name="correct">Shows "incorrect" if false and "correct" if true.</param>
        private void ShowFeedback(bool correct)
        {
            FlowDocument feedback = new FlowDocument();
            feedback.Name = "Feedback_FlowDocument";
            feedback.TextAlignment = TextAlignment.Center;

            string answer = "";

            if (correct)
            {
                answer = "Correct!";
                this._score++;
            } else
            {
                answer = "Incorrect!";
            }


            Run feedbackText = new Run(answer);

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


        /// <summary>
        /// Finishes Test, cleaning up and closing the test window.
        /// </summary>
        private void FinishTest()
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventHandler<TestEventArgs> handler = TestWindowClosing;
            if (handler is not null)
            {
                TestEventArgs e1 = new TestEventArgs()
                {
                    SpatialTest = this._test
                };
                handler(this, e1);
            }
        }
    }
}
