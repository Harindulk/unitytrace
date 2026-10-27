using UnityEngine;

namespace S17
{
    public class B : MonoBehaviour
    {
        void Awake() { Trace.Log("B:Awake"); }
        void OnEnable() { Trace.Log("B:OnEnable"); }
        void Start() { Trace.Log("B:Start"); }
    }

    public class A : MonoBehaviour
    {
        void Awake()
        {
            Trace.Log("A:Awake begin");
            gameObject.AddComponent<B>();
            Trace.Log("A:Awake end");
        }
        void OnEnable() { Trace.Log("A:OnEnable"); }
        void Start() { Trace.Log("A:Start"); gameObject.AddComponent<Late>(); Trace.Log("A:Start end"); }
    }

    public class Late : MonoBehaviour
    {
        void Awake() { Trace.Log("Late:Awake"); }
        void OnEnable() { Trace.Log("Late:OnEnable"); }
        void Start() { Trace.Log("Late:Start"); }
        void Update() { Trace.Log("Late:Update"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            new GameObject("G").AddComponent<A>();
            Trace.Log("Run done");
        }
    }
}
