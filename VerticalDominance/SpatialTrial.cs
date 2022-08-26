using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VerticalDominance
{
    public class SpatialTrial
    {
        public int TrialNumber { get; set; }
        public enums.Orientation Orientation { get; private set; }
        public (enums.StimSize, enums.StimSize) TrialTargets { get; private set; }
        public int _accuracy = -1;
        public int _responseTime = -1;


        /// <summary>
        /// SpatialTrial Constructor
        /// </summary>
        /// <param name="trialNumber">The trial's number.</param>
        /// <param name="orientation">The trial's target orientation.</param>
        /// <param name="trialTargets">The trial's target pair.</param>
        public SpatialTrial(int trialNumber, enums.Orientation orientation, (enums.StimSize, enums.StimSize) trialTargets)
        {
            this.TrialNumber = trialNumber;
            this.Orientation = orientation;
            this.TrialTargets = trialTargets;
        }


        /// <summary>
        /// Setter and getter for the Accuracy field.
        /// </summary>
        public int Accuracy { 
            get { return _accuracy; }
            set 
            { 
                if (value >= 0)
                {
                    _accuracy = value;
                }
            }
        }


        /// <summary>
        /// Setter and getter for the ResponseTime field.
        /// </summary>
        public int ResponseTime
        {
            get { return _responseTime; }
            set 
            { 
                if (value >= 0)
                {
                    _responseTime = value;
                }
            }
        }


        /// <summary>
        /// Evaluates user reponse is correct and determines accuracy. 
        /// </summary>
        /// <param name="response">User's key response to evaluate.</param>
        /// <returns>True if correct; otherwise, returns false if incorect.</returns>
        public bool EvaluateResponse(Key response)
        {
            if (TrialTargets.Item1 > TrialTargets.Item2)
            {
                if (response == Key.Left || response == Key.Up)
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
                if (response == Key.Right || response == Key.Down)
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
