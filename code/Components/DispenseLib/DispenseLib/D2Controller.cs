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
using System.Threading;
using UKRobotics.Common.Enums;
using UKRobotics.Common.Maths;
using UKRobotics.Common.Threading;
using UKRobotics.D2.DispenseLib.Calibration;
using UKRobotics.D2.DispenseLib.Common;
using UKRobotics.D2.DispenseLib.DataAccess;
using UKRobotics.D2.DispenseLib.Labware;
using UKRobotics.D2.DispenseLib.Protocol;
using UKRobotics.MotorControllerLib;

namespace UKRobotics.D2.DispenseLib
{

    /// <summary>
    ///
    /// The primary class for controlling the D2 dispenser.
    ///
    /// To get started, you need to call OpenComms and provide the com port to use.
    /// Next, you can call RunDispense and provide the protocol ID and plate type ID.
    /// 
    /// 
    /// </summary>
    public class D2Controller : IDisposable
    {

        public enum DispenseState
        {
            Error = -1,
            Ended = 1
        }

        public enum ValveCommandState
        {
            Idle = 0,
            Pending = 1
        }


        public const int ValveCount = 2;


        public const ControllerParam SerialIdParamId = (ControllerParam.UserData1 + 60);

        public ControlConnection ControlConnection { get; set; }
        public Controller ControllerArms { get; set; }
        public Controller ControllerZ { get; set; }
        public IAxis ZAxis { get; set; }
        public IAxis Arm1 { get; set; }
        public IAxis Arm2 { get; set; }

        public int ControllerNumberArms { get; protected set; } = 1;
        public int ControllerNumberZAxis { get; protected set; } = 2;
        public int AxisNumberZAxis { get; protected set; } = 1;
        public int ControllerAxisCount { get; protected set; } = 2;


        public D2Controller()
        {
            // default ctor
        }

        protected D2Controller(
            int controllerNumberZAxis = 2, 
            int axisNumberZAxis = 1,
            int controllerAxisCount = 2)
        {
            ControllerNumberZAxis = controllerNumberZAxis;
            AxisNumberZAxis = axisNumberZAxis;
            ControllerAxisCount = controllerAxisCount;
        }


        public void OpenComms(ControlConnection controlConnection)
        {
            ControlConnection = controlConnection;
            ControllerArms = new Controller(ControlConnection, ControllerNumberArms, ControllerAxisCount);
            ControllerZ = new Controller(ControlConnection, ControllerNumberZAxis, ControllerAxisCount);
            ZAxis = ControllerZ.GetAxis(AxisNumberZAxis);
            Arm1 = ControllerArms.GetAxis(1);
            Arm2 = ControllerArms.GetAxis(2);
        }

        public void OpenComms(string comPort, int baud=115200)
        {
            OpenComms(new ControlConnection(comPort, baud));
        }

        public void Dispose()
        {
            try
            {
                ControlConnection?.Dispose();
            }
            catch
            {
            }

            ControlConnection = null;
            ControllerArms = null;
            ControllerZ = null;
            ZAxis = null;
        }

        /// <summary>
        /// Each D2 has a unique ID number to identify the device.
        /// </summary>
        /// <returns></returns>
        public string ReadSerialIDFromDevice()
        {
            return ControllerArms.ReadString(SerialIdParamId);
        }


