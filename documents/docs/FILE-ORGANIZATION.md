# File Organization Rules

**MANDATORY: AI agents must follow this structure when creating files.**

**This project uses C#.** Follow the C#-specific conventions in this file.

## The Problem

AI agents tend to create "flat" directory structures, dumping all files in the root or creating poorly organized folders. This leads to:
- Cluttered root directory
- Unclear file purposes
- Difficulty finding files
- Merge conflicts

## Universal Rules

### Rule 1: Root Directory - MINIMAL FILES ONLY

**ONLY these files belong in root:**
```
GameDev-GameOne-Doggos/
+-- README.md              Project overview
+-- ROADMAP.md             Project roadmap
+-- CLAUDE.md              Agent auto-read config (Claude Code)
+-- .gitignore             Git config
+-- LICENSE                License file
+-- project.godot          Godot project file
+-- *.csproj / *.sln       Godot-generated C# build files
```

**Everything else goes in subdirectories!**

### Rule 2: Standard Directories

| Directory                                          | Purpose                                        | Naming Convention               |
| -------------------------------------------------- | ---------------------------------------------- | ------------------------------- |
| `documents/`                                       | All documentation (Obsidian vault)             | See subfolders below            |
| `documents/docs/`                                  | Project documentation and guides               | SCREAMING-CASE.md               |
| `documents/resources/`                             | Contributor references and prompting resources | SCREAMING-CASE.md               |
| `documents/ai-usage/policies/`                     | AI usage policies and rules                    | SCREAMING-CASE.md               |
| `documents/ai-usage/[username]/`                   | Per-contributor session summaries and logs     | kebab-case.md                   |
| `documents/ai-usage/[username]/transparency-logs/` | Transparency logs from context compaction      | `log-YYYY-MM-DD-USERNAME-##.md` |
| `data/`                                            | Data files, cached responses                   | lowercase                       |
| `data/cache/`                                      | Cached API/external responses                  | lowercase                       |

### Rule 3: Always Propose Location Before Creating

Before creating any file, the AI must:
1. Determine the file type (doc, AI context, source, test, data, config)
2. Check if similar files already exist
3. Propose the location and wait for confirmation

---

## File Placement Decision Tree

```
+-- Is it user-facing documentation or a project guide?
|  +-- Yes -> documents/docs/
|  +-- No
|
+-- Is it an AI usage policy?
|  +-- Yes -> documents/ai-usage/policies/
|  +-- No
|
+-- Is it a contributor reference or prompting resource?
|  +-- Yes -> documents/resources/
|  +-- No
|
+-- Is it a session summary?
|  +-- Yes -> documents/ai-usage/[username]/session-YYYY-MM-DD-USERNAME-##.md
|  +-- No
|
+-- Is it a transparency log?
|  +-- Yes -> documents/ai-usage/[username]/transparency-logs/log-YYYY-MM-DD-USERNAME-##.md
|  +-- No
|
+-- Is it a wiki page?
|  +-- Yes -> documents/ (snake_case.md, follows Obsidian markdown structure)
|  +-- No
|
+-- Is it source code?
|  +-- Yes -> See C# directory structure below
|  +-- No
|
+-- Is it a test?
|  +-- Yes -> See C# test location below
|  +-- No
|
+-- Is it cached data?
|  +-- Yes -> data/cache/
|  +-- No
|
+-- Is it a config file?
|  +-- Yes -> ROOT (only if essential)
|  +-- No -> ASK USER WHERE IT BELONGS
```

---

## C# Conventions

### Quick Reference

| Convention | C# |
|-----------|-----|
| File naming | `PascalCase.cs` |
| Directory naming | `PascalCase/` |
| Test file naming | `[Name]Tests.cs` |
| Test location | Separate test project |
| Entry point | Godot scene + attached script |

### Godot Directory Structure

Godot uses `res://` as its virtual root, which maps to the project root directory. Godot auto-generates `project.godot`, `.godot/`, and C# build files (`*.sln`, `*.csproj`) -- do not manually edit these.

- Script files: `PascalCase.cs` (matches class names)
- Scene files: `snake_case.tscn` (Godot convention)
- Resource files: `snake_case.tres` (Godot convention)
- Directories: `snake_case/` for Godot asset folders (scenes, scripts, assets, etc.)
- Tests: `[Name]Tests.cs` in a dedicated `tests/` directory using GdUnit4 or similar
- Solution file (`*.sln`) is auto-generated in root by Godot -- do not commit manually

### Project Structure

```
GameDev-GameOne-Doggos/
|
+-- README.md
+-- ROADMAP.md
+-- CLAUDE.md
+-- LICENSE
+-- .gitignore
+-- project.godot                           Godot project file (auto-generated)
+-- *.sln / *.csproj                        Godot C# build files (auto-generated)
|
+-- .godot/                                 Godot internal cache (gitignored)
|
+-- scenes/                                 Godot scene files (.tscn)
|   +-- [organized by game area]
|
+-- scripts/                                C# scripts (.cs)
|   +-- [organized by purpose]
|
+-- assets/                                 Art, audio, fonts, etc.
|   +-- [organized by type]
|
+-- resources/                              Godot resource files (.tres)
|
+-- tests/                                  Test project (GdUnit4 or similar)
|
+-- documents/                              Obsidian vault
|   +-- .obsidian/                          Obsidian config
|   +-- resources/
|   |   +-- FOR-CONTRIBUTORS.md             Contributor reference guide
|   |   +-- PROMPT-QUICK-REF.md             Copy-paste prompts
|   +-- ai-usage/
|   |   +-- policies/
|   |   |   +-- HOMEWORK-MANIFESTO.md       Code authorship rules
|   |   +-- [username]/                     Per-contributor session summaries
|   |       +-- session-YYYY-MM-DD-USERNAME-##.md
|   |       +-- transparency-logs/
|   |           +-- log-YYYY-MM-DD-USERNAME-##.md
|   +-- docs/
|       +-- FILE-ORGANIZATION.md            This file
|       +-- PROJECT-CONTEXT.md              AI agent context
|
+-- data/
|   +-- cache/
|       +-- *.json
```

