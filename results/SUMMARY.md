# Results

Strict = every trace line correct and in order. 95% CI: bootstrap over the items (the 3 samples of an item stay together).

| model | strict | 95% CI | pass@3 | all 3 | line similarity |
|---|---|---|---|---|---|
| claude-haiku-5-5 | 40% | 23% to 58% | 46% | 31% | 91% |
| claude-sonnet-5-5 | 58% | 40% to 74% | 65% | 50% | 94% |
| claude-opus-4-8 | 64% | 46% to 81% | 69% | 54% | 96% |
| claude-opus-5-5 | 60% | 42% to 78% | 65% | 54% | 95% |
| claude-fable-5-1 | 55% | 38% to 72% | 69% | 42% | 94% |

## By task (strict)

| task | items | claude-haiku-5-5 | claude-sonnet-5-5 | claude-opus-4-8 | claude-opus-5-5 | claude-fable-5-1 |
|---|---|---|---|---|---|---|
| T1 lifecycle & activation order | 5 | 53% | 47% | 53% | 40% | 40% |
| T2 coroutines & timers | 6 | 11% | 61% | 89% | 72% | 44% |
| T3 destruction & null semantics | 4 | 25% | 25% | 25% | 25% | 33% |
| T4 messaging & events | 3 | 33% | 67% | 22% | 67% | 78% |
| T5 physics steps & queries | 4 | 50% | 75% | 75% | 75% | 75% |
| T6 transform & rotation math | 2 | 100% | 100% | 100% | 100% | 100% |
| T7 exceptions inside callbacks | 2 | 50% | 50% | 100% | 67% | 50% |

## By item (samples passed out of 3)

| item | task | what it tests | claude-haiku-5-5 | claude-sonnet-5-5 | claude-opus-4-8 | claude-opus-5-5 | claude-fable-5-1 |
|---|---|---|---|---|---|---|---|
| S02 | T2 | Yield points within a frame | 2 | 3 | 3 | 3 | 2 |
| S05 | T6 | Parent/child transform math | 3 | 3 | 3 | 3 | 3 |
| S06 | T7 | Exceptions in handlers, UnityEvent, Awake | 0 | 0 | 3 | 1 | 0 |
| S07 | T1 | DefaultExecutionOrder | 0 | 0 | 0 | 0 | 0 |
| S08 | T1 | Instantiating an inactive template | 3 | 3 | 3 | 3 | 3 |
| S10 | T5 | AddForce vs velocity reads | 2 | 3 | 3 | 3 | 2 |
| S11 | T5 | Trigger rules with kinematic bodies | 0 | 0 | 0 | 0 | 1 |
| S12 | T3 | Null checks on destroyed objects | 3 | 3 | 3 | 3 | 3 |
| S13 | T4 | SendMessage / BroadcastMessage | 0 | 0 | 0 | 0 | 1 |
| S15 | T2 | StopCoroutine and disabling | 0 | 3 | 3 | 3 | 2 |
| S16 | T1 | LateUpdate follow chain | 3 | 3 | 3 | 3 | 3 |
| S17 | T1 | AddComponent from Awake and Start | 2 | 1 | 2 | 0 | 0 |
| S18 | T1 | Toggling enabled | 0 | 0 | 0 | 0 | 0 |
| S19 | T2 | Exception inside a coroutine | 0 | 2 | 3 | 3 | 3 |
| S20 | T2 | WaitUntil / WaitWhile | 0 | 1 | 2 | 0 | 0 |
| S22 | T4 | Delegates mutated while invoked | 3 | 3 | 2 | 3 | 3 |
| S23 | T5 | Kinematic MovePosition vs transform | 3 | 3 | 3 | 3 | 3 |
| S25 | T5 | Raycast and transform sync | 1 | 3 | 3 | 3 | 3 |
| S26 | T7 | Exceptions in Awake/OnEnable/Start/Update | 3 | 3 | 3 | 3 | 3 |
| S27 | T6 | Rotation order and Euler results | 3 | 3 | 3 | 3 | 3 |
| S28 | T4 | Broadcast to inactive children | 0 | 3 | 0 | 3 | 3 |
| S31 | T2 | Where coroutines start running | 0 | 0 | 3 | 2 | 1 |
| S33 | T2 | finally blocks when coroutines stop | 0 | 2 | 2 | 2 | 0 |
| S34 | T3 | Queries on a destroyed object | 0 | 0 | 0 | 0 | 1 |
| S35 | T3 | Destroying a component | 0 | 0 | 0 | 0 | 0 |
| S36 | T3 | Destroying a GameObject from its own Awake | 0 | 0 | 0 | 0 | 0 |

Items no model passed in any sample: S07, S18, S35, S36
