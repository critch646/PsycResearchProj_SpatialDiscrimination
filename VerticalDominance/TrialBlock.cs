using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticalDominance.enums;

namespace VerticalDominance
{
    public class TrialBlock
    {
        public int BlockID { get; private set; }
        public int TrialsPerBlock { get; private set; }
        public Range TrialRange { get; private set; }
        public List<SpatialTrial> Trials { get; private set; }
        public Orientation Orientation { get; private set; }

        public TrialBlock(int blockID, Orientation orientation, int trialsPerBlock)
        {
            this.BlockID = blockID;
            TrialsPerBlock = trialsPerBlock;
            this.TrialRange = CalcRange(blockID);
            this.Orientation = orientation;
            this.Trials = new List<SpatialTrial>();
            this.GenerateTrials();
        }


        /// <summary>
        /// Generates trials based on the block's
        /// </summary>
        private void GenerateTrials()
        {
            foreach (int i in Enum.GetValues(typeof(StimSize)))
            {
                foreach (int j in Enum.GetValues(typeof(StimSize)))
                {
                    StimSize first = (StimSize)i;
                    StimSize second = (StimSize)j;

                    SpatialTrial trial = new SpatialTrial(-1, this.Orientation, (first, second));
                    this.Trials.Add(trial);
                }
            }

            this.ShuffleTrials();
            this.NumberTrials();
        }


        /// <summary>
        /// Method shuffles instance's list of SpatialTrials.
        /// </summary>
        private void ShuffleTrials()
        {
            Random random = new Random();
            int n = this.Trials.Count;

            for (int i = this.Trials.Count - 1; i >= 0; i--)
            {
                int rnd = random.Next(i + 1);

                SpatialTrial value = this.Trials[rnd];
                this.Trials[rnd] = this.Trials[i];
                this.Trials[i] = value;
            }
        }


        /// <summary>
        /// Method numbers the instance's list of trials sequentially.
        /// </summary>
        private void NumberTrials()
        {
            int trialNumber = this.TrialRange.Min;

            for (int i = 0; i < this.Trials.Count; i++)
            {
                if (this.Trials[i].TrialID == -1)
                {
                    this.Trials[i].TrialID = trialNumber + i;
                }
            }
        }


        /// <summary>
        /// Method prints the instance's list of trials with their number, orientation, and targets. 
        /// For debugging purposes.
        /// </summary>
        private void PrintTrials()
        {
            foreach (SpatialTrial trial in this.Trials)
            {
                System.Diagnostics.Debug.WriteLine($"TrialID: {trial.TrialID}; Orientation: {trial.Orientation}; TrialTargets: {trial.TrialTargets}");
            }
        }


        /// <summary>
        /// With given block number, returns a Range object with the min and max calculated.
        /// </summary>
        /// <param name="blockNumber">The block number used for calculating trial numbers.</param>
        /// <returns></returns>
        private Range CalcRange(int blockNumber)
        {
            int max = blockNumber * this.TrialsPerBlock;
            int min = max - (this.TrialsPerBlock - 1);

            Range range = new Range(min, max);

            return range;
        }
    }
}
