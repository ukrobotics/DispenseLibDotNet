using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UKRobotics.D2.DispenseLib.Calibration;

namespace UKRobotics.D2.DispenseLibTest.Calibration
{
    [TestFixture]
    public class ActiveCalibrationDataTest
    {


        [Test]
        public void TestFromJson()
        {


            ActiveCalibrationData calibrationDataDoc = ActiveCalibrationData.FromJson(CalibrationTestUtils.Json1);

            Assert.AreEqual(2, calibrationDataDoc.Calibrations.Count);

            {
                ChannelCalibration activeCalibration = calibrationDataDoc.Calibrations[0];
                CalibrationTable cal = activeCalibration.Calibrations.First();
                Assert.AreEqual(1, activeCalibration.ValveChannelNumber);
                Assert.AreEqual("Water_4Point", cal.FluidName);
                Assert.AreEqual("998.201g/l", cal.Density.ToString());
                Assert.AreEqual(0.5, cal.Pressure);

                Queue<CalibrationPoint> pointsQueue = new Queue<CalibrationPoint>(cal.Points);

                CalibrationTestUtils.AssertPoint(200, 4200, 0.183, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(300, 2600, 0.1375, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(500, 2000, 0.1235, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(1000, 1600, 0.1415, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(2500, 750, 0.124, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(4000, 480, 0.142, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(8000, 250, 0.201, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(10000, 200, 0.2205, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(20000, 100, 0.284, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(1000000, 1, 0.1865, pointsQueue.Dequeue());

                Assert.AreEqual(0, pointsQueue.Count);
            }


            {
                ChannelCalibration activeCalibration = calibrationDataDoc.Calibrations[1];
                CalibrationTable cal = activeCalibration.Calibrations.First();

                Assert.AreEqual(2, activeCalibration.ValveChannelNumber);
                Assert.AreEqual("998.201g/l", cal.Density.ToString());
                Assert.AreEqual("Water_4Point", cal.FluidName);
                Assert.AreEqual(0.5, cal.Pressure);

                Queue<CalibrationPoint> pointsQueue = new Queue<CalibrationPoint>(cal.Points);

                CalibrationTestUtils.AssertPoint(200, 4200, 0.183, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(300, 2600, 0.1375, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(500, 2000, 0.1235, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(1000, 1600, 0.1415, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(2500, 750, 0.124, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(4000, 480, 0.142, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(8000, 250, 0.201, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(10000, 200, 0.2205, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(20000, 100, 0.284, pointsQueue.Dequeue());
                CalibrationTestUtils.AssertPoint(1000000, 1, 0.1865, pointsQueue.Dequeue());

                Assert.AreEqual(0, pointsQueue.Count);

            }


        }

    }
}