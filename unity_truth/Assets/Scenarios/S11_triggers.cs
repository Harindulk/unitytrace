using UnityEngine;

namespace S11
{
    public class Sensor : MonoBehaviour
    {
        void OnTriggerEnter(Collider other) { Trace.Log($"{name} trigger-enter {other.name}"); }
        void OnTriggerStay(Collider other) { Trace.Log($"{name} trigger-stay {other.name}"); }
        void OnTriggerExit(Collider other) { Trace.Log($"{name} trigger-exit {other.name}"); }
        void FixedUpdate() { Trace.Log($"{name} fixed"); }
    }

    public static class Scenario
    {
        public const int Frames = 5;

        static GameObject Box(string name, Vector3 pos, bool trigger, bool body, bool kinematic)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.GetComponent<Collider>().isTrigger = trigger;
            if (body) { var rb = go.AddComponent<Rigidbody>(); rb.useGravity = false; rb.isKinematic = kinematic; }
            go.AddComponent<Sensor>();
            return go;
        }

        public static void Run()
        {
            Physics.gravity = Vector3.zero;
            Box("Zone", Vector3.zero, true, false, false);          // static trigger, no Rigidbody
            Box("Mover", new Vector3(3, 0, 0), false, true, true);  // kinematic Rigidbody
            Box("Plain", new Vector3(0, 0, 3), false, false, false); // static collider, no Rigidbody
            var mover = GameObject.Find("Mover").transform;
            mover.gameObject.AddComponent<Slider>();
            Trace.Log("Run done");
        }
    }

    public class Slider : MonoBehaviour
    {
        void Update() { transform.position += new Vector3(-1.5f, 0, 0); Trace.Log("slider at x=" + Trace.R(transform.position.x)); }
    }
}
