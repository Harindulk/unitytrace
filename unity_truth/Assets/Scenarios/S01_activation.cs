using UnityEngine;

namespace S01
{
    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var a = new GameObject("A");
            a.AddComponent<Probe>();

            var b = new GameObject("B");
            b.SetActive(false);
            b.AddComponent<Probe>();

            var c = new GameObject("C");
            c.transform.SetParent(b.transform);
            c.AddComponent<Probe>();

            Trace.Log("activating B");
            b.SetActive(true);
            Trace.Log("Run done");
        }
    }
}
