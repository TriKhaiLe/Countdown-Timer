using System;

namespace ChroniTimer
{
    /// <summary>
    /// Core count-UP timer engine. Tracks elapsed time in hours
    /// and records accumulated totals to the data manager.
    /// </summary>
    public class TimerEngine
    {
        private readonly DataManager _dataManager;
        private DateTime _sessionStart;
        private double _targetHours;
        private DayRecord _todayRecord;

        public bool IsRunning { get; private set; }
        public TimeSpan Elapsed => IsRunning ? DateTime.Now - _sessionStart : TimeSpan.Zero;
        public double TargetHours => _targetHours;
        public DayRecord TodayRecord => _todayRecord;
        public double LastSessionHours { get; private set; }

        public TimerEngine(DataManager dataManager)
        {
            _dataManager = dataManager;
            _todayRecord = _dataManager.LoadToday();
        }

        /// <summary>
        /// Starts a count-up timer session targeting the specified hours.
        /// </summary>
        public void Start(double targetHours)
        {
            _targetHours = targetHours;
            _sessionStart = DateTime.Now;
            IsRunning = true;
            LastSessionHours = 0;
        }

        /// <summary>
        /// Called each tick to check if target has been reached.
        /// Returns true when the target time is exceeded.
        /// </summary>
        public bool Tick()
        {
            if (!IsRunning) return false;
            return Elapsed.TotalHours >= _targetHours;
        }

        /// <summary>
        /// Stops the timer and records the elapsed time.
        /// </summary>
        public double Stop()
        {
            if (!IsRunning) return 0;
            IsRunning = false;
            double hoursElapsed = Elapsed.TotalHours;
            LastSessionHours = hoursElapsed;

            // Add to today's total
            _todayRecord.TotalHours += hoursElapsed;
            _dataManager.SaveToday(_todayRecord);

            return hoursElapsed;
        }

        /// <summary>
        /// Adds externally tracked time (in hours) to today's total.
        /// </summary>
        public void AddExternalTime(double hours)
        {
            LastSessionHours = hours;
            _todayRecord.TotalHours += hours;
            _dataManager.SaveToday(_todayRecord);
        }

        /// <summary>
        /// Reload today's data (for day rollover detection).
        /// </summary>
        public void RefreshTodayData()
        {
            _todayRecord = _dataManager.LoadToday();
        }

        /// <summary>
        /// Persists the current day record to history and reloads.
        /// </summary>
        public void ArchiveToday()
        {
            if (_todayRecord.TotalHours > 0)
            {
                _dataManager.AppendToHistory(_todayRecord);
            }
        }
    }
}
