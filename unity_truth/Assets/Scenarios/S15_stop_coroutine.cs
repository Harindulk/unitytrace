using System.Collections;
using UnityEngine;

namespace S15
{
    public class Runner : MonoBehaviour
    {
        Coroutine a;
        void Start()
        {
            a = StartCoroutine(Count("A"));
            StartCoroutine(Count("B"));
            StartCoroutine(Killer());
        }
        IEnumerator Killer()
        {
            yield return null;
            Trace.Log("killing A");
            StopCoroutine(a);
            Trace.Log("disabling component");
            enabled = false;
            yield return null;
            Trace.Log("killer still alive");
            gameObject.SetActive(false);
            Trace.Log("after SetActive(false)");
        }
        IEnumerator Count(string id)
        {
            for (int i = 1; i <= 5; i++) { Trace.Log($"{id}{i}"); yield return null; }
        }
    }

    public static class Scenario
    {
        public const int Frames = 6;

        public static void Run()
        {
            new GameObject("R").AddComponent<Runner>();
            Trace.Log("Run done");
        }
    }
}
