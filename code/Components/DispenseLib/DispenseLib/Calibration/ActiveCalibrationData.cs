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
using System.Linq;
using System.Runtime.Serialization;
using UKRobotics.D2.DispenseLib.Common;

namespace UKRobotics.D2.DispenseLib.Calibration
{
    [DataContract]
    public class ActiveCalibrationData
    {

        [DataMember(Name= "activeCalibrations")]
        public List<ChannelCalibration> Calibrations { get; set; } = new List<ChannelCalibration>();


        public static ActiveCalibrationData FromJson(string json)
        {
            ActiveCalibrationData calibrationDataDoc = JsonUtils.DeserializeObject<ActiveCalibrationData>(json);

            UpdateVolumePerShots(calibrationDataDoc);

            return calibrationDataDoc;
        }

        public static void UpdateVolumePerShots(ActiveCalibrationData calibrationDataDoc)
        {
            // The VolumePerShot value is required in the table to allow us to interpolate but it is not saved in the DB, so we need to 
            // update ( apply density ) before we can use the calibration data.
            foreach (ChannelCalibration activeCalibration in calibrationDataDoc.Calibrations)
            {
                foreach (CalibrationTable calibration in activeCalibration.Calibrations)
                {
                    calibration.UpdateVolumePerShots();
                }
            }
        }


        public CalibrationTable GetCalibrationForValveNumber(int valveNumber)
        {
            foreach (var activeCalibration in Calibrations)
            {
                if (activeCalibration.ValveChannelNumber == valveNumber)
                {
                    return activeCalibration.Calibrations.First();
                }
            }

            throw new Exception($"Invalid valve number {valveNumber}");
        }
    }

 

}