using UnityEngine;

namespace S18
{
    public class Toggler : MonoBehaviour
    {
        void Awake() { Trace.Log("Awake"); enabled = false; Trace.Log("Awake end"); }
        void OnEnable() { Trace.Log("OnEnable"); }
        void Start() { Trace.Log("Start"); }
        void Update() { Trace.Log("Update"); }
        void OnDisable() { Trace.Log("OnDisable"); }
    }

    public class Switch : MonoBehaviour
    {
        public Toggler t;
        int n;
        void Update()
        {
            n++;
            if (n == 2) { t.enabled = true; t.enabled = false; t.enabled = true; Trace.Log("flipped"); }
            if (n == 3) { t.enabled = false; Trace.Log("off"); }
        }
    }

    public static class Scenario
    {
        public const int Frames = 4;

        public static void Run()
        {
            var g = new GameObject("G");
            var t = g.AddComponent<Toggler>();
            var s = new GameObject("S").AddComponent<Switch>();
            s.t = t;
            Trace.Log("Run done");
        }
    }
}
