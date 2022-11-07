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

using NUnit.Framework;
using UKRobotics.Common.Maths;
using UKRobotics.D2.DispenseLib.Common;
using UKRobotics.D2.DispenseLib.Protocol;

namespace UKRobotics.D2.DispenseLibTest.Protocol
{
    [TestFixture]
    public class ProtocolDataTest
    {

        
        [Test]
        public void Test()
        {
            ProtocolData protocol = ProtocolData.FromJson(Json1);
        
            Assert.AreEqual("61bf7375aeb558fd38a3db9e", protocol.Id);
            Assert.AreEqual("myrealprotocol", protocol.Name);
        
            ProtocolWell well = protocol.GetProtocolWell("A1");
            Assert.AreEqual("A1", well.WellName);
            Assert.AreEqual(1, well.GetValveCommand(1).ValveNumber);
            Assert.AreEqual(42, well.GetValveCommand(1).Volume.GetValue(VolumeUnitType.ul));
            Assert.AreEqual(2, well.GetValveCommand(2).ValveNumber);
            Assert.AreEqual(69, well.GetValveCommand(2).Volume.GetValue(VolumeUnitType.ul));
        
            well = protocol.GetProtocolWell("A2");
            Assert.AreEqual("A2", well.WellName);
            Assert.AreEqual(1, well.GetValveCommand(1).ValveNumber);
            Assert.AreEqual(42, well.GetValveCommand(1).Volume.GetValue(VolumeUnitType.ul));
            Assert.AreEqual(2, well.GetValveCommand(2).ValveNumber);
            Assert.AreEqual(0, well.GetValveCommand(2).Volume.GetValue(VolumeUnitType.ul));
        
            Assert.AreEqual(96, protocol.Wells.Count);

        }

        public const string Json1 = @"{
  ""protocol"": {
    ""_id"": ""61bf7375aeb558fd38a3db9e"",
    ""name"": ""myrealprotocol"",
    ""notes"": ""some notes"",
    ""wells"": [
      {
        ""wellName"": ""A1"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 69
          }
        ]
      },
      {
        ""wellName"": ""A2"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
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
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""A4"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""A5"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""A6"",
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
        ""wellName"": ""A7"",
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
        ""wellName"": ""A8"",
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
        ""wellName"": ""A9"",
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
        ""wellName"": ""A10"",
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
        ""wellName"": ""A11"",
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
        ""wellName"": ""A12"",
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
            ""volumeUl"": 42
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
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B3"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B4"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B5"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""B6"",
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
        ""wellName"": ""B7"",
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
        ""wellName"": ""B8"",
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
        ""wellName"": ""B9"",
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
        ""wellName"": ""B10"",
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
        ""wellName"": ""B11"",
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
        ""wellName"": ""B12"",
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
        ""wellName"": ""C1"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""C2"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""C3"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""C4"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""C5"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""C6"",
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
        ""wellName"": ""C7"",
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
        ""wellName"": ""C8"",
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
        ""wellName"": ""C9"",
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
        ""wellName"": ""C10"",
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
        ""wellName"": ""C11"",
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
        ""wellName"": ""C12"",
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
        ""wellName"": ""D1"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""D2"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""D3"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""D4"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""D5"",
        ""valves"": [
          {
            ""valveNumber"": 1,
            ""volumeUl"": 42
          },
          {
            ""valveNumber"": 2,
            ""volumeUl"": 0
          }
        ]
      },
      {
        ""wellName"": ""D6"",
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
        ""wellName"": ""D7"",
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
        ""wellName"": ""D8"",
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
        ""wellName"": ""D9"",
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
        ""wellName"": ""D10"",
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
        ""wellName"": ""D11"",
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
        ""wellName"": ""D12"",
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
        ""wellName"": ""E1"",
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
        ""wellName"": ""E2"",
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
        ""wellName"": ""E3"",
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
        ""wellName"": ""E4"",
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
        ""wellName"": ""E5"",
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
        ""wellName"": ""E6"",
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
        ""wellName"": ""E7"",
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
        ""wellName"": ""E8"",
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
        ""wellName"": ""E9"",
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
        ""wellName"": ""E10"",
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
        ""wellName"": ""E11"",
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
        ""wellName"": ""E12"",
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
        ""wellName"": ""F1"",
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
        ""wellName"": ""F2"",
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
        ""wellName"": ""F3"",
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
        ""wellName"": ""F4"",
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
        ""wellName"": ""F5"",
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
        ""wellName"": ""F6"",
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
        ""wellName"": ""F7"",
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
        ""wellName"": ""F8"",
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
        ""wellName"": ""F9"",
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
        ""wellName"": ""F10"",
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
        ""wellName"": ""F11"",
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
        ""wellName"": ""F12"",
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
        ""wellName"": ""G1"",
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
        ""wellName"": ""G2"",
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
        ""wellName"": ""G3"",
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
        ""wellName"": ""G4"",
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
        ""wellName"": ""G5"",
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
        ""wellName"": ""G6"",
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
        ""wellName"": ""G7"",
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
        ""wellName"": ""G8"",
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
        ""wellName"": ""G9"",
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
        ""wellName"": ""G10"",
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
        ""wellName"": ""G11"",
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
        ""wellName"": ""G12"",
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
        ""wellName"": ""H1"",
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
        ""wellName"": ""H2"",
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
        ""wellName"": ""H3"",
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
        ""wellName"": ""H4"",
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
        ""wellName"": ""H5"",
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
        ""wellName"": ""H6"",
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
        ""wellName"": ""H7"",
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
        ""wellName"": ""H8"",
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
        ""wellName"": ""H9"",
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
        ""wellName"": ""H10"",
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
        ""wellName"": ""H11"",
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
        ""wellName"": ""H12"",
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
    ],
    ""modifiedTime"": ""2021-12-19T18:01:25.036Z"",
    ""userId"": ""b2b33a9b-9df9-433b-99c7-4ac275104487"",
    ""userEmail"": ""mcounsell0@googlemail.com"",
    ""uuid"": ""843b041cd621e78ae9e000ecc99909e1""
  }
}";


    }
}