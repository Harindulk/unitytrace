#!/bin/bash
# Regenerates truth/*.txt by running every scenario in real Unity (play mode, batch mode).
# Needs Unity 6000.3.2f1 and a licence. Close any Editor that has unity_truth open first.
set -e
cd "$(dirname "$0")"
UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.3.2f1/Unity.app/Contents/MacOS/Unity}"
"$UNITY" -batchmode -projectPath unity_truth -runTests -testPlatform PlayMode \
  -testResults "$PWD/unity_truth/results.xml" -logFile "$PWD/unity_truth/unity.log"
