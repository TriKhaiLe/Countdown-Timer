using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace ChroniTimer
{
    public class AppSettings
    {
        public string? CustomDataFolder { get; set; }
    }

    /// <summary>
    /// Manages persistence for timer session data.
    /// Stores today's accumulated hours and full history in JSON format.
    /// Supports customizable data storage location.
    /// </summary>
    public class DataManager
    {
        private static readonly string ConfigFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ChroniTimer");
        private static readonly string ConfigFile = Path.Combine(ConfigFolder, "settings.json");

        private string _dataFolder = "";
        private string _todayFile = "";
        private string _historyFile = "";
        private string _colorsFile = "";

        public event Action? DataFolderChanged;

        public static string DefaultDataFolder => ConfigFolder;
        public string DataFolder => _dataFolder;
        public string TodayFilePath => _todayFile;
        public string HistoryFilePath => _historyFile;
        public string ColorsFilePath => _colorsFile;

        public DataManager()
        {
            Directory.CreateDirectory(ConfigFolder);
            string initialFolder = LoadConfiguredDataFolder();
            UpdateDataFolderPaths(initialFolder);
        }

        private static string LoadConfiguredDataFolder()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    var json = File.ReadAllText(ConfigFile);
                    var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                    if (!string.IsNullOrWhiteSpace(settings?.CustomDataFolder))
                    {
                        return settings.CustomDataFolder;
                    }
                }
            }
            catch
            {
                // Fallback to default if settings file is unreadable
            }
            return ConfigFolder;
        }

        private void UpdateDataFolderPaths(string folder)
        {
            _dataFolder = folder;
            Directory.CreateDirectory(_dataFolder);
            _todayFile = Path.Combine(_dataFolder, "today.json");
            _historyFile = Path.Combine(_dataFolder, "history.json");
            _colorsFile = Path.Combine(_dataFolder, "saved_colors.json");
        }

        public void SetDataFolder(string newFolder, bool copyExistingFiles)
        {
            if (string.IsNullOrWhiteSpace(newFolder))
                newFolder = DefaultDataFolder;

            newFolder = Path.GetFullPath(newFolder);
            Directory.CreateDirectory(newFolder);

            string oldFolder = Path.GetFullPath(_dataFolder);
            bool isChanging = !string.Equals(oldFolder, newFolder, StringComparison.OrdinalIgnoreCase);

            if (copyExistingFiles && isChanging)
            {
                CopyFileIfExists(_todayFile, Path.Combine(newFolder, "today.json"));
                CopyFileIfExists(_historyFile, Path.Combine(newFolder, "history.json"));
                CopyFileIfExists(_colorsFile, Path.Combine(newFolder, "saved_colors.json"));
            }

            UpdateDataFolderPaths(newFolder);

            // Persist setting
            bool isDefault = string.Equals(newFolder, Path.GetFullPath(DefaultDataFolder), StringComparison.OrdinalIgnoreCase);
            var settings = new AppSettings
            {
                CustomDataFolder = isDefault ? null : newFolder
            };
            File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(settings, Formatting.Indented));

            DataFolderChanged?.Invoke();
        }

        private static void CopyFileIfExists(string source, string dest)
        {
            if (File.Exists(source))
            {
                File.Copy(source, dest, overwrite: true);
            }
        }

        // ── Today data ──────────────────────────────────────────────

        public DayRecord LoadToday()
        {
            if (!File.Exists(_todayFile))
                return new DayRecord { Date = DateTime.Today.ToString("yyyy-MM-dd"), TotalHours = 0 };

            var json = File.ReadAllText(_todayFile);
            var record = JsonConvert.DeserializeObject<DayRecord>(json);
            if (record == null || record.Date != DateTime.Today.ToString("yyyy-MM-dd"))
            {
                // Day rolled over – archive old record
                if (record != null)
                    AppendToHistory(record);
                return new DayRecord { Date = DateTime.Today.ToString("yyyy-MM-dd"), TotalHours = 0 };
            }
            return record;
        }

        public void SaveToday(DayRecord record)
        {
            File.WriteAllText(_todayFile, JsonConvert.SerializeObject(record, Formatting.Indented));
        }

        // ── History ─────────────────────────────────────────────────

        public List<DayRecord> LoadHistory()
        {
            if (!File.Exists(_historyFile))
                return new List<DayRecord>();
            var json = File.ReadAllText(_historyFile);
            return JsonConvert.DeserializeObject<List<DayRecord>>(json) ?? new List<DayRecord>();
        }

        public void AppendToHistory(DayRecord record)
        {
            var history = LoadHistory();
            // Update existing entry for same date or add new
            var existing = history.FindIndex(h => h.Date == record.Date);
            if (existing >= 0)
                history[existing] = record;
            else
                history.Add(record);
            File.WriteAllText(_historyFile, JsonConvert.SerializeObject(history, Formatting.Indented));
        }

        public void SaveHistory(List<DayRecord> history)
        {
            File.WriteAllText(_historyFile, JsonConvert.SerializeObject(history, Formatting.Indented));
        }

        // ── Saved colors ────────────────────────────────────────────

        public List<GradientColor> LoadSavedColors()
        {
            if (!File.Exists(_colorsFile))
                return new List<GradientColor>();
            var json = File.ReadAllText(_colorsFile);
            return JsonConvert.DeserializeObject<List<GradientColor>>(json) ?? new List<GradientColor>();
        }

        public void SaveColors(List<GradientColor> colors)
        {
            File.WriteAllText(_colorsFile, JsonConvert.SerializeObject(colors, Formatting.Indented));
        }
    }

    public class DayRecord
    {
        public string Date { get; set; } = "";
        public double TotalHours { get; set; }
    }

    public class GradientColor
    {
        public string Name { get; set; } = "";
        public int StartR { get; set; }
        public int StartG { get; set; }
        public int StartB { get; set; }
        public int EndR { get; set; }
        public int EndG { get; set; }
        public int EndB { get; set; }

        [JsonIgnore]
        public Color StartColor => Color.FromArgb(Math.Clamp(StartR, 0, 255), Math.Clamp(StartG, 0, 255), Math.Clamp(StartB, 0, 255));

        [JsonIgnore]
        public Color EndColor => Color.FromArgb(Math.Clamp(EndR, 0, 255), Math.Clamp(EndG, 0, 255), Math.Clamp(EndB, 0, 255));

        [JsonIgnore]
        public string StartHex => $"#{StartR:X2}{StartG:X2}{StartB:X2}";

        [JsonIgnore]
        public string EndHex => $"#{EndR:X2}{EndG:X2}{EndB:X2}";
    }
}
