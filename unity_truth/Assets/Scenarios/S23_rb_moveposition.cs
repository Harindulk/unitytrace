using UnityEngine;

namespace S23
{
    public class Mover : MonoBehaviour
    {
        Rigidbody rb;
        int n;
        void Awake() { rb = GetComponent<Rigidbody>(); }
        void FixedUpdate()
        {
            n++;
            if (n == 1) rb.MovePosition(rb.position + Vector3.right);
            if (n == 2) transform.position += Vector3.up * 5;
            Trace.Log($"Fixed {n}: rb.x={Trace.R(rb.position.x)} rb.y={Trace.R(rb.position.y)} tr.x={Trace.R(transform.position.x)} tr.y={Trace.R(transform.position.y)}");
        }
        void Update() { Trace.Log($"Update: rb.x={Trace.R(rb.position.x)} rb.y={Trace.R(rb.position.y)} tr.x={Trace.R(transform.position.x)} tr.y={Trace.R(transform.position.y)}"); }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            Physics.gravity = Vector3.zero;
            var go = new GameObject("Kin");
            var rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            go.AddComponent<Mover>();
        }
    }
}
