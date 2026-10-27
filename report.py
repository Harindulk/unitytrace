#!/usr/bin/env python3
"""Writes results/SUMMARY.md: per-model strict score with bootstrap CI, per-task scores, per-item pass counts."""
import json, random, statistics
from pathlib import Path
from score import score

ROOT = Path(__file__).parent
ORDER = ["claude-haiku-5-5", "claude-sonnet-5-5", "claude-opus-4-8", "claude-opus-5-5", "claude-fable-5-1"]

def main():
    items = {i["id"]: i for i in map(json.loads, (ROOT / "data/items.jsonl").read_text().splitlines())}
    per = {}
    for f in sorted((ROOT / "results").glob("*.jsonl")):
        rows = [json.loads(l) for l in f.read_text().splitlines() if l.strip()]
        per[f.stem] = score(items, rows)
    models = [m for m in ORDER if m in per] + [m for m in per if m not in ORDER]
    rng = random.Random(1)
    out = ["# Results\n", "Strict = every trace line correct and in order. 95% CI: bootstrap over the items (the 3 samples of an item stay together).\n",
           "| model | strict | 95% CI | pass@3 | all 3 | line similarity |", "|---|---|---|---|---|---|"]
    ids = sorted(items)
    for m in models:
        by = {i: [s for s in per[m] if s["id"] == i] for i in ids}
        rate = {i: statistics.mean(s["exact"] for s in by[i]) for i in ids}
        mean = statistics.mean(rate.values())
        boots = sorted(statistics.mean(rng.choice(list(rate.values())) for _ in ids) for _ in range(4000))
        p3 = statistics.mean(any(s["exact"] for s in by[i]) for i in ids)
        a3 = statistics.mean(all(s["exact"] for s in by[i]) for i in ids)
        sim = statistics.mean(s["partial"] for s in per[m])
        out.append(f"| {m} | {mean:.0%} | {boots[100]:.0%} to {boots[3899]:.0%} | {p3:.0%} | {a3:.0%} | {sim:.0%} |")
    tasks = sorted({items[i]["task"] for i in ids})
    out += ["\n## By task (strict)\n", "| task | items | " + " | ".join(models) + " |", "|---|---|" + "---|" * len(models)]
    for t in tasks:
        tid = [i for i in ids if items[i]["task"] == t]
        cells = [f"{statistics.mean(s['exact'] for s in per[m] if s['id'] in tid):.0%}" for m in models]
        out.append(f"| {t} | {len(tid)} | " + " | ".join(cells) + " |")
    out += ["\n## By item (samples passed out of 3)\n", "| item | task | what it tests | " + " | ".join(models) + " |", "|---|---|---|" + "---|" * len(models)]
    for i in ids:
        cells = [str(sum(s["exact"] for s in per[m] if s["id"] == i)) for m in models]
        out.append(f"| {i} | {items[i]['task'].split()[0]} | {items[i]['title']} | " + " | ".join(cells) + " |")
    none = [i for i in ids if all(not s["exact"] for m in models for s in per[m] if s["id"] == i)]
    out.append(f"\nItems no model passed in any sample: {', '.join(none) or 'none'}")
    (ROOT / "results/SUMMARY.md").write_text("\n".join(out) + "\n")
    print("\n".join(out))

if __name__ == "__main__":
    main()
