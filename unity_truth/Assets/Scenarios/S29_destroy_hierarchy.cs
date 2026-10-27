using UnityEngine;

namespace S29
{
    public class Reaper : MonoBehaviour
    {
        public GameObject root, a, b, a1;
        int n;
        void Update()
        {
            n++;
            if (n != 1) return;
            a1.transform.SetParent(null);
            Trace.Log("detached A1");
            Destroy(a);
            Destroy(root);
            Destroy(root);
            Trace.Log($"count now {FindObjectsByType<Probe>(FindObjectsSortMode.None).Length}");
            Trace.Log($"B inHierarchy {b.activeInHierarchy}");
        }
        void LateUpdate()
        {
            if (n == 1) Trace.Log($"late: count {FindObjectsByType<Probe>(FindObjectsSortMode.None).Length}");
        }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        static GameObject Make(string name, Transform parent)
        {
            var g = new GameObject(name);
            g.transform.SetParent(parent);
            g.AddComponent<Probe>();
            return g;
        }

        public static void Run()
        {
            var root = Make("Root", null);
            var a = Make("A", root.transform);
            var b = Make("B", root.transform);
            var a1 = Make("A1", a.transform);
            var r = new GameObject("Reaper").AddComponent<Reaper>();
            r.root = root; r.a = a; r.b = b; r.a1 = a1;
        }
    }
}
