using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
	public float Seconds;
	public bool TimerStarted;
    private float TimeLeft;

	public void StartTimer()
	{
        TimeLeft = Seconds;
        TimerStarted = true;
	}

	public float GetTimeElapsed()
	{
        return Seconds - TimeLeft;
	}

	public float GetTimeLeft()
	{
        return TimeLeft;
	}

	public void StopTimer()
	{
        TimerStarted = false;
        TimeLeft = 0;
	}

	public void ResetTimer()
	{
        TimeLeft = Seconds;
	}

	public void Update()
	{
        if (TimerStarted && TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft < 0)
            {
                StopTimer();
            }
        }
	}

}

