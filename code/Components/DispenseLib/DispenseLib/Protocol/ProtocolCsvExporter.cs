using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UKRobotics.D2.DispenseLib.Protocol;
using UKRobotics.Common.Maths;

namespace UKRobotics.D2.DispenseLib.Protocol
{
    /// <summary>
    /// Provides functionality to export a dispense protocol to a CSV file.
    /// </summary>
    public static class ProtocolCsvExporter
    {
        /// <summary>
        /// Exports a ProtocolData object to a specified CSV file path.
        /// </summary>
        /// <param name="protocolData">The protocol data to export.</param>
        /// <param name="filePath">The full path where the CSV file will be saved.</param>
        public static void Export(ProtocolData protocolData, string filePath)
        {
            var csvBuilder = new StringBuilder();

            // Append the header row.
            csvBuilder.AppendLine("Well,Valve1 (ul),Valve2 (ul)");

            // Sort wells alphanumerically for consistent output, like A1, A2, B1, B2...
            var sortedWells = protocolData.Wells
                                .OrderBy(w => w.WellName.Length)
                                .ThenBy(w => w.WellName);

            foreach (var well in sortedWells)
            {
                // Get volumes for each valve.
                var valve1Volume = well.GetVolume(1).GetValue(VolumeUnitType.ul);
                var valve2Volume = well.GetVolume(2).GetValue(VolumeUnitType.ul);

                // Format the line using InvariantCulture for consistent decimal points.
                var line = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}",
                    well.WellName,
                    valve1Volume,
                    valve2Volume);

                csvBuilder.AppendLine(line);
            }

            // Write the final CSV string to the specified file.
            File.WriteAllText(filePath, csvBuilder.ToString());
        }
    }
}
