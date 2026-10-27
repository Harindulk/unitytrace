using UnityEngine;

namespace S03
{
    public class Killer : MonoBehaviour
    {
        public GameObject parent;
        void Update()
        {
            Trace.Log("Killer:Update");
            Destroy(parent);
            Trace.Log($"parent==null:{parent == null} activeSelf:{parent.activeSelf} childCount:{parent.transform.childCount}");
            Trace.Log("components after Destroy: " + parent.GetComponents<Probe>().Length);
            Destroy(this);
            Trace.Log("Killer:Update end");
        }
        void OnDestroy() { Trace.Log("Killer:OnDestroy"); }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var parent = new GameObject("Parent");
            parent.AddComponent<Probe>();
            var child = new GameObject("Child");
            child.transform.SetParent(parent.transform);
            child.AddComponent<Probe>();
            new GameObject("K").AddComponent<Killer>().parent = parent;
        }
    }
}
