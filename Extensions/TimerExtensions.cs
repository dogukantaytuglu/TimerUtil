using TimerUtil;

public static class TimerExtensions
{
    public static CountdownTimer DestroyOnComplete(this CountdownTimer countdownTimer)
    {
        countdownTimer.DestroyOnComplete = true;
        return countdownTimer;
    }
}
