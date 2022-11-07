using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using UKRobotics.D2.DispenseLib.Calibration;
using UKRobotics.D2.DispenseLib.Common;
using UKRobotics.D2.DispenseLib.Labware;
using UKRobotics.D2.DispenseLib.Protocol;

namespace UKRobotics.D2.DispenseLib.DataAccess
{
    public class D2DataAccess
    {


        private const string ServerUrlBase = @"https://dispense.ukrobotics.app/.netlify/functions/";

        public static Uri FunctionUrl(string funcName)
        {
            return new Uri(ServerUrlBase + funcName);
        }

        [DataContract]
        private class DeviceMeta
        {
            [DataMember(Name = "serialId")]
            public string SerialId { get; set; }

            public DeviceMeta(string serialId)
            {
                SerialId = serialId;
            }
        }

        [DataContract]
        private class GetActiveCalibrationRequest
        {
            [DataMember(Name = "deviceMeta")]
            public DeviceMeta DeviceMeta { get; set; }

            public GetActiveCalibrationRequest(string deviceSerialId)
            {
                DeviceMeta = new DeviceMeta(deviceSerialId);
            }
        }

        public static ActiveCalibrationData GetActiveCalibrationData(string deviceSerialId)
        {

            using (var client = new WebClient())
            {
                var requestData = JsonUtils.SerializeObject(new GetActiveCalibrationRequest(deviceSerialId));

                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                string responseData = client.UploadString(FunctionUrl("calibrationActiveGetAll"), "POST", requestData);

                return ActiveCalibrationData.FromJson(responseData);
            }

        }

        [DataContract]
        private class GetProtocolRequest
        {
            [DataMember(Name= "protocoluuid")]
            public string ProtocolUUID { get; set; }

            public GetProtocolRequest(string protocolUuid)
            {
                ProtocolUUID = protocolUuid;
            }
        }
        
        public static ProtocolData GetProtocol(string guid)
        {
        
            using (var client = new WebClient())
            {
                var requestData = JsonUtils.SerializeObject(new GetProtocolRequest(guid));
        
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var responseData = client.UploadString(FunctionUrl("protocolFindOneByUUID"), "POST", requestData);
        
                return ProtocolData.FromJson(responseData);
            }
        
        }
        
        public static PlateTypeData GetPlateTypeData(string guid)
        {
            // 
            Uri uri = new Uri($"https://labware.ukrobotics.app/{guid}.json");
        
            WebRequest request = WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseData = reader.ReadToEnd();
            return PlateTypeData.FromJson(responseData);
        
        }

    }
}