using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace TimerUtil
{
    public static class TimerService
    {
        private static readonly List<Timer> Timers = new();
        private static TimersUpdater _timersUpdater;

        public static ReadOnlyCollection<Timer> AllTimers => Timers.AsReadOnly();

        public static CountdownTimer CreateCountdownTimer(float duration, int loopCount = 1)
            => CreateCountdownTimer(null, duration, loopCount);

        public static CountdownTimer CreateCountdownTimer(string id, float duration, int loopCount = 1)
        {
            return new CountdownTimer(id, duration, loopCount);
        }

        public static CountdownTimer CreateCountdownTimer(Vector2 randomDuration, int loopCount = 1)
            => CreateCountdownTimer(null, randomDuration, loopCount);

        public static CountdownTimer CreateCountdownTimer(string id, Vector2 randomDuration, int loopCount = 1)
        {
            return new CountdownTimer(id, randomDuration, loopCount);
        }

        public static StopwatchTimer CreateStopwatch(string id = null)
        {
            return new StopwatchTimer(id);
        }

        public static void RegisterTimer(Timer timer)
        {
            if (Timers.Contains(timer))
                return;

            var isValidId = !string.IsNullOrEmpty(timer.Id);
            if (isValidId && Timers.Exists(t => t.Id == timer.Id))
            {
                UnregisterTimer(timer.Id);
            }

            TryCreateTimersUpdater();

            Timers.Add(timer);
        }

        public static Timer FindTimerById(string id)
        {
            return Timers.Find(t => t.Id == id);
        }

        public static bool UnregisterTimer(string id)
        {
            var timer = FindTimerById(id);
            if (timer == null)
                return false;

            return UnregisterTimer(timer);
        }

        public static bool UnregisterTimer(Timer timer)
        {
            var success = Timers.Remove(timer);
            if (success && Timers.Count == 0)
                DestroyTimersUpdater();

            return success;
        }

        private static void TryCreateTimersUpdater()
        {
            if (_timersUpdater)
                return;

            var timersGameObject = CreateTimersUpdater();

            GameObject.DontDestroyOnLoad(timersGameObject);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        static GameObject CreateTimersUpdater()
        {
            var timersGameObject = new GameObject("Timers Updater");
            _timersUpdater = timersGameObject.AddComponent<TimersUpdater>();
            return timersGameObject;
        }

        private static void DestroyTimersUpdater()
        {
            if (!_timersUpdater) return;
            GameObject.Destroy(_timersUpdater.gameObject);
            _timersUpdater = null;
        }
    }
}
