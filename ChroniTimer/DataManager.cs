using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ChroniTimer
{
    /// <summary>
    /// Manages persistence for timer session data.
    /// Stores today's accumulated hours and full history in JSON format.
    /// </summary>
    public class DataManager
    {
        private readonly string _dataFolder;
        private readonly string _todayFile;
        private readonly string _historyFile;
        private readonly string _colorsFile;

        public DataManager()
        {
            _dataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ChroniTimer");
            Directory.CreateDirectory(_dataFolder);

            _todayFile = Path.Combine(_dataFolder, "today.json");
            _historyFile = Path.Combine(_dataFolder, "history.json");
            _colorsFile = Path.Combine(_dataFolder, "saved_colors.json");
        }

        public string DataFolder => _dataFolder;

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
    }
}
