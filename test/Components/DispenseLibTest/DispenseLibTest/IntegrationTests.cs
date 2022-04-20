﻿using System;
using NUnit.Framework;
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
        public void Test1()
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
        public void TestTemp()
        {

            using (D2Controller controller = new D2Controller())
            {
                controller.OpenComms("COM18", 115200);

                
                Console.WriteLine(controller.ControllerArms.ReadString(ControllerParam.UserData1));
                Console.WriteLine(controller.ControllerArms.ReadString(ControllerParam.UserData1+1));
                Console.WriteLine(controller.ControllerArms.ReadString(ControllerParam.UserData1+2));
                Console.WriteLine(controller.ControllerArms.ReadString(ControllerParam.UserData1+3));

            }


        }

    }
}