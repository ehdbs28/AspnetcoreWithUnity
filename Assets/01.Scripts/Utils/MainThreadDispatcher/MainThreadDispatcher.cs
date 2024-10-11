using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
{
    private readonly Queue<Action> _jobs = new Queue<Action>();

    private void Awake()
    {
        var temp = Instance;
    }

    private void Update()
    {
        while (_jobs.Count > 0)
        {
            _jobs.Dequeue()?.Invoke();            
        }
    }

    public void Enqueue(Action job)
    {
        _jobs.Enqueue(job);
    }
}