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
        public void Test()
        {

            string PlateType1 = @"{
                  ""WellCount"": 6,
                  ""WellPitch"": 10.5,
                  ""XOffsetA1"": 20.2,
                  ""YOffsetA1"": 10.1
                }";

            PlateTypeData plateType = PlateTypeData.FromJson(PlateType1);
            Assert.AreEqual(6, plateType.WellCount);
            Assert.AreEqual(10.5, plateType.WellPitch);
            Assert.AreEqual(20.2, plateType.XOffsetA1);
            Assert.AreEqual(10.1, plateType.YOffsetA1);

        }
    }
}