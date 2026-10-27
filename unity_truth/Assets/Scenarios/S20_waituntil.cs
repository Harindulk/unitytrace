using System.Collections;
using UnityEngine;

namespace S20
{
    public class W : MonoBehaviour
    {
        int counter;
        IEnumerator Start()
        {
            StartCoroutine(Waiter());
            Trace.Log("Start: waiting for counter");
            yield return null;
            while (counter < 3) { Trace.Log("Start: yield " + counter); yield return null; }
            Trace.Log("Start done");
        }
        IEnumerator Waiter()
        {
            yield return new WaitUntil(() => counter >= 2);
            Trace.Log("WaitUntil released at counter=" + counter);
            yield return new WaitWhile(() => counter < 4);
            Trace.Log("WaitWhile released at counter=" + counter);
        }
        void Update() { counter++; Trace.Log("Update counter=" + counter); }
        void LateUpdate() { Trace.Log("LateUpdate counter=" + counter); }
    }

    public static class Scenario
    {
        public const int Frames = 5;

        public static void Run()
        {
            new GameObject("W").AddComponent<W>();
        }
    }
}
