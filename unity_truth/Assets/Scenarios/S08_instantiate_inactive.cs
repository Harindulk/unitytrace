using UnityEngine;

namespace S08
{
    public class Spawner : MonoBehaviour
    {
        public GameObject template;
        void Start()
        {
            Trace.Log("Spawner:Start");
            var clone = Instantiate(template);
            Trace.Log("instantiated, clone.activeSelf=" + clone.activeSelf);
            clone.name = "Clone";
            clone.SetActive(true);
            Trace.Log("clone activated");
        }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var template = new GameObject("Template");
            template.SetActive(false);
            template.AddComponent<Probe>();
            Trace.Log("template ready");

            var s = new GameObject("Spawner");
            s.AddComponent<Spawner>().template = template;
            Trace.Log("Run done");
        }
    }
}
