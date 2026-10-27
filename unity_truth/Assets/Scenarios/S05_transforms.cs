using UnityEngine;

namespace S05
{
    public static class Scenario
    {
        public const int Frames = 1;

        public static void Run()
        {
            var p = new GameObject("P").transform;
            p.position = new Vector3(1, 2, 3);
            p.rotation = Quaternion.Euler(0, 90, 0);
            p.localScale = new Vector3(2, 1, 1);

            var c = new GameObject("C").transform;
            c.SetParent(p, false);
            c.localPosition = new Vector3(1, 0, 0);
            Trace.Log("c.position " + Trace.V(c.position));
            Trace.Log("c.lossyScale " + Trace.V(c.lossyScale));

            c.Rotate(0, 0, 90, Space.World);
            Trace.Log("c.forward " + Trace.V(c.forward));
            Trace.Log("c.up " + Trace.V(c.up));
            Trace.Log("c.localEulerAngles " + Trace.V(c.localEulerAngles));

            p.localScale = Vector3.one;
            Trace.Log("c.position " + Trace.V(c.position));
            Trace.Log("c.TransformPoint(0,1,0) " + Trace.V(c.TransformPoint(0, 1, 0)));
        }
    }
}
