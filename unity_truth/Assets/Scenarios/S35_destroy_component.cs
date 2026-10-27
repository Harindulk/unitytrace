using UnityEngine;

namespace S35
{
    public class Victim : MonoBehaviour
    {
        void Update() { Trace.Log("Victim:Update"); }
        void OnDisable() { Trace.Log("Victim:OnDisable"); }
        void OnDestroy() { Trace.Log("Victim:OnDestroy"); }
    }

    public class Suicidal : MonoBehaviour
    {
        void Update()
        {
            Trace.Log("Suicidal:Update begin");
            Destroy(this);
            Trace.Log($"Suicidal:Update end, enabled={enabled}, this==null:{this == null}");
        }
        void OnDestroy() { Trace.Log("Suicidal:OnDestroy"); }
    }

    public class Killer : MonoBehaviour
    {
        public Victim victim;
        int n;
        void Update()
        {
            n++;
            if (n == 1)
            {
                Destroy(victim);
                Trace.Log($"Killer: requested, victim.enabled={victim.enabled}, victim==null:{victim == null}");
            }
            else Trace.Log($"Killer: frame {n}, victim==null:{victim == null}");
        }
    }

    public static class Scenario
    {
        public const int Frames = 3;

        public static void Run()
        {
            var v = new GameObject("V").AddComponent<Victim>();
            new GameObject("S").AddComponent<Suicidal>();
            new GameObject("K").AddComponent<Killer>().victim = v;
        }
    }
}
