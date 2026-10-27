using System.Collections;
using UnityEngine;

namespace S33
{
    public class F : MonoBehaviour
    {
        Coroutine stopMe;

        IEnumerator Body(string id, int steps)
        {
            try
            {
                for (int i = 1; i <= steps; i++) { Trace.Log($"{id} step {i}"); yield return null; }
                Trace.Log(id + " finished loop");
            }
            finally
            {
                Trace.Log(id + " finally");
            }
        }

        IEnumerator Start()
        {
            stopMe = StartCoroutine(Body("stopped", 9));
            StartCoroutine(Body("completes", 1));
            StartCoroutine(Body("deactivated", 9));
            yield return null;
            Trace.Log("-- StopCoroutine");
            StopCoroutine(stopMe);
            Trace.Log("-- after StopCoroutine");
            yield return null;
            Trace.Log("-- SetActive(false)");
            gameObject.SetActive(false);
            Trace.Log("-- after SetActive(false)");
        }
    }

    public static class Scenario
    {
        public const int Frames = 4;

        public static void Run()
        {
            new GameObject("F").AddComponent<F>();
        }
    }
}
