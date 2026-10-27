using UnityEngine;

namespace S36
{
    public class SelfDestruct : MonoBehaviour
    {
        void Awake() { Trace.Log("Awake begin"); Destroy(gameObject); Trace.Log("Awake end"); }
        void OnEnable() { Trace.Log("OnEnable"); }
        void Start() { Trace.Log("Start"); }
        void Update() { Trace.Log("Update"); }
        void OnDisable() { Trace.Log("OnDisable"); }
        void OnDestroy() { Trace.Log("OnDestroy"); }
    }

    public class Other : MonoBehaviour
    {
        void Awake() { Trace.Log("Other:Awake"); }
        void Start() { Trace.Log("Other:Start"); }
        void Update() { Trace.Log("Other:Update"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            new GameObject("S").AddComponent<SelfDestruct>();
            Trace.Log("Run done");
            new GameObject("O").AddComponent<Other>();
        }
    }
}
