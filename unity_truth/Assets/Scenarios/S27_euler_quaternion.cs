using UnityEngine;

namespace S27
{
    public static class Scenario
    {
        public const int Frames = 1;

        public static void Run()
        {
            var t = new GameObject("T").transform;
            t.rotation = Quaternion.Euler(30, 60, 90);
            Trace.Log("euler " + Trace.V(t.eulerAngles));
            t.Rotate(0, 0, 90, Space.Self);
            Trace.Log("after self Z90 forward " + Trace.V(t.forward) + " right " + Trace.V(t.right));
            t.Rotate(90, 0, 0, Space.World);
            Trace.Log("after world X90 forward " + Trace.V(t.forward) + " up " + Trace.V(t.up));
            t.rotation = Quaternion.Euler(0, 0, 90) * Quaternion.Euler(90, 0, 0);
            Trace.Log("Z*X forward " + Trace.V(t.forward) + " up " + Trace.V(t.up));
            t.rotation = Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, 0, 90);
            Trace.Log("X*Z forward " + Trace.V(t.forward) + " up " + Trace.V(t.up));
            t.rotation = Quaternion.LookRotation(Vector3.right, Vector3.down);
            Trace.Log("look right/down euler " + Trace.V(t.eulerAngles));
            Trace.Log("angle " + Trace.R(Quaternion.Angle(Quaternion.identity, Quaternion.Euler(0, 270, 0))));
        }
    }
}
