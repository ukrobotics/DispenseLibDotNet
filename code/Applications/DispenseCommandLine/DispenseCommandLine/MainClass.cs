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
using UKRobotics.Common;
using UKRobotics.D2.DispenseLib;

namespace DispenseCommandLine
{

    /// <summary>
    ///
    /// The main() class for the EXE
    ///
    /// This EXE allows you to run the D2 dispenser from the command line by providing a COM port, a protocol ID and a plate type ID.
    /// 
    /// See the following example:
    /// 
    /// Example:
    ///  DispenseCommandLine.exe -ComPort COM9 -ProtocolId d338f60cb0d79fb0d16c00966f373a58 -PlateTypeId 3c0cdfed-19f9-430f-89e2-29ff7c5f1f20
    ///
    /// The protocol id is taken from the webapp GUI, see the protocol meta info tab
    /// The plate type id is the guid for the plate type. See this list of plate types https://labware.ukrobotics.app/  or on the webapp gui
    /// </summary>
    class MainClass
    {
        private const int SuccessReturnCode = 0;
        private const int ErrorReturnCode = 1;

        private const string ComPortArgName = "ComPort";
        private const string ProtocolIdArgName = "ProtocolId";
        private const string PlateTypeIdArgName = "PlateTypeId";


        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">
        ///  
        /// </param>
        /// <returns></returns>
        static int Main(string[] args)
        {


            D2Controller controller = null;
            try
            {
                controller = new D2Controller();

                string comPort = GetArg(args, ComPortArgName);
                controller.OpenComms(comPort);

                string protocolId = GetArg(args, ProtocolIdArgName);
                string plateTypeId = GetArg(args, PlateTypeIdArgName);
                controller.RunDispense(protocolId, plateTypeId);// this blocks until complete

                return SuccessReturnCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Dispense command failed " + e.Message);
                return ErrorReturnCode;
            }
            finally
            {
                controller?.Dispose();
            }


        }

        private static string GetArg(string[] args, string argName)
        {
            string argValue = CommandLineArgUtils.GetArgOrNull(args, argName);
            if (null == argValue)
            {
                throw new Exception($"Missing arg supplied on command line: '{argName}'");
            }

            return argValue;
        }


    }
}
