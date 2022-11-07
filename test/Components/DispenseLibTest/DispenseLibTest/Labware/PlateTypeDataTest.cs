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
using UKRobotics.D2.DispenseLib.Labware;

namespace UKRobotics.D2.DispenseLibTest.Labware
{
    [TestFixture]
    public class PlateTypeDataTest
    {

        [Test]
        public void Test1()
        {

            string PlateType1 = @"{
                  ""WellCount"": 6,
                  ""WellPitch"": 10.5,
                  ""XOffsetA1"": 20.2,
                  ""YOffsetA1"": 10.1,
                  ""Height"": 12.5
                }";

            PlateTypeData plateType = PlateTypeData.FromJson(PlateType1);
            Assert.AreEqual(6, plateType.WellCount);
            Assert.AreEqual(10.5, plateType.WellPitch);
            Assert.AreEqual(20.2, plateType.XOffsetA1);
            Assert.AreEqual(10.1, plateType.YOffsetA1);
            Assert.AreEqual(12.5, plateType.Height);

        }

        [Test]
        public void Test2()
        {

            string PlateType1 = @"{""Id"":""3c0cdfed-19f9-430f-89e2-29ff7c5f1f20"",""Manufacturer"":"""",""PartNumber"":"""",""Name"":""Standard ANSI/SLAS Microplate with 96 wells"",""Url"":""https://www.slas.org/education/ansi-slas-microplate-standards/"",""Color"":""clear"",""WellCount"":96,""WellShape"":""round"",""WellBottomShape"":""f"",""WellPitch"":9,""WellDepth"":10,""WellDiameter"":8,""WellVolume"":50,""XOffsetA1"":14.38,""YOffsetA1"":11.24,""Height"":14.35,""GripPortrait"":82,""GripLandscape"":124,""StackHeightIncrement"":0,""SkirtHeight"":2.5,""SkirtY"":85.48,""SkirtX"":127.76,""ExternalClearanceToPlateBottom"":0}";

            PlateTypeData plateType = PlateTypeData.FromJson(PlateType1);
            Assert.AreEqual(96, plateType.WellCount);
            Assert.AreEqual(9, plateType.WellPitch);
            Assert.AreEqual(14.38, plateType.XOffsetA1);
            Assert.AreEqual(11.24, plateType.YOffsetA1);
            Assert.AreEqual(14.35, plateType.Height);

        }
    }
}