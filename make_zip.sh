#!/bin/bash
# Builds the code-only submission zip from the committed tree (no Unity Library, no .env, no internal notes).
set -e
cd "$(dirname "$0")"
git archive --format=zip -o micro1eval-submission.zip HEAD
unzip -l micro1eval-submission.zip | tail -1
