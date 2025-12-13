using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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

        // Internal class to map the JSON structure of labware_library.json
        [DataContract]
        private class LabwareLibrary
        {
            [DataMember(Name = "Plates")]
            public List<PlateTypeData> Plates { get; set; }
        }

        public static PlateTypeData GetPlateTypeData(string guid)
        {
            try
            {
                // 1. Try to fetch from the online library first
                Uri uri = new Uri($"https://labware.ukrobotics.app/{guid}.json");

                WebRequest request = WebRequest.Create(uri);
                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseData = reader.ReadToEnd();
                    return PlateTypeData.FromJson(responseData);
                }
            }
            catch (Exception)
            {
                // 2. Fallback: Load the embedded labware_library.json
                var assembly = Assembly.GetExecutingAssembly();

                // A. Try the standard expected path (Namespace.Folder.Filename)
                string resourceName = "UKRobotics.D2.DispenseLib.Labware.labware_library.json";

                // B. Safety Check: If that specific path doesn't exist, search for it.
                // This handles cases where the DefaultNamespace might differ from the folder structure.
                if (assembly.GetManifestResourceInfo(resourceName) == null)
                {
                    resourceName = assembly.GetManifestResourceNames()
                        .FirstOrDefault(r => r.EndsWith("labware_library.json", StringComparison.InvariantCultureIgnoreCase));
                }

                if (!string.IsNullOrEmpty(resourceName))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonContent = reader.ReadToEnd();

                        // Deserialize the library container
                        var library = JsonUtils.DeserializeObject<LabwareLibrary>(jsonContent);

                        if (library != null && library.Plates != null)
                        {
                            var match = library.Plates.FirstOrDefault(p =>
                                string.Equals(p.Id, guid, StringComparison.OrdinalIgnoreCase));

                            if (match != null)
                            {
                                return match;
                            }
                        }
                    }
                }

                // 3. If not found locally or in resources, rethrow
                throw;
            }
        }
    }
}