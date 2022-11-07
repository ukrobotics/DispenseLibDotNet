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
    public class CalibrationDataTest
    {



         [Test]
         public void TestFromJson()
         {


             ActiveCalibrationData calibrationDataDoc = ActiveCalibrationData.FromJson(Json1);

             Assert.AreEqual(2, calibrationDataDoc.Calibrations.Count);

             {
                 ChannelCalibration activeCalibration = calibrationDataDoc.Calibrations[0];
                 CalibrationTable cal = activeCalibration.Calibrations.First();
                 Assert.AreEqual(1, activeCalibration.ValveChannelNumber);
                 Assert.AreEqual("Water_4Point", cal.FluidName);
                 Assert.AreEqual("998.201g/l", cal.Density.ToString());
                 Assert.AreEqual(0.5, cal.Pressure);

                 Queue<CalibrationPoint> pointsQueue = new Queue<CalibrationPoint>(cal.Points);

                 AssertPoint(200, 4200, 0.183, pointsQueue.Dequeue());
                 AssertPoint(300, 2600, 0.1375, pointsQueue.Dequeue());
                 AssertPoint(500, 2000, 0.1235, pointsQueue.Dequeue());
                 AssertPoint(1000, 1600, 0.1415, pointsQueue.Dequeue());
                 AssertPoint(2500, 750, 0.124, pointsQueue.Dequeue());
                 AssertPoint(4000, 480, 0.142, pointsQueue.Dequeue());
                 AssertPoint(8000, 250, 0.201, pointsQueue.Dequeue());
                 AssertPoint(10000, 200, 0.2205, pointsQueue.Dequeue());
                 AssertPoint(20000, 100, 0.284, pointsQueue.Dequeue());
                 AssertPoint(1000000, 1, 0.1865, pointsQueue.Dequeue());

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

                 AssertPoint(200, 4200, 0.183, pointsQueue.Dequeue());
                 AssertPoint(300, 2600, 0.1375, pointsQueue.Dequeue());
                 AssertPoint(500, 2000, 0.1235, pointsQueue.Dequeue());
                 AssertPoint(1000, 1600, 0.1415, pointsQueue.Dequeue());
                 AssertPoint(2500, 750, 0.124, pointsQueue.Dequeue());
                 AssertPoint(4000, 480, 0.142, pointsQueue.Dequeue());
                 AssertPoint(8000, 250, 0.201, pointsQueue.Dequeue());
                 AssertPoint(10000, 200, 0.2205, pointsQueue.Dequeue());
                 AssertPoint(20000, 100, 0.284, pointsQueue.Dequeue());
                 AssertPoint(1000000, 1, 0.1865, pointsQueue.Dequeue());

                 Assert.AreEqual(0, pointsQueue.Count);

             }


         }


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

         private static void AssertPoint(double openTimeUSecs, int shotCount, double massGrams, CalibrationPoint point)
         {
             Assert.AreEqual(openTimeUSecs, point.OpenTimeUSecs);
             Assert.AreEqual(shotCount, point.ShotCount);
             Assert.AreEqual(massGrams, point.Mass.Grams);
         }


        public const string Json1 = @"{
   ""activeCalibrations"":[
      {
         ""_id"":""61bb4ce897bfdcd79eb6b73d"",
         ""deviceSerialId"":""dd042367ca4c57ad9860fe643af3dc6b"",
         ""valveChannelNumber"":1,
         ""calibrationId"":""61ba0aec23f58d497724c48f"",
         ""calibration"":[
            {
               ""_id"":""61ba0aec23f58d497724c48f"",
               ""density"":998.201,
               ""fluidName"":""Water_4Point"",
               ""pressure"":0.5,
               ""valveType"":""19524"",
               ""valveSerialNumber"":"""",
               ""notes"":"""",
               ""points"":[
                  {
                     ""openTimeUSecs"":200,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":4200,
                     ""massGrams"":0.183
                  },
                  {
                     ""openTimeUSecs"":300,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":2600,
                     ""massGrams"":0.1375
                  },
                  {
                     ""openTimeUSecs"":500,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":2000,
                     ""massGrams"":0.1235
                  },
                  {
                     ""openTimeUSecs"":1000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":1600,
                     ""massGrams"":0.1415
                  },
                  {
                     ""openTimeUSecs"":2500,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":750,
                     ""massGrams"":0.124
                  },
                  {
                     ""openTimeUSecs"":4000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":480,
                     ""massGrams"":0.142
                  },
                  {
                     ""openTimeUSecs"":8000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":250,
                     ""massGrams"":0.201
                  },
                  {
                     ""openTimeUSecs"":10000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":200,
                     ""massGrams"":0.2205
                  },
                  {
                     ""openTimeUSecs"":20000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":100,
                     ""massGrams"":0.284
                  },
                  {
                     ""openTimeUSecs"":1000000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":1,
                     ""massGrams"":0.1865
                  }
               ],
               ""modifiedTime"":""2021-12-15T19:01:09.043Z"",
               ""userId"":""d3ca06ac-c9fc-4e68-b7ab-e9dd272cf7c4"",
               ""userEmail"":""mcounsell@ukrobotics.net"",
               ""deviceSerialId"":""dd042367ca4c57ad9860fe643af3dc6b""
            }
         ]
      },
      {
         ""_id"":""61bb4ced97bfdcd79eb6b7c7"",
         ""deviceSerialId"":""dd042367ca4c57ad9860fe643af3dc6b"",
         ""valveChannelNumber"":2,
         ""calibrationId"":""61ba0aec23f58d497724c48f"",
         ""calibration"":[
            {
               ""_id"":""61ba0aec23f58d497724c48f"",
               ""density"":998.201,
               ""fluidName"":""Water_4Point"",
               ""pressure"":0.5,
               ""valveType"":""19524"",
               ""valveSerialNumber"":"""",
               ""notes"":"""",
               ""points"":[
                  {
                     ""openTimeUSecs"":200,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":4200,
                     ""massGrams"":0.183
                  },
                  {
                     ""openTimeUSecs"":300,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":2600,
                     ""massGrams"":0.1375
                  },
                  {
                     ""openTimeUSecs"":500,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":2000,
                     ""massGrams"":0.1235
                  },
                  {
                     ""openTimeUSecs"":1000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":1600,
                     ""massGrams"":0.1415
                  },
                  {
                     ""openTimeUSecs"":2500,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":750,
                     ""massGrams"":0.124
                  },
                  {
                     ""openTimeUSecs"":4000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":480,
                     ""massGrams"":0.142
                  },
                  {
                     ""openTimeUSecs"":8000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":250,
                     ""massGrams"":0.201
                  },
                  {
                     ""openTimeUSecs"":10000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":200,
                     ""massGrams"":0.2205
                  },
                  {
                     ""openTimeUSecs"":20000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":100,
                     ""massGrams"":0.284
                  },
                  {
                     ""openTimeUSecs"":1000000,
                     ""interShotTimeUSecs"":100000,
                     ""shotCount"":1,
                     ""massGrams"":0.1865
                  }
               ],
               ""modifiedTime"":""2021-12-15T19:01:09.043Z"",
               ""userId"":""d3ca06ac-c9fc-4e68-b7ab-e9dd272cf7c4"",
               ""userEmail"":""mcounsell@ukrobotics.net"",
               ""deviceSerialId"":""dd042367ca4c57ad9860fe643af3dc6b""
            }
         ]
      }
   ]
}";


    }
}