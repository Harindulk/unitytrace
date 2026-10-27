using System;
using UnityEngine;

namespace S34
{
    public class Tag : MonoBehaviour { }

    public class Checker : MonoBehaviour
    {
        public GameObject victim;
        int n;

        void Report(string label)
        {
            bool goNull = victim == null;
            string comp;
            try { comp = victim.GetComponent<Tag>() == null ? "null" : "present"; }
            catch (Exception e) { comp = e.GetType().Name; }
            Trace.Log($"{label}: go==null {goNull}, GetComponent {comp}, Find {GameObject.Find("Victim") != null}, tagged {FindObjectsByType<Tag>(FindObjectsSortMode.None).Length}");
        }

        void Update()
        {
            n++;
            if (n == 1)
            {
                Report("before");
                Destroy(victim);
                Report("same frame");
            }
            else Report("frame " + n);
            if (n == 2) { try { Instantiate(victim); } catch (Exception e) { Trace.Log("Instantiate threw " + e.GetType().Name); } }
        }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var v = new GameObject("Victim");
            v.AddComponent<Tag>();
            new GameObject("Checker").AddComponent<Checker>().victim = v;
        }
    }
}
