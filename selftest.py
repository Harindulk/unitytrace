#!/usr/bin/env python3
"""Grader sanity checks: gold answers must pass, damaged answers must fail, formatting noise must not matter."""
import json, random
from pathlib import Path
from score import extract, score

items = {i["id"]: i for i in map(json.loads, Path(__file__).with_name("data").joinpath("items.jsonl").read_text().splitlines())}
rows = []
for i in items.values():
    gold = "\n".join(i["answer"])
    rows.append({"id": i["id"], "response": f"Reasoning...\n```\n{gold}\n```\n"})          # perfect
good = score(items, rows)
assert all(s["exact"] for s in good), "gold answers must pass"
rng = random.Random(0)
bad_rows = []
for i in items.values():
    lines = list(i["answer"])
    k = rng.randrange(len(lines))
    mode = rng.choice(["drop", "swap", "edit"])
    if mode == "drop": del lines[k]
    elif mode == "swap" and len(lines) > 1: j = (k + 1) % len(lines); lines[k], lines[j] = lines[j], lines[k]
    else: lines[k] = lines[k] + "!"
    if lines == i["answer"]: lines.append("extra")
    bad_rows.append({"id": i["id"], "response": "```csharp\n" + "\n".join(lines) + "\n```"})
assert not any(s["exact"] for s in score(items, bad_rows)), "damaged answers must fail"
noisy = [{"id": i["id"], "response": "Here:\n```text\n  " + "\n\n  ".join(i["answer"]) + "  \n```\ndone"} for i in items.values()]
assert all(s["exact"] for s in score(items, noisy)), "whitespace noise must not matter"
assert not extract("no code block here"), "missing block means unparsed"
print(f"selftest ok: {len(items)} items")
