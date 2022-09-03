using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerticalDominance.enums;

namespace VerticalDominance
{
    public class DummySpatialTest
    {
        public SpatialTest DummyTest { get; set; }

        DummySpatialTest(int participantID)
        {
            this.DummyTest = new SpatialTest(participantID, 4, 25);

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
