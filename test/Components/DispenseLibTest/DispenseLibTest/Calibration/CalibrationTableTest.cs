/*
MIT License

Copyright (c) 2021 UK ROBOTICS

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

____________________________________________________________________________

For support - please contact us at  www.ukrobotics.com
 */

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UKRobotics.Common.Maths;
using UKRobotics.D2.DispenseLib.Calibration;

namespace UKRobotics.D2.DispenseLibTest.Calibration
{

    [TestFixture]
    public class CalibrationTableTest
    {

         [Test]
         public void Test()
         {
             CalibrationTable calibration = new CalibrationTable();
             calibration.Density = new Density(new Mass(1, MassUnitType.kg), VolumeUnitType.l);

             calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 10, VolumePerShot = new Volume(5, VolumeUnitType.ul) });
             calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 40, VolumePerShot = new Volume(20, VolumeUnitType.ul) });
             calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 100, VolumePerShot = new Volume(100, VolumeUnitType.ul) });
             calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 300, VolumePerShot = new Volume(300, VolumeUnitType.ul) });
             calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 500, VolumePerShot = new Volume(500, VolumeUnitType.ul) });

             Assert.AreEqual(10, calibration.VolumeToOpenTime(Volume.Parse("5ul")));
             Assert.AreEqual(20, calibration.VolumeToOpenTime(Volume.Parse("10ul")));
             Assert.AreEqual(62, calibration.VolumeToOpenTime(Volume.Parse("50ul")));
             Assert.AreEqual(200, calibration.VolumeToOpenTime(Volume.Parse("200ul")));
             Assert.AreEqual(400, calibration.VolumeToOpenTime(Volume.Parse("400ul")));
             Assert.AreEqual(1000, calibration.VolumeToOpenTime(Volume.Parse("1000ul")));

             Assert.AreEqual(0, calibration.VolumeToOpenTime(Volume.Parse("1ul"))); // too low

         }


    }
}