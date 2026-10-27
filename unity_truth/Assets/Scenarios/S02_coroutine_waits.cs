using System.Collections;
using UnityEngine;

namespace S02
{
    public class Worker : MonoBehaviour
    {
        void Awake() { Trace.Log("Awake"); StartCoroutine(Routine()); Trace.Log("Awake end"); }
        void Update() { Trace.Log("Update"); }
        void FixedUpdate() { Trace.Log("FixedUpdate"); }
        void LateUpdate() { Trace.Log("LateUpdate"); }

        IEnumerator Inner()
        {
            Trace.Log("I1");
            yield return null;
            Trace.Log("I2");
        }

        IEnumerator Routine()
        {
            Trace.Log("R1");
            yield return null;
            Trace.Log("R2");
            yield return new WaitForFixedUpdate();
            Trace.Log("R3");
            yield return new WaitForSeconds(0.05f);
            Trace.Log("R4");
            yield return StartCoroutine(Inner());
            Trace.Log("R5");
        }
    }

    public static class Scenario
    {
        public const int Frames = 8;

        public static void Run()
        {
            new GameObject("W").AddComponent<Worker>();
            Trace.Log("Run done");
        }
    }
}
