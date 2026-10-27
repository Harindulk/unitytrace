using System;
using UnityEngine;

namespace S12
{
    public class Holder : MonoBehaviour
    {
        public Transform target;
        int n;
        void Update()
        {
            n++;
            if (n == 1) Destroy(target.gameObject);
            Trace.Log("== null: " + (target == null) + ", is null: " + (target is null));
            try { Trace.Log("coalesce: " + (target?.name ?? "none")); } catch (Exception e) { Trace.Log("threw " + e.GetType().Name); }
            try { Trace.Log("coalesce-assign ok: " + ((object)(target ?? transform)).GetType().Name); } catch (Exception e) { Trace.Log("threw " + e.GetType().Name); }
        }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            var t = new GameObject("Target");
            new GameObject("H").AddComponent<Holder>().target = t.transform;
        }
    }
}
