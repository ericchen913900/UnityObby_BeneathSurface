#!/usr/bin/env bash
set -euo pipefail

REPO_NAME="${1:-UnityObby_BeneathSurface}"
VISIBILITY="${2:-public}"

if command -v gh >/dev/null 2>&1; then
  GH_BIN="$(command -v gh)"
elif [ -x "/home/yeeee/tools/gh/gh_2.87.3_linux_amd64/bin/gh" ]; then
  GH_BIN="/home/yeeee/tools/gh/gh_2.87.3_linux_amd64/bin/gh"
else
  echo "gh CLI is not installed. Install first:"
  echo "  sudo apt update && sudo apt install gh"
  exit 1
fi

if ! "$GH_BIN" auth status >/dev/null 2>&1; then
  echo "GitHub auth required. Run: gh auth login"
  exit 1
fi

if [ ! -d .git ]; then
  git init
  git branch -m main
fi

if ! "$GH_BIN" repo view "$REPO_NAME" >/dev/null 2>&1; then
  "$GH_BIN" repo create "$REPO_NAME" --"$VISIBILITY" --source=. --remote=origin --push
else
  if ! git remote get-url origin >/dev/null 2>&1; then
    "$GH_BIN" repo set-default "$REPO_NAME"
    OWNER="$("$GH_BIN" api user -q .login)"
    git remote add origin "https://github.com/$OWNER/$REPO_NAME.git"
  fi
  git push -u origin main
fi

echo "Done."
