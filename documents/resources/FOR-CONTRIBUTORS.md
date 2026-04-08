# For Contributors

A reference guide for everyone working on GameDev-GameOne-Doggos. This document covers project policies, workflows, and tooling expectations. Specs here are duplicated in the relevant spec documents (PROJECT-CONTEXT.md, FILE-ORGANIZATION.md) so this file can serve as a standalone onboarding reference.

---

## AI Usage and Transparency

AI is used to augment our capacity to learn and create, not replace it. See [HOMEWORK-MANIFESTO.md](../ai-usage/policies/HOMEWORK-MANIFESTO.md) for the full code authorship policy.

### Transparency Log Requirements

- All AI usage and prompting must be disclosed via transparency logs or full session transcripts
- Logs go in `documents/ai-usage/[YOUR GITHUB USERNAME]/transparency-logs/`
- Log filenames follow the format `log-YYYY-MM-DD-USERNAME-##.md`

### Session Summary Requirements

- A short session summary is required at the end of every session
- Summaries go in `documents/ai-usage/[YOUR GITHUB USERNAME]/`
- Summary filenames follow the format `session-YYYY-MM-DD-USERNAME-##.md`

### Per-Tool AI Workflows

These are suggested workflows. You are not required to use them if you can accomplish the same transparency another way, but the project is optimized with them in mind.

**Claude Code:**
- For transparency logs, copy and paste compact logs as your log entry
- Include the URL at the end directing to the full (not-super-human-readable) session transcript
- You may sanitize the filepath to be relative so your machine's file structure is not revealed

**Perplexity:**
- For transparency logs, use the 3-dot menu in the top right and click "Export to Markdown"
- Copy and paste the text into your log file, or paste the exported file into your transparency-logs folder and rename it to match the naming convention

---

## Resource Restrictions

- Cannot use paid resources UNLESS they are equivalent (in all but convenience) to a free version
  - This includes game engines, art programs, and all other tools
  - If you want to use a paid resource, you may IF AND ONLY IF you also agree to pay for it for the whole class
  - Students may not take up the offer, but it must be agreed to per class rules

### Known Approved Resources

| Resource | Reason |
|----------|--------|
| Godot | Open source |
| Audacity | Open source |
| FireAlpaca / FireAlpaca SE | SE is paid but contains no additional features over the free version (only zero ads and a better UI) |
| Aseprite | Paid but contains fewer features than FireAlpaca; UI is optimized for pixel art |

---

## Git Workflow

### Tool

Use GitHub Desktop, not IDE-based or terminal/gitbash-based git.

### Branching Strategy

1. Pull origin and/or the feature branch you want to work on (e.g., `run-walk-toggle`)
2. Create a branch from that branch for your session (e.g., `thaimor-rwt-active`)
3. Work on your active branch. Commit as much as you want (at least 1 non-empty commit per session)
4. At end of session:
   - Merge your active branch back into the feature branch if your work is done
   - Leave it open if you plan to continue in another session
   - Do NOT plan on requesting a feature-to-stable merge at the end of a session

### Branch Rules

- **Do not work on any feature branch that has child branches. At all.**
- **Feature branches** are allowed to be unstable. They do not require code review. Test coverage must still be at 80%, but not all tests need to pass in case of an abrupt session end.
- **Stable branch** MUST be stable and requires code review before merge.
- **Release branch** ONLY contains working builds of the game. No incomplete feature suites (multiple feature branches may make up an entire feature).

### Rebase Strategy

**Rebase early, rebase often.** This is how we prevent merge conflicts.

- Always rebase a feature branch with the stable branch before starting work
- Always rebase before making a pull request into stable
- With the branch rules above, the only rebase conflicts that should arise are in documentation -- much easier to resolve than code conflicts
- This keeps pull requests clean: conflicts are resolved during the rebase, not in the PR

---

## Test Coverage

- Target: 80% coverage
- Feature branches: coverage requirement applies, but not all tests must pass (allows for in-progress work)
- Stable branch: all tests must pass AND meet coverage target

---

## File Organization

All file placement rules are in [FILE-ORGANIZATION.md](../docs/FILE-ORGANIZATION.md). Key points for contributors:

- Root directory is minimal (README, ROADMAP, CLAUDE.md, LICENSE, .gitignore, Godot project files)
- All documentation lives under `documents/`
- Source code goes in Godot directories (`scenes/`, `scripts/`, `assets/`)
- C# files use `PascalCase.cs`, directories use `PascalCase/`
- Tests go in a separate test project

---

## Quick Links

- [ROADMAP.md](../../ROADMAP.md) -- What to build
- [PROJECT-CONTEXT.md](../docs/PROJECT-CONTEXT.md) -- Development rules and AI agent context
- [FILE-ORGANIZATION.md](../docs/FILE-ORGANIZATION.md) -- Where to put files
- [HOMEWORK-MANIFESTO.md](../ai-usage/policies/HOMEWORK-MANIFESTO.md) -- Code authorship rules
- [PROMPT-QUICK-REF.md](PROMPT-QUICK-REF.md) -- Copy-paste prompts for AI sessions
