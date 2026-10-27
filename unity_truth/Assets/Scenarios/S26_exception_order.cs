using System;
using UnityEngine;

namespace S26
{
    public class A : MonoBehaviour
    {
        void Update() { Trace.Log("A:Update"); throw new InvalidOperationException("a"); }
        void LateUpdate() { Trace.Log("A:LateUpdate"); }
    }

    public class B : MonoBehaviour
    {
        void OnEnable() { Trace.Log("B:OnEnable"); throw new ArgumentException("b"); }
        void Start() { Trace.Log("B:Start"); }
        void Update() { Trace.Log("B:Update"); }
    }

    public class C : MonoBehaviour
    {
        void Awake() { Trace.Log("C:Awake"); }
        void Start() { Trace.Log("C:Start"); throw new NullReferenceException(); }
        void Update() { Trace.Log("C:Update"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            var g = new GameObject("G");
            g.AddComponent<A>();
            g.AddComponent<B>();
            g.AddComponent<C>();
            Trace.Log("Run done");
        }
    }
}
