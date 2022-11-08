﻿using System;
using NUnit.Framework;
using UKRobotics.Common.Maths;
using UKRobotics.D2.DispenseLib;
using UKRobotics.MotorControllerLib;

namespace UKRobotics.D2.DispenseLibTest
{
    [TestFixture]
    [Explicit]
    public class IntegrationTests
    {

        [Test]
        [Explicit]
        public void TestRunDispense()
        {

            using (D2Controller controller = new D2Controller())
            {
                controller.OpenComms("COM18", 115200);

                Console.WriteLine(controller.ReadSerialIDFromDevice());

                controller.RunDispense("14f7fcb6f7be8f4676bcbd4b7c262c56", "5c527097-5a5c-4a97-9263-02b05f9a95cd");

            }


        }

        [Test]
        [Explicit]
        public void TestFlush()
        {

            using (D2Controller controller = new D2Controller())
            {
                controller.OpenComms("COM7");

                controller.Flush(1, Volume.Parse("100ul"));
                controller.Flush(2, Volume.Parse("100ul"));

            }


        }


    }
}