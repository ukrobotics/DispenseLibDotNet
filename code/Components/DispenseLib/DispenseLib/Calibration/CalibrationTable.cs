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
using System.Runtime.Serialization;
using UKRobotics.Common.Maths;

namespace UKRobotics.D2.DispenseLib.Calibration
{
    [DataContract]
    public class CalibrationTable
    {

        [DataMember(Name="_id")]
        public string Id { get; set; }

        [DataMember(Name = "density")]
        public double DensityJson
        {
            get
            {
                return Density.CalculateMass(new Volume(1, VolumeUnitType.l)).Grams;
            }
            set
            {
                Density = new Density(new Mass(value, MassUnitType.g), VolumeUnitType.l);
            }
        }

        [IgnoreDataMember]
        public Density Density { get; set; }

        [DataMember(Name= "fluidName")]
        public string FluidName { get; set; }

        [DataMember(Name = "pressure")]
        public double Pressure { get; set; }

        [DataMember(Name= "points")]
        public List<CalibrationPoint> Points { get; set; } = new List<CalibrationPoint>();

        public void UpdateVolumePerShots()
        {
            foreach (CalibrationPoint point in Points)
            {
                point.SetDensity(Density);
            }
        }


        //
        // public static Calibration CreateFromDynamic(dynamic d)
        // {
        //     Calibration calibration = new Calibration();
        //
        //     calibration.ValveChannelNumber = d.valveChannelNumber;
        //
        //     dynamic calibrationDynamic = d.calibration[0];// why array???
        //     calibration.Density = new Density(new Mass((double)calibrationDynamic.density, MassUnitType.g), VolumeUnitType.l);
        //     calibration.FluidName = calibrationDynamic.fluidName;
        //     calibration.Pressure = (double)calibrationDynamic.pressure;
        //
        //     foreach (dynamic pointDynamic in calibrationDynamic.points)
        //     {
        //         calibration.Points.Add(CalibrationPoint.CreateFromDynamic(pointDynamic, calibration.Density));
        //     }
        //
        //     return calibration;
        //
        // }

        public int VolumeToOpenTime(Volume volume)
        {

            if (Points.Count < 2)
            {
                //return 0;
                throw new Exception("No calibration data !");
            }

            if (volume < Points[0].VolumePerShot)
            {
                // below the bottom of our scale... error?? or zero...??
                return 0;
            }

            var prevPoint = Points[0];
            for (var pointIndex = 1; pointIndex < Points.Count; pointIndex++)
            {
                var thisPoint = Points[pointIndex];

                if (volume >= prevPoint.VolumePerShot && volume <= thisPoint.VolumePerShot)
                {
                    //between these two points
                    var openTime = InterpolationUtils.Interpolate(
                        prevPoint.OpenTimeUSecs,
                        thisPoint.OpenTimeUSecs,
                        prevPoint.VolumePerShot.Litres,
                        thisPoint.VolumePerShot.Litres,
                        volume.Litres
                    );
                    return (int)Math.Round(openTime);
                }

                prevPoint = Points[pointIndex];
            }


            /////
            // volume is above the top of our scale.  use a linear calc based on the top of our scale

            var topScaleVolumePicoLitre = prevPoint.VolumePerShot.GetValue(VolumeUnitType.pl);
            var topScaleOpenDurationUsecs = prevPoint.OpenTimeUSecs;
            var topScalePicoLitrePerUsec = topScaleVolumePicoLitre / topScaleOpenDurationUsecs;
            return (int)Math.Round(volume.GetValue(VolumeUnitType.pl) / topScalePicoLitrePerUsec);
        }



    }

}