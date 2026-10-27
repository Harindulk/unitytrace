using UnityEngine;

namespace S07
{
    [DefaultExecutionOrder(10)]
    public class Late : MonoBehaviour
    {
        void Awake() { Trace.Log("Late:Awake"); }
        void Start() { Trace.Log("Late:Start"); }
        void Update() { Trace.Log("Late:Update"); }
    }

    [DefaultExecutionOrder(-10)]
    public class Early : MonoBehaviour
    {
        void Awake() { Trace.Log("Early:Awake"); }
        void Start() { Trace.Log("Early:Start"); }
        void Update() { Trace.Log("Early:Update"); }
    }

    public class Normal : MonoBehaviour
    {
        void Awake() { Trace.Log("Normal:Awake"); }
        void Start() { Trace.Log("Normal:Start"); }
        void Update() { Trace.Log("Normal:Update"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            var go = new GameObject("G");
            go.AddComponent<Late>();
            go.AddComponent<Normal>();
            go.AddComponent<Early>();
            Trace.Log("Run done");
        }
    }
}
