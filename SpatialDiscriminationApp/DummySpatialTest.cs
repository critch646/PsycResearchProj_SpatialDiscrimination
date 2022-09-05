using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpatialDiscriminationApp.enums;

namespace SpatialDiscriminationApp
{
    public class DummySpatialTest
    {
        public SpatialTest DummyTest { get; set; }

        public DummySpatialTest(UserPreferences preferences)
        {
            this.DummyTest = new SpatialTest(preferences.CurrentParticipantID, preferences.BlocksPerTest, preferences.TrialsPerBlock);

            bool testFinished = false;

            while (!testFinished)
            {
                if (DummyTest.GetOrientation() == Orientation.horizontal)
                {
                    DummyTest.EvaluateResponse(500, System.Windows.Input.Key.Left);
                } else
                {
                    DummyTest.EvaluateResponse(500, System.Windows.Input.Key.Up);
                }

                testFinished = DummyTest.BumpTest();
            }
        }
    }
}