---

## Documentation Files

**Rule:** All documentation lives under `documents/`, organized by subfolder purpose.

```
CORRECT:
documents/docs/TESTING-GUIDE.md
documents/ai-usage/policies/HOMEWORK-MANIFESTO.md
documents/resources/PROMPT-QUICK-REF.md

WRONG:
TESTING-GUIDE.md                   (root is cluttered)
src/docs/guide.md                  (docs not in src/)
.claude/PROJECT-CONTEXT.md         (no .claude/ folder in this project)
```

### Naming Conventions by File Type

| File type                           | Convention                          | Example                            |
| ----------------------------------- | ----------------------------------- | ---------------------------------- |
| Project docs / policies / resources | `SCREAMING-CASE.md`                 | `FILE-ORGANIZATION.md`             |
| Session summaries                   | `session-YYYY-MM-DD-USERNAME-##.md` | `session-2026-04-01-thaimor-01.md` |
| Transparency logs                   | `log-YYYY-MM-DD-USERNAME-##.md`     | `log-2026-04-01-thaimor-01.md`     |
| Wiki pages (Obsidian)               | `snake_case.md`                     | `dog_breeds.md`                    |

The `##` in session and transparency log filenames is a zero-padded enumeration that forces ordering within a user+date. Numbers are never skipped.

---

## Common Mistakes & Corrections

### Mistake 1: Using Wrong Naming Convention

```
C#:       data-processor.cs     WRONG (non-standard)
          DataProcessor.cs      CORRECT

C#:       dataProcessor.cs      WRONG (camelCase)
          DataProcessor.cs      CORRECT
```

### Mistake 2: Putting Tests in Wrong Location

```
C#:       src/Project/Tests/ApiTests.cs              WRONG (tests in main project)
          tests/Project.Tests/ApiClientTests.cs      CORRECT
```

### Mistake 3: Creating Utility Dumping Grounds

```
WRONG:  src/Utils/Everything.cs    (catch-all)

CORRECT: Ask "What does this utility do?" and place it accordingly:
- String formatting -> Core/Formatters/StringFormatter.cs
- API helpers       -> Core/Api/ApiHelpers.cs
- Analysis helpers  -> Core/Analysis/AnalysisHelpers.cs
```

### Mistake 4: Creating Docs Outside documents/

```
WRONG:  FEATURE-GUIDE.md in root

CORRECT: documents/docs/FEATURE-GUIDE.md
```

### Mistake 5: Using Agent-Specific Directories

```
WRONG:  .claude/PROJECT-CONTEXT.md
WRONG:  .copilot/context.md

CORRECT: documents/docs/PROJECT-CONTEXT.md
```

---

## AI Agent Instructions

### When User Says "Create a [X]"

**Step 1: Determine type** (doc, AI context/policy, source, test, data, config)

**Step 2: Check this file** for correct paths and naming

**Step 3: Propose location** before creating

**Step 4: Follow C# naming conventions** (see quick reference table)

---

## Verification Checklist

### After Every File Creation

- [ ] File is in correct directory per decision tree
- [ ] File follows C# naming conventions
- [ ] Similar files are in same directory
- [ ] Root directory has minimal files
- [ ] Tests follow C# test location convention
- [ ] No catch-all "utils" or "helpers" folders
- [ ] Documentation is under documents/, not root or src/

### Session End Checkpoint

```
"Before we finish, verify file organization:
1. ls on root -- should only see README.md, ROADMAP.md, CLAUDE.md, LICENSE, .gitignore, and Godot project files
2. ls documents/docs/ -- should see project documentation
3. ls documents/ai-usage/ -- should see AI policies and resources
4. Any files in wrong location? Move them now.
5. Session summary created in documents/ai-usage/[username]/session-YYYY-MM-DD-USERNAME-##.md
6. All transparency logs saved in documents/ai-usage/[username]/transparency-logs/"
```

---

## Summary

**Golden Rules:**
1. **Root directory = MINIMAL** (README, ROADMAP, CLAUDE.md, LICENSE, .gitignore, Godot project files only)
2. **Project docs go in `documents/docs/`** (guides, references, context)
3. **Resources go in `documents/resources/`** (contributor guide, prompts)
4. **AI policies go in `documents/ai-usage/policies/`** (manifesto, rules)
5. **Code goes in Godot directories** (`scenes/`, `scripts/`, `assets/`)
6. **Tests follow C# conventions** (separate test project)
7. **Always propose location before creating**
8. **Follow C# naming conventions** (PascalCase for files and directories)

**When AI forgets:**
```
"Review documents/docs/FILE-ORGANIZATION.md
Follow the C# section.
Where should [file] be placed?
Move it if it's in the wrong location."
```

**Prevention:**
Include in every prompt: "Follow file organization rules from documents/docs/FILE-ORGANIZATION.md"
