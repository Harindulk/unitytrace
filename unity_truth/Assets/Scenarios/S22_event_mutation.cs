using System;
using UnityEngine;
using UnityEngine.Events;

namespace S22
{
    public static class Scenario
    {
        public const int Frames = 1;

        public static void Run()
        {
            Action a = null;
            Action second = () => Trace.Log("second");
            a += () => { Trace.Log("first"); a -= second; a += () => Trace.Log("added during"); };
            a += second;
            a();
            Trace.Log("--");
            a();

            var ev = new UnityEvent();
            UnityAction late = () => Trace.Log("unity late");
            UnityAction removeSelf = null;
            removeSelf = () => { Trace.Log("unity first"); ev.RemoveListener(removeSelf); ev.AddListener(late); };
            ev.AddListener(removeSelf);
            ev.AddListener(() => Trace.Log("unity second"));
            ev.Invoke();
            Trace.Log("--");
            ev.Invoke();
        }
    }
}
