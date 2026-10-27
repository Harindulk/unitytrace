using System;
using UnityEngine;
using UnityEngine.Events;

namespace S06
{
    public class Thrower : MonoBehaviour
    {
        void Awake() { Trace.Log("Thrower:Awake"); throw new InvalidOperationException("boom"); }
        void OnEnable() { Trace.Log("Thrower:OnEnable"); }
        void Start() { Trace.Log("Thrower:Start"); }
        void Update() { Trace.Log("Thrower:Update"); }
    }

    public class Bystander : MonoBehaviour
    {
        void Update() { Trace.Log("Bystander:Update"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            Action multicast = null;
            multicast += () => Trace.Log("handler 1");
            multicast += () => throw new Exception("x");
            multicast += () => Trace.Log("handler 3");
            try { multicast(); } catch (Exception e) { Trace.Log("caught " + e.Message); }

            var ev = new UnityEvent();
            ev.AddListener(() => Trace.Log("unity 1"));
            ev.AddListener(() => throw new Exception("y"));
            ev.AddListener(() => Trace.Log("unity 3"));
            try { ev.Invoke(); } catch (Exception e) { Trace.Log("caught " + e.Message); }

            var go = new GameObject("G");
            go.AddComponent<Bystander>();
            go.AddComponent<Thrower>();
            Trace.Log("Run done");
        }
    }
}
