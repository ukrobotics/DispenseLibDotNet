using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UKRobotics.Common.Maths;

namespace UKRobotics.D2.DispenseLib.Protocol
{

    [DataContract]
    public class ProtocolWell
    {

        [DataMember(Name="wellName")]
        public string WellName { get; set; }

        [DataMember(Name="valves")]
        public List<ValveCommand> ValveCommands { get; set; } = new List<ValveCommand>();

        public ValveCommand GetValveCommand(int valveNumber)
        {
            return ValveCommands.Single(x => x.ValveNumber == valveNumber);
        }

        public Volume GetVolume(int valveNumber)
        {
            return GetValveCommand(valveNumber).Volume;
        }


        [DataContract]
        public class ValveCommand
        {
            [DataMember(Name="valveNumber")]
            public int ValveNumber { get; set; }

            [DataMember(Name = "volumeUl")]
            public double VolumeUl
            {
                get
                {
                    return Volume.GetValue(VolumeUnitType.ul);
                }
                set
                {
                    Volume = new Volume(value, VolumeUnitType.ul);
                }
            }

            public Volume Volume { get; set; }


        }

    }
}