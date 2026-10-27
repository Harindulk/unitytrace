# UnityTrace

Can a language model predict what real Unity does? Each item is a short C# scenario. The model has to write the exact log trace Unity prints when the scenario runs in play mode: which callbacks fire, in what order, in which frame. The expected trace is not written by hand. It is captured by running the same scenario in Unity 6000.3.2f1, so the answer key is whatever the engine actually did.

The best result in this repo is 64% of runs fully correct. Four of the 26 items were never solved by any model in any of 15 samples.

## What it tests, who it is for, why it matters

It tests knowledge of engine behaviour that is not in the C# you read: Awake, OnEnable and Start ordering, when Destroy takes effect, what coroutines do when they are stopped or disabled, what happens to a component after an exception, when physics callbacks fire. Game developers hit these bugs daily, and a model that helps with Unity code has to get them right to avoid plausible, wrong advice ("this runs before that").

It is for people who build or choose coding assistants for game development, and for anyone who wants an example of an eval where the answer key comes from running the real system instead of from a person's memory. While piloting, an expected value I wrote from memory turned out to be wrong: `Mathf.LerpAngle(350, 10, 0.5)` returns 360, not 0. The models had it right and I did not. That is the reason the key comes from Unity.

Models get no tools and no Unity. They must reason from the code alone, which is what a developer asking a chat assistant gets.

## Results

Five Claude models, 26 scored items, 3 samples each, strict scoring (every line of the trace right and in order). The CI is a bootstrap over items.

| model | strict | 95% CI | pass@3 | all 3 samples right |
|---|---|---|---|---|
| claude-haiku-5-5 | 40% | 23 to 58% | 46% | 31% |
| claude-sonnet-5-5 | 58% | 40 to 74% | 65% | 50% |
| claude-opus-4-8 | 64% | 46 to 81% | 69% | 54% |
| claude-opus-5-5 | 60% | 42 to 78% | 65% | 54% |
| claude-fable-5-1 | 55% | 38 to 72% | 69% | 42% |

The intervals are wide. Haiku is clearly behind; the other four cannot be ranked from 26 items. Partial credit is high (line similarity 91 to 96%), so the models are mostly close and fail on one or two lines, which is exactly what makes these bugs hard to spot. Full tables by task and by item are in [results/SUMMARY.md](results/SUMMARY.md), raw responses in `results/*.jsonl`.

### What the models get wrong

The failures agree with each other, which points at shared beliefs rather than noise. In the first divergence of each failed sample:

- Destroy takes effect on the enabled state immediately. After `Destroy(component)` the component is already disabled and `OnDisable` has run; only `OnDestroy` waits for the end of the frame. Models put `OnDisable` at the end of the frame. No model passed the two Destroy items S35 and S36 in any sample.
- `OnDisable` can run without `OnEnable`. Setting `enabled = false` inside `Awake` (S18), or destroying a GameObject in its own `Awake` (S36), calls `OnDisable` anyway. All models skipped it.
- `[DefaultExecutionOrder]` reorders `Update`, but in S07 the `Start` calls still ran in the order the components were added. 15 of 15 samples sorted `Start` by execution order.
- A trigger fires `OnTriggerStay` in the same physics step as `OnTriggerEnter` (S11). 14 of 15 failures left it out.
- `Instantiate` on a destroyed object throws `MissingReferenceException`; models said `ArgumentException` (S34).
- An exception in `Awake` stops that component's `OnEnable`, `Start` and `Update` (S06). Only Opus 4.8 reliably got this.

## How it works

1. `unity_truth/Assets/Scenarios/S*.cs` holds one scenario per file. Every scenario defines `Scenario.Run()` and a frame count, and logs through `Trace.Log`.
2. `capture_truth.sh` runs every scenario in Unity play mode in batch mode. A player-loop hook calls `Run()` at the very start of the first frame and writes `--- frame N ---` at the start of every later frame. The result goes to `truth/S*.txt`. Time is fixed (`captureFramerate` 50, `fixedDeltaTime` 0.02), so runs are deterministic.
3. `build_items.py` turns each scenario plus its trace into one prompt and one answer in `data/items.jsonl`. The prompt contains the source, the harness rules and the answer format, nothing else.
4. `run_eval.py` asks a model. `score.py` pulls the last fenced code block from the reply, strips whitespace and blank lines, and compares the line list to the key. There is no model-as-judge anywhere.

