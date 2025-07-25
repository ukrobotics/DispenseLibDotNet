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
using UKRobotics.D2.DispenseLib.DataAccess;
using UKRobotics.D2.DispenseLib.Protocol;

namespace DispenseCommandLine
{

    /// <summary>
    ///
    /// The main() class for the EXE
    ///
    /// This EXE allows you to run the D2 dispenser from the command line or export protocols.
    /// 
    /// </summary>
    class MainClass
    {
        private const int SuccessReturnCode = 0;
        private const int ErrorReturnCode = 1;

        private const string ComPortArgName = "ComPort";
        private const string ProtocolIdArgName = "ProtocolId";
        private const string PlateTypeIdArgName = "PlateTypeId";
        private const string ProtocolCsvPathArgName = "DispenseCsv";
        private const string ExportCsvPathArgName = "ExportToPath";


        /// <summary>
        /// Main method
        /// </summary>
        static int Main(string[] args)
        {
            D2Controller controller = null;
            try
            {
                // --- Argument Parsing ---
                string comPort = GetArg(args, ComPortArgName, false);
                string plateTypeId = GetArg(args, PlateTypeIdArgName, false);
                string protocolId = GetArg(args, ProtocolIdArgName, false);
                string protocolCsvPath = GetArg(args, ProtocolCsvPathArgName, false);
                string exportToPath = GetArg(args, ExportCsvPathArgName, false);

                // --- Mode 1: Export Protocol ---
                if (!string.IsNullOrEmpty(exportToPath))
                {
                    if (string.IsNullOrEmpty(protocolId))
                    {
                        throw new Exception($"The ' -{ProtocolIdArgName}' argument is required when exporting a protocol.");
                    }

                    Console.WriteLine($"Fetching protocol with ID: {protocolId}...");
                    ProtocolData protocolToExport = D2DataAccess.GetProtocol(protocolId);

                    Console.WriteLine($"Exporting protocol to: {exportToPath}...");
                    ProtocolCsvExporter.Export(protocolToExport, exportToPath);

                    Console.WriteLine("Export completed successfully.");
                    return SuccessReturnCode;
                }

                // --- Mode 2: Run Dispense ---
                // Required arguments for running a dispense
                if (string.IsNullOrEmpty(comPort)) throw new Exception($"Missing required argument: '-{ComPortArgName}'");
                if (string.IsNullOrEmpty(plateTypeId)) throw new Exception($"Missing required argument: '-{PlateTypeIdArgName}'");

                controller = new D2Controller();
                controller.OpenComms(comPort);

                if (!string.IsNullOrEmpty(protocolId) && !string.IsNullOrEmpty(protocolCsvPath))
                {
                    throw new Exception($"Please provide either '-{ProtocolIdArgName}' or '-{ProtocolCsvPathArgName}', but not both.");
                }

                if (!string.IsNullOrEmpty(protocolId))
                {
                    Console.WriteLine($"Running dispense with Protocol ID: {protocolId}");
                    controller.RunDispense(protocolId, plateTypeId);
                }
                else if (!string.IsNullOrEmpty(protocolCsvPath))
                {
                    Console.WriteLine($"Importing and running dispense from: {protocolCsvPath}");
                    ProtocolData protocolData = ProtocolCsvImporter.Import(protocolCsvPath);
                    controller.RunDispense(protocolData, plateTypeId);
                }
                else
                {
                    throw new Exception($"Missing protocol source. Please supply either '-{ProtocolIdArgName}' or '-{ProtocolCsvPathArgName}'.");
                }

                Console.WriteLine("Dispense completed successfully.");
                return SuccessReturnCode;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Command failed: " + e.Message);
                return ErrorReturnCode;
            }
            finally
            {
                controller?.Dispose();
            }
        }

        private static string GetArg(string[] args, string argName, bool isRequired)
        {
            string argValue = CommandLineArgUtils.GetArgOrNull(args, argName);
            if (isRequired && string.IsNullOrEmpty(argValue))
            {
                throw new Exception($"Missing required argument on command line: '-{argName}'");
            }

            return argValue;
        }
    }
}
