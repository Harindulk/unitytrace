using System.Collections.Generic;
using UnityEngine;

// Shared by every scenario. Trace.Log appends one line to the trace. The harness
// also writes "--- frame N ---" at the very start of every player-loop iteration.
public static class Trace
{
    public static readonly List<string> Lines = new List<string>();
    // Rounds to 2 decimals and prints without trailing zeros; negative zero prints as 0.
    public static string R(float x) => (Mathf.Round(x * 100f) / 100f + 0f).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
    public static string V(Vector3 v) => $"({R(v.x)}, {R(v.y)}, {R(v.z)})";
    public static void Log(string message) => Lines.Add(message);
}

// Logs every lifecycle callback it receives as "<gameObject.name>:<Callback>".
// Update/FixedUpdate/LateUpdate only log while logTicks is true.
public class Probe : MonoBehaviour
{
    public bool logTicks;
    void Awake() => Trace.Log($"{name}:Awake");
    void OnEnable() => Trace.Log($"{name}:OnEnable");
    void Start() => Trace.Log($"{name}:Start");
    void FixedUpdate() { if (logTicks) Trace.Log($"{name}:FixedUpdate"); }
    void Update() { if (logTicks) Trace.Log($"{name}:Update"); }
    void LateUpdate() { if (logTicks) Trace.Log($"{name}:LateUpdate"); }
    void OnDisable() => Trace.Log($"{name}:OnDisable");
    void OnDestroy() => Trace.Log($"{name}:OnDestroy");
}
