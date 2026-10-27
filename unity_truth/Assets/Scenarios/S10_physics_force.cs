using UnityEngine;

namespace S10
{
    // Gravity is disabled in Run(). The harness fixes Time.fixedDeltaTime = 0.02 and one frame = 0.02 s.
    public class Pusher : MonoBehaviour
    {
        Rigidbody rb;
        void Awake() { rb = GetComponent<Rigidbody>(); }
        void Start()
        {
            rb.AddForce(Vector3.right * 100f);
            Trace.Log("Start v=" + Trace.V(rb.linearVelocity));
        }
        void FixedUpdate() { Trace.Log("FixedUpdate v=" + Trace.V(rb.linearVelocity) + " x=" + Trace.R(rb.position.x)); }
        void Update() { Trace.Log("Update v=" + Trace.V(rb.linearVelocity) + " tx=" + Trace.R(transform.position.x)); }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            Physics.gravity = Vector3.zero;
            var go = new GameObject("Ball");
            var rb = go.AddComponent<Rigidbody>();
            rb.mass = 2f;
            rb.linearDamping = 0f;
            go.AddComponent<Pusher>();
            Trace.Log("Run done");
        }
    }
}