The 26 items fall into 7 tasks: lifecycle and activation order (5 items), coroutines and timers (6), destruction and null semantics (4), messaging and events (3), physics steps and queries (4), transform and rotation math (2), exceptions inside callbacks (2). The transform items work as a control: every model passes them.

## Run it

Python 3.9 or later, standard library only.

```bash
python3 selftest.py    # checks the grader: gold passes, damaged answers fail, whitespace is ignored
python3 run_eval.py --backend claude-cli --model claude-opus-5-5 --samples 3
python3 score.py
python3 report.py      # writes results/SUMMARY.md
```

Backends:

- `claude-cli` uses a logged-in Claude Code CLI in an isolated mode (no tools, no settings, no project files). All the results above come from this backend, with the CLI's default thinking and effort.
- `anthropic` calls the Messages API with adaptive thinking and effort `high` (set `ANTHROPIC_API_KEY` in `.env`, copy `.env.example`).
- `openai` calls any OpenAI-compatible endpoint such as OpenRouter (`OPENAI_API_KEY`, `OPENAI_BASE_URL`), for other vendors or free models.

The `anthropic` and `openai` backends were written but not run in the session that built this repo, so treat them as untested. Everything else was run.

Cost and time, measured with the CLI backend (3 samples of every item, list-price equivalent): Haiku 5.5 about $0.2, Sonnet 5.5 $1.2, Opus 5.5 $2.9, Fable 5.1 $10.5, Opus 4.8 $14.6. A call takes 15 to 75 seconds on average. With `--workers 8` one model finishes in roughly 10 to 25 minutes.

You do not need Unity to run the eval. Regenerating the answer key does need it (Unity 6000.3.2f1 and a licence; close any Editor that has `unity_truth` open first):

```bash
./capture_truth.sh
python3 build_items.py
```

## Checks on the answer key

- Each trace was captured twice and was identical both times.
- All scenarios were also run on Unity 6000.6.0f1. Three differ: S01, S03 and S29, where the order of parent and child callbacks changed between versions. They are kept in `data/version_sensitive.jsonl` and never scored, because "correct" is not stable for them. The 26 scored items produce identical traces on both versions.
- `selftest.py` confirms the grader accepts every gold answer, rejects dropped, swapped and edited lines, and ignores whitespace and surrounding commentary.
- Two scenarios that depended on wall-clock alignment (timer-based Destroy and Invoke) were removed after a probe showed their outcome depended on the absolute time at which the harness started.

## Limits

- Only Claude models were run, because no other vendor's key was available. The scenarios say nothing specific to Claude, so another vendor can be run with the `openai` backend.
- With 26 items and 3 samples the confidence intervals are wide, and items differ a lot in difficulty. Do not read a ranking from the top four rows.
- The CLI backend uses the CLI's default thinking and effort, which are not pinned in this repo. The API backend pins them. Scores from the two may differ.
- The answer key is one Unity version on one machine. Unity may change some of these behaviours later, as it did for the three version-sensitive items.
- Frame numbers, the position of `Run()` in the loop and the fixed time step are properties of the harness. They are stated in every prompt, but they are not how a shipped game starts.
- The scenarios were written by one author with AI assistance and have not been reviewed by other Unity developers. There is no human baseline.
- A model that can run Unity could simply run the scenario. This eval measures what a model knows, not what an agent with the engine could find out.

## Disclosure

The repository was created on 27 October 2026 with only a licence and a gitignore. All scenarios, scripts and results were made after that, before the hackathon kickoff on 30 October, and nothing in it existed earlier. AI coding tools (Claude Code) wrote the scenarios, scripts and this text; the author directed the project, chose the domain and is responsible for the result. The expected traces come from the Unity Editor, not from a model.

## Files

- `data/items.jsonl`: scored items (id, task, prompt, answer lines). `data/version_sensitive.jsonl`: the 3 items not scored.
- `truth/`: traces captured from Unity 6000.3.2f1.
- `unity_truth/`: the Unity project that produces the traces (`Assets/Scenarios`, `Assets/Tests/PlayMode/Capture.cs`).
- `results/`: raw model responses and `SUMMARY.md`.
- `run_eval.py`, `score.py`, `report.py`, `build_items.py`, `selftest.py`, `capture_truth.sh`, `make_zip.sh`.

Licence: MIT.
