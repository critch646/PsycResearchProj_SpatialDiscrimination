using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerticalDominance
{
    public class SpatialTest
    {
        public DateTime _dateTime;
        public int _participantNumber;
        public int _numberOfBlocks;
        public int _trialsPerBlock;
        public List<TrialBlock> _trialBlocks;

        public SpatialTest(DateTime dateTime, int participantNumber, int numberOfBlocks, int trialsPerBlock)
        {
            this._dateTime = dateTime;
            this._participantNumber = participantNumber;
            this._numberOfBlocks = numberOfBlocks;
            this._trialsPerBlock = trialsPerBlock;
            this._trialBlocks = new List<TrialBlock>();
        }

        private void GenerateTest()
        {
            for (int i = 1; i <= this._numberOfBlocks; i++)
            {

            }
        }

    }
}
