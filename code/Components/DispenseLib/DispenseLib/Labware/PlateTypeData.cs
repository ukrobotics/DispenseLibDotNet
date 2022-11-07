﻿/*
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


using System.Runtime.Serialization;
using UKRobotics.D2.DispenseLib.Common;

namespace UKRobotics.D2.DispenseLib.Labware
{

    [DataContract]
    public class PlateTypeData
    {

        [DataMember]
        public int WellCount { get; set; }

        [DataMember]
        public double WellPitch { get; set; }

        [DataMember]
        public double XOffsetA1 { get; set; }

        [DataMember]
        public double YOffsetA1 { get; set; }

        [DataMember]
        public double Height { get; set; }

        public static PlateTypeData FromJson(string json)
        {
            return JsonUtils.DeserializeObject<PlateTypeData>(json);
        }
    }
}