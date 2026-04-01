# Prompt Quick Reference

**Purpose:** Copy-paste prompts for common scenarios. Keep this file open during development sessions. These prompts are agent-agnostic and work with any AI coding assistant.

**Markdown Preview:** ctrl+shift+v

---

## Quick Navigation

| Scenario | Prompt |
|----------|--------|
| Start session | [Session Start](#session-start) |
| Quick sync (~10 exchanges) | [Quick Checkpoint](#quick-checkpoint) |
| After compaction (~20 exchanges) | [Full Checkpoint](#full-checkpoint) |
| AI seems confused | [Context Recovery](#context-recovery) |
| Build feature | [Feature Implementation](#feature-implementation) |
| Fix bug | [Bug Fix](#bug-fix) |
| Create docs | [Create Documentation](#create-documentation) |
| Create code | [Create Source Code](#create-source-code) |
| AI made a mistake | [Correction Prompts](#correction-prompts) |
| End session | [Session End](#session-end) |
| Check status | [Status Check](#status-check) |
| Refactor | [Refactor Code](#refactor-code) |
| Pre-commit | [Pre-Commit Check](#pre-commit-check) |

---

## How These Prompts Handle Context Loss

AI agents lose context over long conversations. These prompts are tiered to manage that:

- **Session Start** assumes the agent has access to project files. It asks for a status report and sets the tone.
- **Quick Checkpoint** is lightweight. No file re-reads. Just inline reminders and a test run. Use this every ~10 exchanges.
- **Full Checkpoint** re-reads PROJECT-CONTEXT.md only (not FILE-ORGANIZATION.md, which is rarely needed mid-session). Use this after context loss or every ~20 exchanges.
- **Context Recovery** is the nuclear option. Re-reads everything. Use only when the AI is clearly confused about the project or ignoring rules.

**Rule of thumb:** If the AI is still following the rules, use Quick Checkpoint. If it starts forgetting rules or tone, use Full Checkpoint. If it has gone off the rails, use Context Recovery.

---

## Pro Tips

### Keep This File Open

- Open in split view while working
- Ctrl+F to search for scenario
- Copy prompt, paste in AI chat
- Modify `[placeholders]` as needed

### VS Code Snippets

Create snippet for your project:
1. File > Preferences > Configure User Snippets
2. New Global Snippets file: "GameDev-GameOne-Doggos-prompts"
3. Add snippets for your most-used prompts

---

## Session Start

**When:** Beginning of every new session.

```
Hey! I'm working on GameDev-GameOne-Doggos.

Read these files and give me a quick status:
1. documents/ai-usage/policies/HOMEWORK-MANIFESTO.md
2. documents/docs/PROJECT-CONTEXT.md
3. ROADMAP.md

Then tell me:
- What phase are we in?
- Any uncommitted changes?
- Does the project match ROADMAP.md?

Keep things conversational -- you're my pair-programming partner, not
a terminal. Explain your thinking in plain language.

HOMEWORK-MANIFESTO.md is active on this project: you guide,
I write the code. No exceptions.

Let's work on: [your goal]
```

---

## Quick Checkpoint

**When:** Every ~10 exchanges. Lightweight, no file re-reads.

```
Quick checkpoint before we keep going.

Quick reminders (no need to re-read files):
- Tests before code
- Files go where FILE-ORGANIZATION.md says
- No emojis, ASCII only
- HOMEWORK-MANIFESTO.md is active: you suggest, I write

Let's continue with: [next task]
```

---

## Full Checkpoint

**When:** After context loss or every ~20 exchanges.

```
I think you've lost some context. Let's get back on track.

Re-read documents/docs/PROJECT-CONTEXT.md to restore the development rules.

Then give me a quick summary:
- What have we accomplished this session?
- Any files we created in the wrong location?
- What's left to do?

Tone check: keep things conversational. You're my pair-programming
navigator. Explain your thinking, suggest approaches, and keep me
in the loop.

HOMEWORK-MANIFESTO.md is active: you guide, I write.
No inserting code into the codebase.

Let's continue with: [next task]
```

---

## Context Recovery

**When:** The AI is clearly confused -- ignoring rules, wrong tone, forgetting project details.

```
I think you've lost a lot of context. Let's do a full reset.

Please re-read these files in order:
1. documents/ai-usage/policies/HOMEWORK-MANIFESTO.md
2. documents/docs/PROJECT-CONTEXT.md
3. documents/docs/FILE-ORGANIZATION.md

Then confirm:
- You understand the testing requirements
- You understand the file organization rules
- You know whether homework rules apply
- You'll keep the tone conversational and explain things in
  plain language

What phase are we in, and what were we working on?
```

---

## Feature Implementation

**When:** Asking AI to help build a new feature

```
I want to work on [feature name].

Walk me through the approach:
1. What tests should we write first?
2. Are there property tests we should consider?
3. Where should the files go? (Check documents/docs/FILE-ORGANIZATION.md)

Remember the testing philosophy from PROJECT-CONTEXT.md:
tests first, then implementation.

Check FILE-ORGANIZATION.md for correct paths and naming conventions.
```

**Homework project variant:**
```
I want to work on [feature name].

Walk me through the approach. Remember, this is a homework project --
help me think through it, but I need to write the code myself.

1. What should I test first? Describe the test in pseudocode.
2. What's the implementation strategy? Explain the algorithm.
3. Where should the files go? (Check documents/docs/FILE-ORGANIZATION.md)

Let's start with the test design.
```

---

## Bug Fix

**When:** Asking AI to help fix a bug

```
I'm seeing a bug: [bug description].

Help me work through it:
1. What test would reproduce this bug?
2. Where do you think the problem is?
3. What's the fix?

After we fix it, we need to make sure everything still passes.
```

**Homework project variant:**
```
I'm seeing a bug: [bug description].

Help me track it down. Remember, homework project -- I'll write the
fix, but I need your help understanding the problem.

1. What test would reproduce this?
2. Walk me through what's happening and why.
3. What approach should I take to fix it?
```

---

## Create Documentation

**When:** Asking AI to create or update project docs

```
I need documentation for [topic].

Create it at documents/docs/[NAME].md using SCREAMING-CASE naming.
Reference any existing docs that are relevant.

Per FILE-ORGANIZATION.md: all project docs go in documents/docs/.
```

---

## Create Source Code

**When:** Asking AI to help create new code files

```
I need to create [class/function] for [purpose].

Let's figure out the right approach:
- Where should it go? (Check documents/docs/FILE-ORGANIZATION.md)
- What tests do we need?
- Tests first, then implementation.
- External data needs validation.
```

**Homework project variant:**
```
I need to create [class/function] for [purpose].

Help me plan it out -- I'll do the writing. Where should the file go,
what should the tests look like (in pseudocode), and what's the
implementation strategy?
```

---

## Correction Prompts

### AI Forgot Tests

**When:** AI writes or suggests code without tests

```
Hold on -- we skipped tests. Per PROJECT-CONTEXT.md, we need tests
before code. What tests should we write for this?
```

### AI Created File in Wrong Location

**When:** AI creates file in root or wrong directory

```
That file ended up in the wrong place. Per FILE-ORGANIZATION.md:
- Project documentation goes in documents/docs/
- AI policies go in documents/ai-usage/policies/
- Resources go in documents/resources/
- Source code goes in the Godot directories (scenes/, scripts/, assets/)
- Tests follow C# test location conventions

Can you move it?
```

### AI Forgot Data Validation

**When:** AI processes external data without validation

```
We're missing validation on that external data. Per PROJECT-CONTEXT.md,
all external data needs validation before we process it. Can we add that?
```

### AI Wrote Code Directly (Homework)

**When:** AI inserts code into the codebase on a homework project

```
Hey, remember this is a homework project. HOMEWORK-MANIFESTO.md says
you guide and I write. Can you undo that change and instead walk me
through what the code should do? Pseudocode is fine.
```

### AI Tone Went Robotic

**When:** AI responses become terse, overly formal, or list-heavy

```
Your tone shifted -- you're sounding pretty robotic. Remember, we're
pair-programming here. Keep things conversational. Explain your
thinking like you're talking to a colleague, not writing a report.
```

---

## Session End

**When:** Wrapping up a development session. Even if the session was short or resulted in a single transparency log, you MUST run this checklist.

```
Let's wrap up. Run through the full shutdown procedure:

1. Create a transparency log (regardless of remaining context). Save it
   to documents/ai-usage/[USERNAME]/transparency-logs/log-YYYY-MM-DD-USERNAME-##.md

2. Are tests passing?

3. Check the root directory -- should only have README.md, ROADMAP.md,
   CLAUDE.md, LICENSE, .gitignore, and Godot project files. Anything else sneak in?

4. Update ROADMAP.md and PROJECT-CONTEXT.md to reflect current project status.

5. Create a session summary at:
   documents/ai-usage/[USERNAME]/session-YYYY-MM-DD-USERNAME-##.md

   Format:
   - Title: "[USERNAME]'s Development Session ##"
   - Subtitle: "YYYY-MM-DD"
   - A paragraph summarizing project status and accomplishments
   - Bullets of what was worked on
   - List of all transparency log filenames from this session

If everything looks good, I'll commit.
```

---

## Status Check

**When:** You want a quick project status

```
Quick status check:
1. What phase are we in?
2. What's next on the ROADMAP?
3. Any uncommitted changes?
```

---

## Verify File Organization

**When:** Checking if files are in correct locations

```
Can you do a file organization audit?

Check documents/docs/FILE-ORGANIZATION.md, then:
- Are there any .md files in root besides README, ROADMAP, and CLAUDE?
- Are all docs under documents/?
- Anything look out of place?
```

---

## Refactor Code

**When:** Asking AI to help refactor existing code

```
I want to refactor [code] because [reason].

Before we touch anything:
1. Check our test baseline
2. Then we'll make changes
3. Then verify everything still passes

Tests are our safety net here. If tests pass before and after,
the refactor is good.
```

---

## Pre-Commit Check

**When:** Before committing changes

```
Pre-commit check:
1. Are tests passing?
2. Is the build clean?
3. ls on root -- only README.md, ROADMAP.md, CLAUDE.md, LICENSE,
   .gitignore, and Godot project files?

Green across the board? I'll commit.
```

---

## Read Roadmap

**When:** You want to know what to build next

```
Can you read ROADMAP.md and tell me where we stand?
- What phase are we in?
- What's the next feature or milestone?
- What are the acceptance criteria?
- What tests will we need?

Then let's talk through the approach.
```

---
