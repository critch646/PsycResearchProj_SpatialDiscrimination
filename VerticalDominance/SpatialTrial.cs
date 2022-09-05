using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpatialDiscrimination.enums;

namespace SpatialDiscrimination
{
    public class SpatialTrial
    {
        public int TrialID { get; set; }
        public Orientation Orientation { get; private set; }
        public (StimSize, StimSize) TrialTargets { get; private set; }
        public int _accuracy = -1;
        public long _responseTime = -1;
        public Key _responseKey = Key.None;


        /// <summary>
        /// SpatialTrial Constructor
        /// </summary>
        /// <param name="trialID">The trial's number.</param>
        /// <param name="orientation">The trial's target orientation.</param>
        /// <param name="trialTargets">The trial's target pair.</param>
        public SpatialTrial(int trialID, Orientation orientation, (StimSize, StimSize) trialTargets)
        {
            this.TrialID = trialID;
            this.Orientation = orientation;
            this.TrialTargets = trialTargets;
        }


        /// <summary>
        /// Setter and getter for the Accuracy field.
        /// </summary>
        public int Accuracy { 
            get { return _accuracy; }
            private set 
            { 
                if (value >= 0)
                {
                    _accuracy = value;
                }
            }
        }


        /// <summary>
        /// The time in which the user entered their response (in milliseconds). A value of -1 indicates the user
        /// failed to respond in time.
        /// </summary>
        public long ResponseTime
        {
            get { return _responseTime; }
            private set 
            { 
                if (value >= -1)
                {
                    _responseTime = value;
                }
            }
        }


        /// <summary>
        /// Key that user responded with.
        /// </summary>
        public Key ResponseKey
        {
            get { return _responseKey; }
            private set
            {
                _responseKey = value;
            }
        }


        /// <summary>
        /// Evaluates user reponse is correct and determines accuracy. 
        /// </summary>
        /// <param name="responseKey">User's key response to evaluate.</param>
        /// <returns>True if correct; otherwise, returns false if incorect.</returns>
        public bool EvaluateResponse(Key responseKey, long responseTime)
        {

            _responseKey = responseKey;

            if (responseKey == Key.None)
            {
                this.ResponseTime = -1;
                this.Accuracy = 0;

                return false;
            }

            this.ResponseTime = responseTime;

            if (TrialTargets.Item1 > TrialTargets.Item2)
            {
                if (responseKey == Key.Left || responseKey == Key.Up)
                {
                    _accuracy = 1;
                    return true;
                } else
                {
                    _accuracy = 0;
                    return false;
                }
            } 
            else if (TrialTargets.Item1 < TrialTargets.Item2)
            {
                if (responseKey == Key.Right || responseKey == Key.Down)
                {
                    _accuracy = 1;
                    return true;
                }
                else
                {
                    _accuracy = 0;
                    return false;
                }
            } 
            else if (TrialTargets.Item1 == TrialTargets.Item2)
            {
                _accuracy = 1;
                return true;
            }


            return false;
        }
    }
}
