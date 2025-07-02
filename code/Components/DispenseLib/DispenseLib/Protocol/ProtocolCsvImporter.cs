using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace UKRobotics.D2.DispenseLib.Protocol
{
    /// <summary>
    /// Provides functionality to import a dispense protocol from a CSV file.
    /// </summary>
    public static class ProtocolCsvImporter
    {
        // Define expected header columns for validation.
        private const int ExpectedHeaderCount = 3;
        private const string WellHeader = "Well";
        private const string Valve1Header = "Valve1 (ul)";
        private const string Valve2Header = "Valve2 (ul)";

        /// <summary>
        /// Imports a protocol from a specified CSV file path.
        /// </summary>
        /// <param name="filePath">The full path to the CSV file.</param>
        /// <returns>A ProtocolData object populated with data from the CSV.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the CSV file does not exist at the specified path.</exception>
        /// <exception cref="InvalidDataException">Thrown if the CSV has an invalid header or data format.</exception>
        public static ProtocolData Import(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Protocol CSV file not found.", filePath);
            }

            var lines = File.ReadAllLines(filePath);

            // A valid file must have a header and at least one data row.
            // If not, return an empty protocol.
            if (lines.Length < 2)
            {
                return new ProtocolData { Name = Path.GetFileNameWithoutExtension(filePath), Wells = new List<ProtocolWell>() };
            }

            // First line is the header.
            var header = lines[0].Split(',');
            ValidateHeader(header);

            var protocolData = new ProtocolData
            {
                Id = Guid.NewGuid().ToString(), // Generate a new ID for the imported protocol.
                Name = Path.GetFileNameWithoutExtension(filePath), // Use the filename as the protocol name.
                Wells = new List<ProtocolWell>()
            };

            // Process data rows, starting from the second line.
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue; // Skip any empty lines in the file.
                }

                var values = line.Split(',');
                if (values.Length != ExpectedHeaderCount)
                {
                    throw new InvalidDataException($"Invalid data on line {i + 1}. Expected {ExpectedHeaderCount} columns, but found {values.Length}.");
                }

                var wellName = values[0].Trim();

                // Parse volumes, using InvariantCulture to handle decimal points correctly.
                if (!double.TryParse(values[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double valve1VolumeUl))
                {
                    throw new InvalidDataException($"Invalid volume for Valve 1 on line {i + 1}. Value was '{values[1]}'.");
                }
                if (!double.TryParse(values[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double valve2VolumeUl))
                {
                    throw new InvalidDataException($"Invalid volume for Valve 2 on line {i + 1}. Value was '{values[2]}'.");
                }

                // Create a new ProtocolWell with the parsed data.
                var protocolWell = new ProtocolWell
                {
                    WellName = wellName,
                    ValveCommands = new List<ProtocolWell.ValveCommand>
                    {
                        new ProtocolWell.ValveCommand { ValveNumber = 1, VolumeUl = valve1VolumeUl },
                        new ProtocolWell.ValveCommand { ValveNumber = 2, VolumeUl = valve2VolumeUl }
                    }
                };

                protocolData.Wells.Add(protocolWell);
            }

            return protocolData;
        }

        /// <summary>
        /// Validates the CSV header against the expected format.
        /// </summary>
        private static void ValidateHeader(string[] header)
        {
            if (header.Length != ExpectedHeaderCount)
            {
                throw new InvalidDataException($"Invalid CSV header. Expected {ExpectedHeaderCount} columns, but found {header.Length}.");
            }

            if (header[0].Trim() != WellHeader ||
                header[1].Trim() != Valve1Header ||
                header[2].Trim() != Valve2Header)
            {
                throw new InvalidDataException($"Invalid CSV header. Expected '{WellHeader},{Valve1Header},{Valve2Header}' but found '{string.Join(",", header)}'.");
            }
        }
    }
}
