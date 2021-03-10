using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FrameTimer
{
    public event TimerEventHandler onEnd;

    private bool isTicking = false;
    public bool isDone = true;
    [SerializeField] private int endTime;
    [SerializeField] private int time;

    public void StartTimer(int setTime)
    {
        endTime = setTime;
        isTicking = true;
        isDone = false;
        time = 0;
        //Debug.Log("timer started");
    }

    public bool TickTimer()
    {
        if (isTicking)
        {
            if (++time > endTime)
            {
                EndTimer();
            }
        }

        return isTicking;
    }

    public int ElapsedTime()
    {
        return time;
    }

    public bool IsTicking()
    {
        return isTicking;
    }

    public void ForceTimerEnd()
    {
        EndTimer();
    }

    private void EndTimer()
    {
        isTicking = false;
        isDone = true;
        endTime = 0;
        onEnd?.Invoke(this);
    }

    public delegate void TimerEventHandler(object sender);

}
