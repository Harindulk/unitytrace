using UnityEngine;

namespace S25
{
    public class Caster : MonoBehaviour
    {
        public Transform wall;
        int n;
        bool Cast() { return Physics.Raycast(Vector3.zero, Vector3.right, out var hit, 100f) && hit.collider != null; }
        void Update()
        {
            n++;
            if (n == 1)
            {
                Trace.Log("hit before move: " + Cast());
                wall.position = new Vector3(0, 10, 0);
                Trace.Log("hit after moving transform: " + Cast());
                Physics.SyncTransforms();
                Trace.Log("hit after SyncTransforms: " + Cast());
            }
        }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            Physics.autoSyncTransforms = false;
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = new Vector3(5, 0, 0);
            new GameObject("C").AddComponent<Caster>().wall = wall.transform;
        }
    }
}
