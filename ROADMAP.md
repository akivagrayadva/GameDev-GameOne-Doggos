# GameDev-GameOne-Doggos Project Roadmap

**Last Updated:** 2026-04-01

---


## Assignment Specs

### Target Outcome

A roguelike gacha game about dogs and the messes they get into.

### Required Components

- Working student-defined game
- .exe download of final game from github

### Optional Components

- none from professor

### Deliverables

- Final program .exe
- AI Documentation folders
    - Reproduction of AI chat history
    - Notes on what AI was used for and why
    - Any AI-specific files in the project

---

## Project Specs

### Game Overview

This game blends two genres, roguelikes and gacha games, to make a pet-based roguelike where upgrades are entirely dependent on what pulls you land in the gacha game. The story evolves through some flavor text and environmental storytelling, following a young family as they try to wrangle their high-energy hound(s) through several life stages. 

### Key Aspects

### Layered Tetrad Elements
| Tetrad Segment | Definition | MVP Elements | Ideal Elements | Stretch Elements |
|-|-|-|-|-|
| Mechanics | Rules of the game | base currency is placed randomly in room; human touching the dog ends a round; dog collecting all currency ends a round; buying a "pull" in the shop gives a chance to get either a duplicate of the dog you have or a new dog; money does not expire; dogs have tiers, where better tiers correlate to faster dogs; human speed is static; | dog collecting all base currency spawns 1 premium currency far away from the dog's current location; x premium currency guarantees the next gacha pull will be y rarity; some dogs have abilities that temporarily give a boon to the dog or a bane to the human; human speed increases with basic currency collected and with room progression | all non-basic dogs have abilities; can play as multiple dogs at once and swap perspectives (which also swaps the human's target); human can still eliminate non-focused dog by touching it; human can use an ability that gives the human a boon or the dog(s) a bane; consumable powerups can be bought for an extra boost;  |
| Aesthetics | The game according to the 5 senses | 2D top-down; simple, readable, low-detail sprites; extremely simple menus, guis; background music; "collect" sfx; "win" sfx; "lose" sfx | modestly detailed sprites (2nd or 3rd pass); simple themed menus, guis; splash-stle pop-up art for win, lose, spending premium curency, pulling 1, and pulling more than 1; gacha shop music; purchase sfx; | well-tied-together sprites; fully themed UI and menus; basic animations in splash arts; seamless looping on all background music; rounds have 2 or more background music options to cycle through; ambient-intermittent animal/human noise sfx; |
| Story | The game's dramatic elements | story is implied; "naughty dogs" is the start and end of it | story is static; room setup tells story of family as it is right now; | story is dynamic, with story beats that are tied to player progression; flavor dialogue from the gacha shopkeeper tells a story throughout the game; story beats unlock changes to room obstacles and layouts; |
| Technology | Technology used to create and support the game | godot (GDScript and C#); audacity and manual recordings; royalty free music; firealpaca; | FireAlpaca SE; | *lazy* |

### MDA Elements
| MDA Segment | Definition | MVP Elements | Ideal Elements | Stretch Elements |
|-|-|-|-|-|
| Mechanics | The game experience when it's not played | 1 currency (base); 1 human, 2 dogs; 2 dog power tiers; gacha shop; 1 room (aka map); 3 main obstacles in the room; | 2 currencies (base, premium); several dogs; 3-5 dog power tiers; 3 rooms; room-themed obstacles for each room; | comsumable powerups; many dogs; 5+ tiers and a secret super power tier; 2+ humans; secret shop or mid-round shop; 5+ rooms; 2nd floor in house; non-linear room layout;  |
| Dynamics | The game experience while it's played | game opens to 1-button start menu; game automatically opens gacha shop after round end; can only exit game from gacha shop UI; no save ability; wasd movement of dog; point-and-click menu navigation; pulls bought one-by-one; | game opens to 2-button start menu (start, options); pause menu during round allows round abandonment; one save, always overwritten; game auto-saves before each round; options allows bgm, sfx on/off; options allows save reset; wasd or udlr movement of dog; wasd+enter or udlr+enter or point-and-click menu navigation; single or bulk pulls; gacha shop pause menu has exit, options, always saves before exiting; | rooms procedurally place obstacles so that each round has a slightly different set of layouts; game auto-saves after each pull to prevent save-scumming; game auto-saves before and after each round to prevent save-scumming; several save files; gacha shop pause menu has exit, title, options |
| Aesthetics | The game experience after it's played | ? | ?? | ??? |

---

## The Holding Tank
Ideas, concepts, questions, and propositions that need team review

- [ ] MVP vs Ideal vs Stretch elements
- [ ] Use Godot mobile-compatible renderer? (still able to render on PC)
- [ ] Pixel or 2D?
  - Akiva: after thinking for a bit and working on specs, maybe we try a hybrid? pixel assets are going to be easy to find, but we can use 2D for splash art and menus.

---

## Features to Build

### Phase 1: Paper Prototyping
No code, only messing around on paper or paper-adjacent

- [ ] [Feature or milestone]

### Phase 2: MVP I
This phase is dedicated to getting the game into a playable state, with any non-essentials deferred. 

- [ ] [Feature or milestone]
- [ ] [Feature or milestone]

### Phase 2: MVP II
Nothing in this phase is negotiable. Without a single piece, the game is incomplete

- [ ] [Feature or milestone]
- [ ] [Feature or milestone]

### Phase 4: Bonus Round!

- [ ] [Feature or milestone]
- [ ] [Feature or milestone]

---

## Success Metrics

### Phase 1

- [ ] Tests: [number] passing
- [ ] Coverage: [target]%
- [ ] [Other metrics]

---

## Reference Links

### Project Documentation

- [README.md](README.md) - Project overview
- [CLAUDE.md](CLAUDE.md) - Claude Code auto-read config
- [documents/docs/PROJECT-CONTEXT.md](documents/docs/PROJECT-CONTEXT.md) - AI agent context
- [documents/docs/FILE-ORGANIZATION.md](documents/docs/FILE-ORGANIZATION.md) - File placement rules
- [documents/resources/FOR-CONTRIBUTORS.md](documents/resources/FOR-CONTRIBUTORS.md) - Contributor reference guide
- [documents/ai-usage/policies/HOMEWORK-MANIFESTO.md](documents/ai-usage/policies/HOMEWORK-MANIFESTO.md) - Code authorship rules

---

## Update Log

### 2026-04-01 @ 09:23

- Human created project
- Human updated AI files