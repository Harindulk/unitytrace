using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.TestTools;

// Runs every `<Namespace>.Scenario` class (public const int Frames, public static void Run())
// in play mode and writes the recorded Trace lines to <repo>/truth/<Namespace>.txt.
public class Capture
{
    static string[] Ids() => AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => { try { return a.GetTypes(); } catch { return new Type[0]; } })
        .Where(t => t.Name == "Scenario" && t.Namespace != null && t.Namespace.StartsWith("S"))
        .Select(t => t.Namespace).OrderBy(n => n).ToArray();

    [UnityTest]
    public IEnumerator Run([ValueSource(nameof(Ids))] string id)
    {
        LogAssert.ignoreFailingMessages = true;
        var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => { try { return a.GetTypes(); } catch { return new Type[0]; } })
            .First(t => t.Name == "Scenario" && t.Namespace == id);
        int frames = (int)type.GetField("Frames").GetRawConstantValue();
        var lines = Trace.Lines;
        lines.Clear();
        Application.logMessageReceived += OnLog;
        Time.captureFramerate = 50;
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1f;
        yield return null;
        int frame = 0;
        var loop = PlayerLoop.GetCurrentPlayerLoop();
        var marker = new PlayerLoopSystem { type = typeof(Capture), updateDelegate = () =>
        {
            // Run() executes at the very start of the first player-loop iteration, like scene-load code in a real game.
            if (frame == 0) type.GetMethod("Run").Invoke(null, null);
            lines.Add($"--- frame {++frame} ---");
        } };
        var list = new System.Collections.Generic.List<PlayerLoopSystem>(loop.subSystemList);
        list.Insert(0, marker);
        loop.subSystemList = list.ToArray();
        PlayerLoop.SetPlayerLoop(loop);
        for (int i = 0; i < frames + 1; i++) yield return null;
        PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
        Application.logMessageReceived -= OnLog;
        var recorded = lines.TakeWhile(l => l != $"--- frame {frames + 1} ---").ToArray();
        Time.captureFramerate = 0;
        Time.timeScale = 1f;
        foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)) UnityEngine.Object.Destroy(go);
        yield return null;
        var root = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "truth"));
        Directory.CreateDirectory(root);
        File.WriteAllText(Path.Combine(root, id + ".txt"), string.Join("\n", recorded) + "\n", new UTF8Encoding(false));
    }

    static void OnLog(string condition, string stack, LogType type)
    {
        if (type == LogType.Exception) Trace.Log("EXCEPTION: " + condition.Split(':')[0].Trim());
    }
}