        /// <summary>
        ///
        /// Run a dispense and block until complete.
        /// 
        /// </summary>
        /// <param name="protocolId">
        /// The protocol ID is created automatically when you edit and save a protocol from the D2's software on [https://dispense.ukrobotics.app](https://dispense.ukrobotics.app)
        /// </param>
        /// <param name="plateTypeGuid">
        /// The plate type ID is taken from our public labware library here: [https://labware.ukrobotics.app/](https://labware.ukrobotics.app/) . If you have an item of labware that is not currently in our library please contact us at info at ukrobotics.net.
        /// </param>
        public void RunDispense(string protocolId, string plateTypeGuid)
        {

            try
            {
                plateTypeGuid = plateTypeGuid.Trim();
                protocolId = protocolId.Trim();

                ClearMotorErrorFlags();

                string deviceSerialId = ReadSerialIDFromDevice();


                var plateType = D2DataAccess.GetPlateTypeData(plateTypeGuid);
                List<string> dispenseCommands = null;

                MethodInvokerThread dataAccessThread = new MethodInvokerThread(new MethodInvokerThread.MethodInvoker(
                    () =>
                    {
                        dispenseCommands = CompileDispense(deviceSerialId, protocolId, plateType);
                    }));

                MethodInvokerThread zAndClampThread = new MethodInvokerThread(new MethodInvokerThread.MethodInvoker(() =>
                {
                    Distance plateHeight = new Distance(plateType.Height, DistanceUnitType.mm);
                    Distance dispenseHeight = plateHeight + Distance.Parse("1mm");
                    MoveZToDispenseHeight(dispenseHeight);

                    SetClamp(true);
                }));


                dataAccessThread.StartThread();
                zAndClampThread.StartThread();
                dataAccessThread.JoinWithExceptionRethrow();
                zAndClampThread.JoinWithExceptionRethrow();


                foreach (string dispenseCommand in dispenseCommands)
                {
                    ControlConnection.SendMessageRaw(dispenseCommand, true, out bool success, out string errorMessage);
                }


                StartDispense(out TimeSpan dispenseDurationEstimate);// this is an under estimate of time that is returned!!
                WaitForDispenseComplete(dispenseDurationEstimate);

            }
            finally
            {
                try
                {
                    DisableAllMotors();
                }
                catch
                {
                }
                try
                {
                    SetClamp(false);
                }
                catch
                {
                }
            }

        }

