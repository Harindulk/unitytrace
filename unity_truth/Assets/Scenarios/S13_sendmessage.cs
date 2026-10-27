using UnityEngine;

namespace S13
{
    public class Receiver : MonoBehaviour
    {
        void Ping(int n) { Trace.Log($"{name}:Ping({n})"); }
        void Pong() { Trace.Log($"{name}:Pong"); }
        void OnDisable() { Trace.Log($"{name}:OnDisable"); }
    }

    public static class Scenario
    {
        public const int Frames = 2;

        public static void Run()
        {
            var root = new GameObject("Root"); root.AddComponent<Receiver>();
            var mid = new GameObject("Mid"); mid.transform.SetParent(root.transform); mid.AddComponent<Receiver>();
            var leaf = new GameObject("Leaf"); leaf.transform.SetParent(mid.transform); leaf.AddComponent<Receiver>();
            leaf.SetActive(false);
            var off = new GameObject("Off"); off.transform.SetParent(root.transform); off.AddComponent<Receiver>();
            off.GetComponent<Receiver>().enabled = false;

            mid.SendMessage("Ping", 1);
            root.BroadcastMessage("Ping", 2);
            leaf.SendMessageUpwards("Pong");
            root.BroadcastMessage("Missing", SendMessageOptions.DontRequireReceiver);
            root.SetActive(false);
            Trace.Log("Run done");
        }
    }
}
