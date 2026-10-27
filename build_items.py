#!/usr/bin/env python3
"""Builds data/items.jsonl from unity_truth/Assets/Scenarios/*.cs and truth/*.txt (no Unity needed)."""
import json, re, sys
from pathlib import Path

ROOT = Path(__file__).parent
SC = ROOT / "unity_truth/Assets/Scenarios"
TASKS = json.loads((ROOT / "data/tasks.json").read_text())

HARNESS = """You are an expert Unity engineer. Below is a small Unity scenario. Predict, exactly, the trace it produces when it is run in real Unity.

How the scenario is executed (Unity 6000.3.2f1, Mono scripting backend, Editor play mode, empty scene, default project settings except where Run() changes them):
- Time.captureFramerate = 50, so every frame advances exactly 0.02 s of game time; Time.fixedDeltaTime = 0.02; Time.timeScale = 1; Physics.gravity is the default unless Run() changes it.
- Scenario.Run() is called once, at the very start of the first player-loop iteration, before any Unity callback of frame 1. It is not a MonoBehaviour. Anything it logs (including callbacks triggered synchronously from inside it, such as Awake and OnEnable of a freshly added component) comes first, before the `--- frame 1 ---` line.
- The harness itself writes the line `--- frame N ---` at the very start of every player-loop iteration (N = 1, 2, ...), before Unity runs any callback of that frame.
- The trace covers frames 1 to Scenario.Frames inclusive and then stops. Nothing runs after that.
- Every Trace.Log call appends one line. If Unity logs an unhandled exception, the harness appends `EXCEPTION: <ExceptionTypeName>` (type name only) at that point in the trace.
- Nothing else is logged. Not shown: the Unity version's own warnings. Assume no other scripts exist.

Answer format: think as long as you need, then give the complete trace in one fenced code block, one line per trace line, in order, including the `--- frame N ---` lines. Do not add commentary inside the block.
"""

def build():
    common = (SC / "Common.cs").read_text()
    items = []
    for f in sorted(SC.glob("S*.cs")):
        sid = f.name.split("_")[0]
        truth = (ROOT / "truth" / f"{sid}.txt").read_text().strip("\n")
        meta = TASKS[sid]
        prompt = (HARNESS + f"\n// ---- Common.cs ----\n{common}\n// ---- {f.name} ----\n{f.read_text()}")
        items.append({"id": sid, "scored": meta.get("scored", True), "task": meta["task"], "title": meta["title"], "prompt": prompt,
                      "answer": truth.split("\n")})
    return items

if __name__ == "__main__":
    items = build()
    dump = lambda xs: "".join(json.dumps(i, ensure_ascii=False) + "\n" for i in xs)
    (ROOT / "data/items.jsonl").write_text(dump(i for i in items if i["scored"]))
    # Truth differs between Unity 6000.3.2f1 and 6000.6.0f1, so "correct" is not stable: kept for reference, never scored.
    (ROOT / "data/version_sensitive.jsonl").write_text(dump(i for i in items if not i["scored"]))
    print(f"wrote {sum(i['scored'] for i in items)} scored items, {sum(not i['scored'] for i in items)} version-sensitive")
