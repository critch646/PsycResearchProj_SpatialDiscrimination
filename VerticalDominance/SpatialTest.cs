using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticalDominance.enums;

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


        /// <summary>
        /// Method generates blocks and trials for test.
        /// </summary>
        /// <param name="orientationStart">The first block will use this orientation, then will alternate.</param>
        /// <returns>true if successful; otherwise, returns false.</returns>
        public bool GenerateTest(Orientation orientationStart = Orientation.horizontal)
        {
            for (int i = 1; i <= this._numberOfBlocks; i++)
            {
                TrialBlock trialBlock = new(i, orientationStart, this._trialsPerBlock);
                this._trialBlocks.Add(trialBlock);
                orientationStart = NextOrientation(orientationStart);
            }
            return true;
        }


        /// <summary>
        /// Takes in Orientation enum and returns the next orientation.
        /// </summary>
        /// <param name="o">Orientation to increment</param>
        /// <returns>The next orientation.</returns>
        private static Orientation NextOrientation(Orientation o)
        {
            if(o == Orientation.horizontal)
            {
                return Orientation.vertical;
            } else
            {
                return Orientation.horizontal;
            }
        }

    }
}
