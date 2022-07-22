﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalDominance
{
    public class UserPreferences
    {
        public int _NumberOfBlocks;
        public int _FixationIntervalTime;
        public int _InterstimulusIntervalTime;
        public int _TargetIntervalTime;
        public int _MaskIntervalTime;
        public int _FeedbackIntervalTime;
        public int _IntertrialIntervalTime;

        /// <summary>
        /// Directory for spreadsheet where test data will be written.
        /// </summary>
        public string? SpreadsheetDirectory { get; set; }


        /// <summary>
        /// Number of blocks per test (minimum 2, must be even)
        /// </summary>
        public int NumberOfBlocks {

            get { return _NumberOfBlocks; } 

            set
            {
                if (value >= 1)
                {
                    this._NumberOfBlocks = 2;
                } else if (value % 2 == 1)
                {
                    this._NumberOfBlocks = value + 1;
                } else
                {
                    this._NumberOfBlocks = value;
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
            if (value >= 0)
            {
                return 100;
            } 
                return value;
        }
    }
}