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

using System;
using System.Collections.Generic;
using NUnit.Framework;
using UKRobotics.Common.Maths;
using UKRobotics.D2.DispenseLib;
using UKRobotics.D2.DispenseLib.Calibration;
using UKRobotics.D2.DispenseLib.Labware;
using UKRobotics.D2.DispenseLib.Protocol;

namespace UKRobotics.D2.DispenseLibTest
{
    [TestFixture]
    public class D2ControllerTest
    {

        
        [Test]
        public void TestCompile1()
        {
            D2Controller controller = new D2Controller();
        
        
            ActiveCalibrationData calibrationDataDoc = new ActiveCalibrationData();
        
            {
                CalibrationTable calibration = new CalibrationTable();
                calibration.Density = new Density(new Mass(1000, MassUnitType.g), VolumeUnitType.ml);
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 0, VolumePerShot = new Volume(0, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 100, VolumePerShot = new Volume(100, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 300, VolumePerShot = new Volume(300, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 500, VolumePerShot = new Volume(500, VolumeUnitType.ul) });
                calibrationDataDoc.Calibrations.Add(new ChannelCalibration(1, calibration));
            }
        
            {
                CalibrationTable calibration = new CalibrationTable();
                calibration.Density = new Density(new Mass(1000, MassUnitType.g), VolumeUnitType.ml);
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 0, VolumePerShot = new Volume(0, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 100, VolumePerShot = new Volume(100, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 300, VolumePerShot = new Volume(300, VolumeUnitType.ul) });
                calibration.Points.Add(new CalibrationPoint() { OpenTimeUSecs = 500, VolumePerShot = new Volume(500, VolumeUnitType.ul) });
                calibrationDataDoc.Calibrations.Add(new ChannelCalibration(2, calibration));
            }
        
            ProtocolData protocol = ProtocolData.FromJson(ProtocolJson1);
            PlateTypeData plateType = PlateTypeData.FromJson(PlateType1);
        
            // ACT
            var commands = controller.CompileDispense(calibrationDataDoc, protocol, plateType);
        
            foreach (var command in commands)
            {
                Console.WriteLine(command);
            }
        
            //assert
            Queue<string> commandsQueue = new Queue<string>(commands);
            Assert.AreEqual("CLR_VALVE_WELL,1,0", commandsQueue.Dequeue());
            Assert.AreEqual("VALVE_WELL,1,0,1,750,3,20000,10000,-1,10,1,30000,10000,-1,20,1,40000,10000,-1,0,1", commandsQueue.Dequeue());
            Assert.AreEqual("VALVE_WELL,1,0,2,750,3,20000,20000,-1,0,1,30000,20000,-1,150,1,40000,20000,-1,0,1", commandsQueue.Dequeue());
            Assert.AreEqual(0, commandsQueue.Count);
        
        
        
        }

        public const string PlateType1 = @"{
  ""WellCount"": 6,
  ""WellPitch"": 10,
  ""XOffsetA1"": 20,
  ""YOffsetA1"": 10
}";

        public const string ProtocolJson1 = @"{
  ""protocol"": {
    ""_id"": ""61bf7375aeb558fd38a3db9e"",
    ""name"": ""p1"",
    ""wells"": [
      {
        ""wellName"": ""A1"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 10
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""A2"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 20
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""A3"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 0
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B1"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 0
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B2"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 0
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 150
          }
        ]
      },
      {
        ""wellName"": ""B3"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 0
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      }

    ]
  }
}";






    }
}