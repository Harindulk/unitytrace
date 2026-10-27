using UnityEngine;

namespace S28
{
    public class R : MonoBehaviour
    {
        void Hit() { Trace.Log(name + ":Hit"); }
        void Awake() { Trace.Log(name + ":Awake"); }
        void OnEnable() { Trace.Log(name + ":OnEnable"); }
    }

    public static class Scenario
    {
        public const int Frames = 1;

        public static void Run()
        {
            var root = new GameObject("Root"); root.AddComponent<R>();
            var off = new GameObject("Off"); off.SetActive(false); off.transform.SetParent(root.transform); off.AddComponent<R>();
            var under = new GameObject("UnderOff"); under.transform.SetParent(off.transform); under.AddComponent<R>();
            var on = new GameObject("On"); on.transform.SetParent(root.transform); on.AddComponent<R>();

            Trace.Log("-- broadcast");
            root.BroadcastMessage("Hit");
            Trace.Log("-- broadcast includeInactive via SendMessageOptions");
            root.BroadcastMessage("Hit", SendMessageOptions.RequireReceiver);
            Trace.Log("-- children (inactive included)");
            foreach (var r in root.GetComponentsInChildren<R>(true)) r.SendMessage("Hit");
            Trace.Log("-- children (default)");
            foreach (var r in root.GetComponentsInChildren<R>()) r.SendMessage("Hit");
            Trace.Log("-- find");
            Trace.Log("count " + Object.FindObjectsByType<R>(FindObjectsSortMode.None).Length);
            Trace.Log("count incl inactive " + Object.FindObjectsByType<R>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length);
        }
    }
}
