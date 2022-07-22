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
        public int _blockNumber;
        public int _trialsPerBlock;
        public Range TrialRange { get; private set; }
        public List<SpatialTrial> _trials;
        public enums.Orientation Orientation { get; private set; }

        public TrialBlock(int blockNumber, Orientation orientation, int trialsPerBlock)
        {
            this._blockNumber = blockNumber;
            _trialsPerBlock = trialsPerBlock;
            this.TrialRange = CalcRange(blockNumber);
            this.Orientation = orientation;
            this._trials = new List<SpatialTrial>();
            this.GenerateTrials();
        }


        /// <summary>
        /// TrialBlock constructor.
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
                    this._trials.Add(trial);
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
            int n = this._trials.Count;

            for (int i = this._trials.Count - 1; i >= 0; i--)
            {
                int rnd = random.Next(i + 1);

                SpatialTrial value = this._trials[rnd];
                this._trials[rnd] = this._trials[i];
                this._trials[i] = value;
            }
        }


        /// <summary>
        /// Method numbers the instance's list of trials sequentially.
        /// </summary>
        private void NumberTrials()
        {
            int trialNumber = this.TrialRange.Min;

            for (int i = 0; i < this._trials.Count; i++)
            {
                if (this._trials[i].TrialNumber == -1)
                {
                    this._trials[i].TrialNumber = trialNumber + i;
                }
            }
        }


        /// <summary>
        /// Method prints the instance's list of trials with their number, orientation, and targets. 
        /// For debugging purposes.
        /// </summary>
        private void PrintTrials()
        {
            foreach (SpatialTrial trial in this._trials)
            {
                System.Diagnostics.Debug.WriteLine($"TrialNumber: {trial.TrialNumber}; Orientation: {trial.Orientation}; TrialTargets: {trial.TrialTargets}");
            }
        }


        /// <summary>
        /// With given block number, returns a Range object with the min and max calculated.
        /// </summary>
        /// <param name="blockNumber">The block number used for calculating trial numbers.</param>
        /// <returns></returns>
        private Range CalcRange(int blockNumber)
        {
            int max = blockNumber * this._trialsPerBlock;
            int min = max - (this._trialsPerBlock - 1);

            Range range = new Range(min, max);

            return range;
        }
    }
}
