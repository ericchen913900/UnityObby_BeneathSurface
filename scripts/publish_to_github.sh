#!/usr/bin/env bash
set -euo pipefail

REPO_NAME="${1:-UnityObby_BeneathSurface}"
VISIBILITY="${2:-public}"

if ! command -v gh >/dev/null 2>&1; then
  echo "gh CLI is not installed. Install first:"
  echo "  sudo apt update && sudo apt install gh"
  exit 1
fi

if ! gh auth status >/dev/null 2>&1; then
  echo "GitHub auth required. Run: gh auth login"
  exit 1
fi

if [ ! -d .git ]; then
  git init
  git branch -m main
fi

if ! gh repo view "$REPO_NAME" >/dev/null 2>&1; then
  gh repo create "$REPO_NAME" --"$VISIBILITY" --source=. --remote=origin --push
else
  if ! git remote get-url origin >/dev/null 2>&1; then
    gh repo set-default "$REPO_NAME"
    OWNER="$(gh api user -q .login)"
    git remote add origin "https://github.com/$OWNER/$REPO_NAME.git"
  fi
  git push -u origin main
fi

echo "Done."
