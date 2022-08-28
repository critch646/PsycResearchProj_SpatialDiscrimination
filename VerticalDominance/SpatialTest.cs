using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VerticalDominance.enums;

namespace VerticalDominance
{
    public class SpatialTest
    {
        public DateTime DateTime { get; private set; }
        public int ParticipantID { get; private set; }
        public int NumberOfBlocks { get; private set; }
        public int TrialsPerBlock { get; private set; }
        public int CurrentBlockIndex { get; private set; }
        public int CurrentTrialIndex { get; private set; }
        public List<TrialBlock> TrialBlocks;
        

        /// <summary>
        /// Represents the structure of a spatial discrimination test. The test is divided into blocks, eahc block containing trials.
        /// </summary>
        /// <param name="participantID">The participant's ID number.</param>
        /// <param name="numberOfBlocks">The desired number of blocks for the test.</param>
        /// <param name="trialsPerBlock">The desired number of trials for each block.</param>
        public SpatialTest(int participantID, int numberOfBlocks, int trialsPerBlock)
        {
            this.DateTime = DateTime.Now;
            this.ParticipantID = participantID;
            this.NumberOfBlocks = numberOfBlocks;
            this.TrialsPerBlock = trialsPerBlock;
            this.TrialBlocks = new List<TrialBlock>();
            this.CurrentBlockIndex = 0;
            this.CurrentTrialIndex = 0;

            // Generate test
            // TODO: randomize starting orientation
            this.GenerateTest();
        }


        /// <summary>
        /// Generates blocks and trials for test.
        /// </summary>
        /// <param name="orientationStart">The first block will use this orientation, then will alternate.</param>
        /// <returns>true if successful; otherwise, returns false.</returns>
        public bool GenerateTest(Orientation orientationStart = Orientation.horizontal)
        {
            for (int i = 1; i <= this.NumberOfBlocks; i++)
            {
                TrialBlock trialBlock = new(i, orientationStart, this.TrialsPerBlock);
                this.TrialBlocks.Add(trialBlock);
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


        /// <summary>
        /// Bumps the test, incrementing the trial index and, if necessary, the block index. 
        /// </summary>
        /// <returns>True if there are trials remaining; otherwise, returns false if the test is over.</returns>
        public bool BumpTest()
        {
            // Increment CurrentTrialIndex
            this.CurrentTrialIndex++;

            // Check if CurrentTrialIndex is within range. If not, set to zero and increment CurrentBlockIndex
            if (this.CurrentTrialIndex >= this.TrialBlocks[this.CurrentBlockIndex].Trials.Count)
            {
                this.CurrentTrialIndex = 0;
                this.CurrentBlockIndex++;
            } 

            // Check if CurrentBlockIndex is within range. If it's out of range, the test is complete.
            if (this.CurrentBlockIndex >= this.TrialBlocks.Count)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Evaluates the user's current trial.
        /// </summary>
        /// <param name="responseKey">The key the user responded with.</param>
        /// <param name="responseTime">The total time the user took to respond.</param>
        /// <returns></returns>
        public bool EvaluateResponse(int responseTime, Key responseKey = Key.None)
        {
            return this.TrialBlocks[this.CurrentBlockIndex].Trials[this.CurrentTrialIndex].EvaluateResponse(responseKey, responseTime);
        }

    }
}
