#!/usr/bin/env python3
"""Runs the eval for one model and writes results/<label>.jsonl.

Backends (standard library only):
  claude-cli  the logged-in `claude` CLI, isolated: no tools, no settings, no project context
  anthropic   Anthropic Messages API (ANTHROPIC_API_KEY)
  openai      any OpenAI-compatible chat endpoint, e.g. OpenRouter (OPENAI_API_KEY, OPENAI_BASE_URL)

  python3 run_eval.py --backend claude-cli --model claude-opus-5-5 --samples 3
"""
import argparse, json, os, re, subprocess, sys, tempfile, time, urllib.request, concurrent.futures as cf
from pathlib import Path

ROOT = Path(__file__).parent
SYSTEM = "You are a careful expert. Follow the answer format exactly."

def load_env():
    env = ROOT / ".env"
    if env.exists():
        for line in env.read_text().splitlines():
            if "=" in line and not line.lstrip().startswith("#"):
                k, v = line.split("=", 1)
                os.environ.setdefault(k.strip(), v.strip().strip('"'))

def call_cli(model, prompt):
    with tempfile.TemporaryDirectory() as cwd:
        r = subprocess.run(["claude", "-p", prompt, "--model", model, "--setting-sources", "", "--tools", "",
                            "--disable-slash-commands", "--strict-mcp-config", "--no-session-persistence",
                            "--system-prompt", SYSTEM, "--output-format", "json"],
                           capture_output=True, text=True, cwd=cwd, timeout=1500)
    d = json.loads(r.stdout)
    return d["result"], d.get("total_cost_usd", 0.0)

def post(url, headers, body):
    req = urllib.request.Request(url, json.dumps(body).encode(), {"content-type": "application/json", **headers})
    with urllib.request.urlopen(req, timeout=1500) as r:
        return json.load(r)

def call_anthropic(model, prompt):
    d = post("https://api.anthropic.com/v1/messages",
             {"x-api-key": os.environ["ANTHROPIC_API_KEY"], "anthropic-version": "2023-06-01"},
             {"model": model, "max_tokens": 32000, "system": SYSTEM, "thinking": {"type": "adaptive"},
              "output_config": {"effort": os.environ.get("EFFORT", "high")},
              "messages": [{"role": "user", "content": prompt}]})
    return "".join(b.get("text", "") for b in d["content"]), 0.0

def call_openai(model, prompt):
    base = os.environ.get("OPENAI_BASE_URL", "https://api.openai.com/v1").rstrip("/")
    d = post(f"{base}/chat/completions", {"authorization": "Bearer " + os.environ["OPENAI_API_KEY"]},
             {"model": model, "messages": [{"role": "system", "content": SYSTEM}, {"role": "user", "content": prompt}]})
    return d["choices"][0]["message"]["content"], 0.0

BACKENDS = {"claude-cli": call_cli, "anthropic": call_anthropic, "openai": call_openai}

def one(backend, model, item, sample):
    for attempt in range(3):
        try:
            t = time.time()
            text, cost = BACKENDS[backend](model, item["prompt"])
            return {"id": item["id"], "sample": sample, "model": model, "backend": backend,
                    "response": text, "cost_usd": cost, "seconds": round(time.time() - t, 1)}
        except Exception as e:
            err = repr(e)
            time.sleep(5 * (attempt + 1))
    return {"id": item["id"], "sample": sample, "model": model, "backend": backend, "response": "", "error": err}

def main():
    load_env()
    ap = argparse.ArgumentParser()
    ap.add_argument("--backend", required=True, choices=BACKENDS)
    ap.add_argument("--model", required=True)
    ap.add_argument("--label", help="results file name (default: model id)")
    ap.add_argument("--samples", type=int, default=1)
    ap.add_argument("--only", help="comma-separated item ids")
    ap.add_argument("--workers", type=int, default=4)
    ap.add_argument("--append", action="store_true", help="add to an existing results file instead of replacing it")
    a = ap.parse_args()
    items = [json.loads(l) for l in (ROOT / "data/items.jsonl").read_text().splitlines()]
    if a.only: items = [i for i in items if i["id"] in a.only.split(",")]
    out = ROOT / "results" / f"{a.label or a.model}.jsonl"
    jobs = [(i, s) for s in range(a.samples) for i in items]
    with cf.ThreadPoolExecutor(a.workers) as ex, out.open("a" if a.append else "w") as f:
        for r in ex.map(lambda j: one(a.backend, a.model, *j), jobs):
            f.write(json.dumps(r, ensure_ascii=False) + "\n"); f.flush()
            print(r["id"], r["sample"], "ERR" if r.get("error") else "ok", file=sys.stderr)

if __name__ == "__main__":
    main()
