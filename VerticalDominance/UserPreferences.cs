using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialDiscrimination
{
    public class UserPreferences
    {
        private int _BlocksPerTest;
        private int _TrialsPerBlock;
        private int _FixationIntervalTime;
        private int _InterstimulusIntervalTime;
        private int _TargetIntervalTime;
        private int _MaskIntervalTime;
        private int _FeedbackIntervalTime;
        private int _IntertrialIntervalTime;


        /// <summary>
        /// Is auto increment participant ID enabled.
        /// </summary>
        public bool AutoIncrement { get; set; }


        /// <summary>
        /// The current participant ID.
        /// </summary>
        public int CurrentParticipantID { get; set; }


        /// <summary>
        /// Directory for spreadsheet where test data will be written.
        /// </summary>
        public string? SpreadsheetDirectory { get; set; }


        /// <summary>
        /// Number of blocks per test (minimum 2, must be even)
        /// </summary>
        public int BlocksPerTest {

            get { return _BlocksPerTest; } 

            set
            {
                if (value < 2)
                {
                    this._BlocksPerTest = 2;
                } else if (value % 2 == 1)
                {
                    this._BlocksPerTest = value + 1;
                } else
                {
                    this._BlocksPerTest = value;
                }
            }
        }


        /// <summary>
        /// Getter and setter for `_TrialsPerBlock` user preference.
        /// </summary>
        public int TrialsPerBlock
        {
            get { return _TrialsPerBlock; }

            set
            {
                if (value < 25)
                {
                    this._TrialsPerBlock = 25;
                }
                else if (value > 50)
                {
                    this._TrialsPerBlock = 50;
                }
                else if (value % 25 == 0)
                {
                    this._TrialsPerBlock = value;
                }
                else
                { 
                    this._TrialsPerBlock = 25;
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }


        /// <summary>
        /// Getter and setter for `_FixationIntervalTime` user preference.
        /// </summary>
        public int FixationIntervalTime {
            get { return _FixationIntervalTime; } 
            set { _FixationIntervalTime = ValidIntervalTime(value); } 
        }


        /// <summary>
        /// Getter and setter for `_InterstimulusIntervalTime` user preference.
        /// </summary>
        public int InterstimulusIntervalTime {
            get { return _InterstimulusIntervalTime; }
            set { _InterstimulusIntervalTime = ValidIntervalTime(value); }
        }


        /// <summary>
        /// Getter and setter for `_TargetIntervalTime` user preference.
        /// </summary>
        public int TargetIntervalTime
        {
            get { return _TargetIntervalTime; }
            set { _TargetIntervalTime = ValidIntervalTime(value); }
        }


        /// <summary>
        /// Getter and setter for `_MaskIntervalTime` user preference.
        /// </summary>
        public int MaskIntervalTime
        {
            get { return _MaskIntervalTime; }
            set { _MaskIntervalTime = ValidIntervalTime(value); }
        }


        /// <summary>
        /// Getter and setter for `_FeedbackIntervalTime` user preference.
        /// </summary>
        public int FeedbackIntervalTime
        {
            get { return _FeedbackIntervalTime; }
            set { _FeedbackIntervalTime = ValidIntervalTime(value); }
        }


        /// <summary>
        /// Getter and setter for `_IntertrialIntervalTime` user preference.
        /// </summary>
        public int IntertrialIntervalTime
        {
            get { return _IntertrialIntervalTime; }
            set { _IntertrialIntervalTime = ValidIntervalTime(value); }
        }

        /// <summary>
        /// Checks if passed `value` is negative or 0 if true returns 1; otherwise, if false
        /// returns the value.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>A positive, non-zero integer.</returns>
        private static int ValidIntervalTime(int value)
        {
            if (value <= 99)
            {
                return 100;
            } 
                return value;
        }
    }
}
