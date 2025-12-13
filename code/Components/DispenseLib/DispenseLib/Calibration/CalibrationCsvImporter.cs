using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UKRobotics.Common.Maths;

namespace UKRobotics.D2.DispenseLib.Calibration
{
    public static class CalibrationCsvImporter
    {
        /// <summary>
        /// Creates an ActiveCalibrationData object from CSV files.
        /// </summary>
        /// <param name="valve1CsvPath">Path to the CSV file for Valve 1 (required)</param>
        /// <param name="valve2CsvPath">Path to the CSV file for Valve 2 (optional)</param>
        /// <returns>A fully populated ActiveCalibrationData object</returns>
        public static ActiveCalibrationData Import(string valve1CsvPath, string valve2CsvPath = null)
        {
            var activeCalibration = new ActiveCalibrationData();

            // Load Valve 1
            if (string.IsNullOrEmpty(valve1CsvPath) || !File.Exists(valve1CsvPath))
            {
                throw new FileNotFoundException($"Calibration file for Valve 1 not found: {valve1CsvPath}");
            }
            var table1 = ParseCsv(valve1CsvPath);
            activeCalibration.Calibrations.Add(new ChannelCalibration(1, table1));

            // Load Valve 2 (if provided)
            if (!string.IsNullOrEmpty(valve2CsvPath))
            {
                if (!File.Exists(valve2CsvPath))
                {
                    throw new FileNotFoundException($"Calibration file for Valve 2 not found: {valve2CsvPath}");
                }
                var table2 = ParseCsv(valve2CsvPath);
                activeCalibration.Calibrations.Add(new ChannelCalibration(2, table2));
            }

            // CRITICAL: Calculate volume per shot for all points based on the loaded density
            ActiveCalibrationData.UpdateVolumePerShots(activeCalibration);

            return activeCalibration;
        }

        private static CalibrationTable ParseCsv(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 4) throw new InvalidDataException("Calibration CSV is too short.");

            // 1. Parse Metadata (Line 2, 0-based index 1)
            // Header: valveType,fluidName,density,pressure,notes,
            // Values: 19524,DMSO new valve,998.201,0.3,Calibration...
            var metaParts = lines[1].Split(',');
            if (metaParts.Length < 4) throw new InvalidDataException("Invalid metadata format in CSV.");

            var table = new CalibrationTable
            {
                Id = Guid.NewGuid().ToString(), // Generate a temp ID
                FluidName = metaParts[1].Trim(),
                DensityJson = double.Parse(metaParts[2].Trim(), CultureInfo.InvariantCulture),
                Pressure = double.Parse(metaParts[3].Trim(), CultureInfo.InvariantCulture),
                Points = new List<CalibrationPoint>()
            };

            // 2. Find Data Start
            // Look for the header line starting with "openTimeUSecs"
            int dataStartIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("openTimeUSecs", StringComparison.OrdinalIgnoreCase))
                {
                    dataStartIndex = i + 1;
                    break;
                }
            }

            if (dataStartIndex == -1) throw new InvalidDataException("Could not find 'openTimeUSecs' header row.");

            // 3. Parse Data Points
            for (int i = dataStartIndex; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                // Format: openTimeUSecs,interShotTimeUSecs,shotCount,massGrams,...
                if (parts.Length < 4) continue;

                // Parse columns
                double openTime = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                double interShot = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
                int shotCount = int.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
                double mass = double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture);

                // Skip invalid points (zero open time or zero mass often indicates a baseline/dummy row)
                if (openTime <= 0 || mass <= 0) continue;

                table.Points.Add(new CalibrationPoint
                {
                    OpenTimeUSecs = openTime,
                    InterShotTimeUSecs = interShot,
                    ShotCount = shotCount,
                    MassGramms = mass
                });
            }

            return table;
        }
    }
}