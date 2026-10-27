using UnityEngine;

namespace S16
{
    public class Mover : MonoBehaviour
    {
        void Update() { transform.position += Vector3.right; Trace.Log("Mover x=" + Trace.R(transform.position.x)); }
    }

    public class Follower : MonoBehaviour
    {
        public Transform target;
        void Update() { Trace.Log("Follower:Update sees x=" + Trace.R(target.position.x)); }
        void LateUpdate()
        {
            Trace.Log("Follower:LateUpdate sees x=" + Trace.R(target.position.x));
            transform.position = target.position + Vector3.back * 5;
        }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var m = new GameObject("M"); m.AddComponent<Mover>();
            var f = new GameObject("F").AddComponent<Follower>();
            f.target = m.transform;
            var f2 = new GameObject("F2").AddComponent<Follower>();
            f2.target = f.transform;
            Trace.Log("Run done");
        }
    }
}