        /// <summary>
        ///
        /// Flush on the given valve with the given volume.
        ///
        /// This will :
        /// Park the arms
        /// Move the Z axis to 10mm
        ///
        /// 
        /// 
        /// </summary>
        /// <param name="valveNumber"></param>
        /// <param name="flushVolume"></param>
        public void Flush(int valveNumber, Volume flushVolume)
        {


            try
            {
                ClearMotorErrorFlags();


                ParkArms();
                MoveZToDispenseHeight(Distance.Parse("10mm"));
                DisableAllMotors();


                string deviceSerialId = ReadSerialIDFromDevice();

                var calibrationData = D2DataAccess.GetActiveCalibrationData(deviceSerialId);
                int openTimeUsecs = calibrationData.GetCalibrationForValveNumber(valveNumber).VolumeToOpenTime(flushVolume);

                string command = CreateValveCommand(valveNumber, openTimeUsecs, 1, 0);
                ControlConnection.SendMessageRaw(command, true, out bool success, out string errorMessage);
                if (!success)
                {
                    throw new Exception(errorMessage);
                }

                AwaitIdleValveState(TimeSpan.FromMilliseconds(((double)openTimeUsecs / 1000) + 250) );

            }
            finally
            {
                try
                {
                    DisableAllMotors();
                }
                catch
                {
                }
                try
                {
                    SetClamp(false);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Awaits valve idle state
        /// </summary>
        /// <param name="timeout"></param>
        /// <exception cref="Exception">on timeout</exception>
        public void AwaitIdleValveState(TimeSpan timeout)
        {
            DateTime timeoutAt = DateTime.Now + timeout;

            while (true)
            {

                ResponseMessage response = ControlConnection.SendMessageRaw(
                    $"GET_VALVE_STATE,{ControllerNumberArms},0", true, out bool success, out string errorMessage);
                response.GetParameter( 0, out int i );
                ValveCommandState state = (ValveCommandState) i;
                if ( ValveCommandState.Idle == state )
                {
                    break;
                }

                if (DateTime.Now > timeoutAt)
                {
                    throw new Exception("Timeout waiting for valve idle state");
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Raw valve fire, by opentime only ( rather than volume)
        /// </summary>
        /// <param name="valveNumber"></param>
        /// <param name="openTimeUsecs"></param>
        public void FireValve(int valveNumber, int openTimeUsecs)
        {

            string command = CreateValveCommand(valveNumber, openTimeUsecs, 1, 0);
            ControlConnection.SendMessageRaw(command, true, out bool success, out string errorMessage);

            if (!success)
            {
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Set clamp to clamped or released
        /// </summary>
        /// <param name="clamped">true to clamp, false to release</param>
        public void SetClamp(bool clamped)
        {
            int commandValue = 0;
            if (clamped)
            {
                commandValue = 1;
            }

            ControlConnection.SendMessage("CLAMP", ControllerNumberZAxis, 0, commandValue);
        }

        /// <summary>
        ///
        /// Move the valve head to the given dispense height relative to the base of the plate.
        ///
        /// Will block until move completed and will auto home the Z axis if required first.
        ///
        /// Note you do not need to explicitly use this method!  D2Controller.RunDispense() will move to the correct height for the plate type you specify.
        /// This method is just provided for completeness.
        /// 
        /// </summary>
        /// <param name="dispenseHeight">the height, <see cref="Distance.Parse(String)"/> if you want to create an instance from a string eg 10mm </param>
        public void MoveZToDispenseHeight(Distance dispenseHeight)
        {

            if (!ZAxis.ReadBoolean(ControllerParam.IsHomed))
            {
                ZAxis.Home();// the Z is not homed yet, so home it now...
                Thread.Sleep(1000);// just a small wait to ensure the home action has started before we check for completion
                ZAxis.WaitForIsHomed(TimeSpan.FromSeconds(40));
            }

            int heightMicrons = (int)Math.Round(dispenseHeight.GetValue(DistanceUnitType.um));
            ControlConnection.SendMessage("MOVE_Z", ControllerNumberZAxis, AxisNumberZAxis, heightMicrons);
            ZAxis.WaitForPositionSettledAndInRange(TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Park arms
        /// </summary>
        public void ParkArms()
        {
            //clear motor error flags before try park/unpark to clear any prev errors
            ClearMotorErrorFlags();

            ControlConnection.SendMessage("PARK", ControllerNumberArms, 0);
            Thread.Sleep(500);
            Arm1.WaitForPositionSettledAndInRange(TimeSpan.FromSeconds(20));
            Arm2.WaitForPositionSettledAndInRange(TimeSpan.FromSeconds(20));

            DisableArms();
        }

        /// <summary>
        /// Unpark arms
        /// </summary>
        public void UnparkArms()
        {
            //clear motor error flags before try park/unpark to clear any prev errors
            ClearMotorErrorFlags();

            ControlConnection.SendMessage("UNPARK", ControllerNumberArms, 0);
            Thread.Sleep(500);
            Arm1.WaitForPositionSettledAndInRange(TimeSpan.FromSeconds(20));
            Arm2.WaitForPositionSettledAndInRange(TimeSpan.FromSeconds(20));
        }

        /// <summary>
        /// Disable Z Axis Motor
        /// </summary>
        public void DisableZ()
        {
            ZAxis.SetMode(AxisControllerMode.Disabled);
        }

        /// <summary>
        /// Disable Arms Motor
        /// </summary>
        public void DisableArms()
        {
            Arm1.SetMode(AxisControllerMode.Disabled);
            Arm2.SetMode(AxisControllerMode.Disabled);
        }

        /// <summary>
        /// Disable all motors
        /// </summary>
        public void DisableAllMotors()
        {
            ZAxis.SetMode(AxisControllerMode.Disabled);
            Arm1.SetMode(AxisControllerMode.Disabled);
            Arm2.SetMode(AxisControllerMode.Disabled);
        }

        /// <summary>
        /// Clear error flags after arm or z axis motor being blocked.
        /// </summary>
        public void ClearMotorErrorFlags()
        {
            ZAxis.Write(ControllerParam.ErrorCode, 0);
            Arm1.Write(ControllerParam.ErrorCode, 0);
            Arm2.Write(ControllerParam.ErrorCode, 0);
        }

        private void StartDispense(out TimeSpan durationEstimate)
        {

            ResponseMessage response = ControlConnection.SendMessage("DISPENSE", ControllerNumberArms, 0);// Start the dispense
            response.GetParameter(0, out double durationMillis);

            durationEstimate = TimeSpan.FromMilliseconds(durationMillis);
        }

        /// <summary>
        ///
        /// Wait for dispense protocol to be completed.
        /// 
        /// </summary>
        /// <exception cref="Exception">on timeout</exception>
        /// <param name="timeout"></param>
        public void WaitForDispenseComplete(TimeSpan timeout)
        {
            DateTime timeoutAt = DateTime.Now + timeout + TimeSpan.FromSeconds(30);

            while (DateTime.Now < timeoutAt)
            {

                switch (GetDispenseState())
                {
                    case DispenseState.Error:
                        throw new Exception(
                            $"A dispense error occurred. Check the arms for anything that could block motion. Is the plate higher than the chosen type?... are the hoses too short? See https://ukrobotics.tech/docs/d2dispenser/troubleshooting/");
                    case DispenseState.Ended:
                        return;
                    default:
                        break;// running
                }

                Thread.Sleep(100);
            }

            throw new Exception($"Timeout waiting for dispense to finish");

        }

        public DispenseState GetDispenseState()
        {
            var responseMessage = ControlConnection.SendMessage("GET_DISPENSE_STATE", ControllerNumberArms, 0);

            responseMessage.GetParameter(0, out int value);
            return (DispenseState)value;
        }

        /// <summary>
        /// This method provides the same value as the web based JS application and defines the delta distance ( error )
        /// within which the valve will fire.
        /// 
        /// </summary>
        /// <param name="wellCount"></param>
        /// <returns></returns>
        private int GetDispenseWellDeltaMicrons(int wellCount)
        {
            if (wellCount > 384)
            {
                return 250;
            }

            return 750;
        }

        /// <summary>
        /// NOTE THIS IS BASED/COPIED FROM THE WEB APP FROM THE JAVASCRIPT
        /// </summary>
        private XYPoint GetWellXY(PlateTypeData plateType, SBSWellAddress well)
        {

            Distance pitch = new Distance((double)plateType.WellPitch, DistanceUnitType.mm);
            Distance xOffsetA1 = new Distance((double)plateType.XOffsetA1, DistanceUnitType.mm);
            Distance yOffsetA1 = new Distance((double)plateType.YOffsetA1, DistanceUnitType.mm);

            Distance x = new Distance(xOffsetA1) + (pitch * well.Column);
            Distance y = new Distance(yOffsetA1) + (pitch * well.Row);

            return new XYPoint(x, y);
        }


        /// <summary>
        ///
        /// NOTE THIS IS BASED/COPIED FROM THE WEB APP FROM THE JAVASCRIPT FILE
        /// dispense-protocol-lib.js func: toDispenseCommandsJson()
        /// 
        /// 
        /// </summary>
        /// <param name="activeCalibrationData"></param>
        /// <param name="protocolData"></param>
        /// <param name="plateType"></param>
        /// <returns></returns>
        public List<string> CompileDispense(
            ActiveCalibrationData activeCalibrationData, 
            ProtocolData protocolData, 
            PlateTypeData plateType)
        {
            int wellCount = plateType.WellCount;

            List<string> commands = new List<string>();

            commands.Add($"CLR_VALVE_WELL,{ControllerNumberArms},0");

            int dispenseWellDeltaMicrons = GetDispenseWellDeltaMicrons(wellCount);

            for (var valveNumber = 1; valveNumber <= ValveCount; valveNumber++)
            {

                bool reverseLine = false;
                List<List<SBSWellAddress>> wellLines = GetWellSequenceLines(wellCount);
                foreach (List<SBSWellAddress> wellLine in wellLines)
                {

                    SBSWellAddress firstWell = wellLine[0];
                    SBSWellAddress lastWell = wellLine[wellLine.Count - 1];


                    var requestString = $"VALVE_WELL,{ControllerNumberArms},0,{valveNumber},{dispenseWellDeltaMicrons},{wellLine.Count}";

                    int xApproachDirection = -1;
                    if (reverseLine)
                    {
                        xApproachDirection = 1;
                        wellLine.Reverse();
                    }

                    bool nonZeroDispenseOnLine = false;
                    foreach (var well in wellLine)
                    {
                        XYPoint xy = GetWellXY(plateType, well);

                        var wellRequestString = "";
                        wellRequestString += $"{ (int)Math.Round(xy.X.GetValue(DistanceUnitType.um))}";
                        wellRequestString += $",{ (int)Math.Round(xy.Y.GetValue(DistanceUnitType.um))}";
                        wellRequestString += $",{ xApproachDirection}";

                        int durationMicroseconds = GetDispenseDurationMicroseconds(
                            valveNumber,
                            activeCalibrationData,
                            well,
                            protocolData);

                        wellRequestString += $",{ durationMicroseconds}";
                        wellRequestString += ",1";//SHOT COUNT.... 1 shot per well

                        if (durationMicroseconds > 0)
                        {
                            nonZeroDispenseOnLine = true;
                        }

                        requestString += $",{ wellRequestString}";// append params

                    }

                    if (nonZeroDispenseOnLine)
                    {
                        commands.Add(requestString);

                        reverseLine = !reverseLine;
                    }

                }
            }



            return commands;

        }

        /// <summary>
        /// NOTE THIS IS BASED/COPIED FROM THE WEB APP FROM THE JAVASCRIPT
        /// 
        /// </summary>
        /// <param name="valveNumber"></param>
        /// <param name="activeCalibrationData"></param>
        /// <param name="well"></param>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public int GetDispenseDurationMicroseconds(
            int valveNumber,
            ActiveCalibrationData activeCalibrationData,
            SBSWellAddress well,
            ProtocolData protocol)
        {

            int durationMicroseconds = 0;

            CalibrationTable calibration = activeCalibrationData.GetCalibrationForValveNumber(valveNumber);

            Volume dispenseVolume = protocol.GetDispenseVolume(well, valveNumber);
            if (dispenseVolume > D2Constants.MinDispenseVolume)
            {
                durationMicroseconds = calibration.VolumeToOpenTime(dispenseVolume);
            }

            return durationMicroseconds;
        }


        public List<List<SBSWellAddress>> GetWellSequenceLines(int wellCount)
        {
            SBSWellAddress.GetRowAndColumnCountForWellCount(wellCount, out int rowCount, out int columnCount);

            List<List<SBSWellAddress>> wellLines = new List<List<SBSWellAddress>>();

            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                List<SBSWellAddress> lineWells = SBSWellAddress.GetColumnAddressIterator(wellCount, rowIndex);

                wellLines.Add(lineWells);
            }

            return wellLines;
        }

        public List<string> CompileDispense(string deviceSerialId, string protocolId, PlateTypeData plateType)
        {
            ActiveCalibrationData calibration = D2DataAccess.GetActiveCalibrationData(deviceSerialId);
            ProtocolData protocol = D2DataAccess.GetProtocol(protocolId);

            return CompileDispense(calibration, protocol, plateType);
        }


        public string CreateValveCommand(int valveNumber, int openTimeUsecs, int shotCount, int interShotTimeUSecs)
        {
            var command = "VALVE," + ControllerNumberArms + ",0," +
                          valveNumber + "," +
                          openTimeUsecs + "," +
                          shotCount + "," +
                          interShotTimeUSecs;

            return command;
        }

    }
}