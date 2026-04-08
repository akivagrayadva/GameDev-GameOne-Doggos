# Project Context for AI Agents

**Read this file at the start of every development session.**

## Project Overview

**Name:** GameDev-GameOne-Doggos
**Purpose:** Dogcha! A roguelike gacha game about dogs and the messes they get into
**Language:** C#
**Engine:** Godot (with C# scripting)
**Current Phase:** Phase 1 - Paper Prototyping

**For detailed roadmap:** See [../../ROADMAP.md](../../ROADMAP.md)

## Critical Development Rules

### Code Authorship

This is a collaborative project. AI agents act in an *augmentative* capacity.

**HOMEWORK-MANIFESTO.md is active on this project. Its stricter rules take precedence over this section.**

DO NOT:
- Write large blocks of code without explaining your rationale
- Act without the permission of the programmer
- Make changes that the programmer does not understand

DO:
- Suggest code snippets and explain them
- Proof-read the codebase
- Assist with file management
- Explain your thought process using uncomplicated language
- Act as the "passenger" in a pair-programming context (where the programmer is the "driver")
- Identify errors in programmer-written code
- Identify code the programmer does not seem to understand
- Track progress of the project

### Testing Philosophy (NON-NEGOTIABLE)

**BEFORE writing any new code, you MUST:**

1. **Validate with schema** - All external data must pass validation
2. **Write tests first** - For new features, write at least one test before implementing
3. **Run tests** - Execute the test suite and verify all tests pass
4. **Check coverage** - Ensure coverage stays at or above 80%

**Test types required for new features:**
- **Schema validation** - If touching API or external data code
- **Integration test** - For any workflow changes
- **Property test** - For data processing logic
- **Unit test** - For complex algorithms

### Code Quality Standards

```
-- NEVER write code like this:
function parse_data(data)      -- Untyped, unvalidated
  return data.items            -- No validation
end

-- ALWAYS write code like this:
function parse_data(data)
  validated = validate(data)   -- Schema validation
  return {
    id: validated.id,
    items: validated.items,    -- Properly validated
  }
end
```

(Above is pseudocode. Apply the pattern in C#.)

### Error Handling

- Never silently catch errors
- Always validate external data
- Provide clear error messages
- Log validation failures with details

### Code Maintainability (HUMAN-MAINTAINED CODEBASE)

**This code will be maintained by humans.** Write code that is easy to understand and modify.

**DRY (Don't Repeat Yourself):**
- **Never duplicate helper functions** across test files
- Extract shared helpers to a common location
- If you find yourself copying code, extract it to a shared location first

**Shared Test Utilities:**
```
-- DON'T duplicate helpers in each test file
-- test-file-a
function create_test_fixture(...)  -- 50 lines

-- test-file-b
function create_test_fixture(...)  -- same 50 lines

-- DO extract to shared location
-- shared test fixtures file
export create_test_fixture(...)    -- single source

-- test-file-a and test-file-b both import from shared location
```

(Above is pseudocode. Apply the pattern in C#.)

**Documentation:**
- Write comments for exported functions
- Explain "why" not just "what" in complex logic
- Don't over-comment obvious code

**Code Organization:**
- Keep functions focused (single responsibility)
- Use descriptive names that explain intent
- Group related functionality in the same file
- Don't create files with just one small function

### File Organization (MANDATORY)

**BEFORE creating ANY file, read:** [FILE-ORGANIZATION.md](FILE-ORGANIZATION.md)

**Critical rules:**
- Root = MINIMAL (README, ROADMAP, CLAUDE.md, LICENSE, .gitignore, Godot project files only)
- Documentation goes in `documents/docs/`
- AI policies go in `documents/ai-usage/policies/`
- Resources go in `documents/resources/`
- Source code goes in Godot directories (`scenes/`, `scripts/`, `assets/`)
- Tests follow C# conventions (see FILE-ORGANIZATION.md)
- NO flat directories, NO files in root unless config
- **Always propose location before creating**

## Current Project State

### What Works
- Project documentation structure is complete
- Godot project initialized
- No game code yet (Phase 1 is paper prototyping, no code)

### Planned Features
- See ROADMAP.md for full Tetrad/MDA breakdown
- Phase 1: Paper prototyping (numeric balancing, room layouts, UI layouts)
- Phase 2: MVP I (playable game with placeholder art)
- Phase 3: MVP II (cohesive sprites, GUI menus, music, SFX)
- Phase 4: Bonus round (ideal/stretch elements)

### Test Files Location
See [FILE-ORGANIZATION.md](FILE-ORGANIZATION.md) for C#-specific test paths and naming conventions.

## Transparency and Compact Log Rules (NON-NEGOTIABLE)

**On EVERY context compaction**, you MUST:
1. Copy and paste the transparency log into a file at:
   `documents/ai-usage/[USERNAME]/transparency-logs/log-YYYY-MM-DD-USERNAME-##.md`
2. The `##` is a zero-padded enumeration per user per day. Numbers are never skipped.
3. If you forget a transparency log, create one retroactively with the proper name and enumeration, and include a manual note explaining the missing data.

**Example:** First transparency log of the day for user `thaimor`:
`documents/ai-usage/thaimor/transparency-logs/log-2026-04-01-thaimor-01.md`

## Session End Procedure (NON-NEGOTIABLE)

At the end of every session, regardless of remaining context:

**Step 1: Create a transparency log.** Run a final compaction even if context is not low. Save the transparency log per the rules above.

**Step 2: Run shutdown checklist.**
- Are tests passing?
- Is the root directory clean? (README.md, ROADMAP.md, CLAUDE.md, LICENSE, .gitignore, Godot project files only)
- Update ROADMAP.md and PROJECT-CONTEXT.md to reflect current status.

**Step 3: Create a session summary.** This is required even if the entire session produced only a single transparency log.

**Session summary location:** `documents/ai-usage/[USERNAME]/session-YYYY-MM-DD-USERNAME-##.md`

**Session summary format:**
```
# [USERNAME]'s Development Session ##
## YYYY-MM-DD

[One paragraph summarizing project status and session accomplishments.]

## What Was Worked On
- [Bullet point]
- [Bullet point]
- [...]

## Transparency Logs
- log-YYYY-MM-DD-USERNAME-01.md
- log-YYYY-MM-DD-USERNAME-02.md
- [...]
```

## Git Workflow

Use GitHub Desktop for all git operations.

### Branching Strategy

1. Pull origin and/or the feature branch to work on (e.g., `run-walk-toggle`)
2. Create a session branch from that branch (e.g., `thaimor-rwt-active`)
3. Work on the session branch. Commit frequently (at least 1 non-empty commit)
4. At end of session: merge back into feature branch if done, or leave open

### Branch Rules

- **Never work on a feature branch that has child branches**
- **Feature branches:** allowed to be unstable, no code review required. Coverage must be 80% but not all tests must pass (allows in-progress work)
- **Stable branch:** must be stable, requires code review
- **Release branch:** only contains working builds. No incomplete feature suites

### Rebase Strategy

**Rebase early, rebase often.** This is the primary strategy for preventing merge conflicts.

- Always rebase a feature branch with the stable branch before starting work
- Always rebase before making a pull request into stable
- With the branch rules in place, the only rebase conflicts that should arise are in documentation, which is far easier to resolve than code conflicts
- This keeps pull requests clean by shunting any conflicts to the rebase operation, not the PR itself

### Resource Restrictions

- Cannot use paid resources unless equivalent to a free version
- See [FOR-CONTRIBUTORS.md](../resources/FOR-CONTRIBUTORS.md) for the full list of approved resources

## Commands You Should Know

```bash
# Godot editor (open project)
godot --editor

# Run the game from CLI
godot --path . scenes/main.tscn

# Build C# solution
dotnet build

# Run tests (GdUnit4 or similar -- TBD once test framework is set up)
# [test commands here]
```

## Next Phase

**Goal:** Complete paper prototyping -- establish numeric balancing and visual layouts before writing code

**Implementation Plan:**
1. Establish currency spawning rates, prices, speed ratios, gacha drop rates
2. Finalize room layouts for Phases 2 and 3
3. Finalize menu and UI layouts

**Requirements for new code:**
1. Write integration test showing expected workflow
2. Add property tests for invariants
3. Validate any external data
4. Maintain 80%+ coverage

## Common Pitfalls to Avoid

### DON'T:
- Skip tests because "it's just a small change"
- Process external data without validation
- Write code without reading existing patterns first
- Include emojis in any code or documentation
- Create new files when editing existing ones would work
- **Create files in root directory** (see FILE-ORGANIZATION.md)
- **Create flat directory structures** (organize by purpose)
- Use unsafe/untyped patterns without good reason

### DO:
- Run the test suite while coding
- Read similar existing code first
- Use characters from the 128 standard ASCII characters
- Ask clarifying questions before big changes
- Update tests when changing behavior
- Keep functions small and focused

## Documentation

- [ROADMAP.md](../../ROADMAP.md) - **What to build** (features, acceptance criteria)
- [FILE-ORGANIZATION.md](FILE-ORGANIZATION.md) - **Where to put files** (mandatory)
- [HOMEWORK-MANIFESTO.md](../ai-usage/policies/HOMEWORK-MANIFESTO.md) - **Code authorship rules** (active)

## Key Architectural Decisions

### Why property-based testing?
Property tests catch edge cases we wouldn't think to test manually.

### Why integration tests over pure unit tests?
We care about teaching and testing end-to-end workflows.

## Session Checklist

At the start of each session, verify:
- [ ] Read this file (PROJECT-CONTEXT.md)
- [ ] Read [../../ROADMAP.md](../../ROADMAP.md) - Know what to build
- [ ] Check git status for uncommitted changes
- [ ] Identify current phase and next feature from roadmap

Before ending session:
- [ ] Final compaction done, transparency log saved
- [ ] All tests pass
- [ ] No build errors
- [ ] Code is formatted
- [ ] Root directory is clean
- [ ] ROADMAP.md and PROJECT-CONTEXT.md updated
- [ ] Session summary created in documents/ai-usage/[USERNAME]/
