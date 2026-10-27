#!/usr/bin/env python3
"""Scores results/*.jsonl against data/items.jsonl. Strict metric: the whole trace must match, line for line."""
import json, re, sys
from difflib import SequenceMatcher
from pathlib import Path
from collections import defaultdict

ROOT = Path(__file__).parent

def extract(response):
    blocks = re.findall(r"```[^\n]*\n(.*?)```", response, re.S)
    text = blocks[-1] if blocks else ""
    return [l.strip() for l in text.splitlines() if l.strip()]

def score(items, rows):
    out = []
    for r in rows:
        if r["id"] not in items: continue  # e.g. version-sensitive items are not scored
        gold = [l.strip() for l in items[r["id"]]["answer"] if l.strip()]
        got = extract(r["response"])
        exact = got == gold
        partial = SequenceMatcher(None, got, gold, autojunk=False).ratio() if got else 0.0
        out.append({"id": r["id"], "task": items[r["id"]]["task"], "exact": exact, "partial": partial, "formatted": bool(got)})
    return out

def main():
    items = {i["id"]: i for i in map(json.loads, (ROOT / "data/items.jsonl").read_text().splitlines())}
    files = sorted((ROOT / "results").glob("*.jsonl"))
    if len(sys.argv) > 1: files = [Path(p) for p in sys.argv[1:]]
    table = {}
    for f in files:
        rows = [json.loads(l) for l in f.read_text().splitlines() if l.strip()]
        sc = score(items, rows)
        n = len(sc)
        by = defaultdict(list)
        for s in sc: by[s["task"]].append(s["exact"])
        table[f.stem] = {"n": n, "exact": sum(s["exact"] for s in sc) / n, "partial": sum(s["partial"] for s in sc) / n,
                         "unformatted": sum(not s["formatted"] for s in sc), "by_task": {t: sum(v) / len(v) for t, v in by.items()},
                         "per_item": {i: sum(s["exact"] for s in sc if s["id"] == i) / max(1, sum(1 for s in sc if s["id"] == i)) for i in items}}
    for m, t in table.items():
        print(f"{m:28s} n={t['n']:3d} strict={t['exact']:.0%} line-similarity={t['partial']:.0%} unparsed={t['unformatted']}")
        print("   " + "  ".join(f"{k}:{v:.0%}" for k, v in sorted(t["by_task"].items())))
    if "--json" in sys.argv: pass
    return table

if __name__ == "__main__":
    main()
