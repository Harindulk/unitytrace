using System;
using System.Collections;
using UnityEngine;

namespace S19
{
    public class Co : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Bad());
            StartCoroutine(Good());
            StartCoroutine(Outer());
        }

        IEnumerator Bad()
        {
            Trace.Log("Bad 1");
            yield return null;
            Trace.Log("Bad 2");
            throw new InvalidOperationException("bad");
        }

        IEnumerator Good()
        {
            for (int i = 1; i <= 3; i++) { Trace.Log("Good " + i); yield return null; }
        }

        IEnumerator Outer()
        {
            Trace.Log("Outer begin");
            yield return StartCoroutine(Inner());
            Trace.Log("Outer end");
        }

        IEnumerator Inner()
        {
            Trace.Log("Inner begin");
            yield return null;
            yield return null;
            Trace.Log("Inner end");
        }
    }

    public static class Scenario
    {
        public const int Frames = 4;

        public static void Run()
        {
            new GameObject("C").AddComponent<Co>();
        }
    }
}
