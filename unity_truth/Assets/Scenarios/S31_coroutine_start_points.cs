using System.Collections;
using UnityEngine;

namespace S31
{
    public class Starter : MonoBehaviour
    {
        int n;
        bool fixedDone;

        IEnumerator C(string id)
        {
            Trace.Log(id + "1");
            yield return null;
            Trace.Log(id + "2");
            yield return new WaitForFixedUpdate();
            Trace.Log(id + "3");
        }

        void FixedUpdate()
        {
            Trace.Log("FixedUpdate");
            if (!fixedDone) { fixedDone = true; StartCoroutine(C("F")); }
        }

        void Update()
        {
            n++;
            Trace.Log("Update " + n);
            if (n == 1) StartCoroutine(C("U"));
        }

        void LateUpdate()
        {
            Trace.Log("LateUpdate");
            if (n == 1) StartCoroutine(C("L"));
        }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            new GameObject("S").AddComponent<Starter>();
        }
    }
}
