using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UKRobotics.Common.Maths;
using UKRobotics.D2.DispenseLib.Common;

namespace UKRobotics.D2.DispenseLib.Protocol
{
    [DataContract]
    public class ProtocolData
    {

        [DataMember(Name="_id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name="wells")]
        public List<ProtocolWell> Wells { get; set; } = new List<ProtocolWell>();


        public Volume GetDispenseVolume(SBSWellAddress well, int valveNumber)
        {
            ProtocolWell protocolWell = GetProtocolWell(well);
            return protocolWell.GetVolume(valveNumber);
        }

        public ProtocolWell GetProtocolWell(SBSWellAddress well)
        {
            return GetProtocolWell(well.ToString());
        }

        public ProtocolWell GetProtocolWell(string wellName)
        {
            return Wells.Single(x => x.WellName.Equals(wellName));
        }

        public static ProtocolData FromJson(string json)
        {
            return JsonUtils.DeserializeObject<ProtocolDataDoc>(json).ProtocolData;
        }

        /// <summary>
        /// small class to match json structure to aid serialization
        /// </summary>
        [DataContract]
        private class ProtocolDataDoc
        {
            [DataMember(Name = "protocol")]
            public ProtocolData ProtocolData { get; set; }
        }

    }


}