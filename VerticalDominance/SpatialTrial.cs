using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalDominance
{
    public class SpatialTrial
    {
        public int TrialNumber { get; set; }
        public enums.Orientation Orientation { get; private set; }
        public (enums.StimSize, enums.StimSize) TrialTargets { get; private set; }
        public int _accuracy = -1;
        public int _responseTime = -1;

        public SpatialTrial(int trialNumber, enums.Orientation orientation, (enums.StimSize, enums.StimSize) trialTargets)
        {
            this.TrialNumber = trialNumber;
            this.Orientation = orientation;
            this.TrialTargets = trialTargets;
        }

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
    }
}
