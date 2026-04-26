# AI Usage in Dogcha Development

## Overview

Dogcha is a 2D roguelike game built in Godot 4 using C#. Players control a dog navigating through rooms collecting treats while avoiding a human AI. Claude (Anthropic) was used as a development assistant throughout the project.

---

## How AI Was Used

### Conceptual Guidance
AI was used to explain core concepts before implementation, rather than simply generating code. For example, the student was walked through how A* pathfinding works — including G cost, H cost, F cost, and the open/closed list — before any code was written. The student was asked questions to verify understanding at each step.

### Human AI Pathfinding
The most significant use of AI assistance was in developing the human enemy's pathfinding system. Several approaches were explored and iterated on:

- An initial A* waypoint graph implementation was attempted, with the student placing markers manually around furniture and defining connections between them
- After finding the waypoint approach too complex for the room layout, the project pivoted to Godot's built-in `NavigationAgent2D` system
- Navigation mesh setup, obstacle avoidance, and corner handling were debugged iteratively with AI assistance
- A stuck detection system with unstuck markers was developed to handle cases where the human got caught on long obstacles

### Finite State Machine
The human enemy uses a Finite State Machine with states for Chasing, Patrolling, Paused, and Distracted. The student was already familiar with FSMs from a compilers course — AI helped translate that theoretical knowledge into practical Godot/C# implementation.

### Game Systems
AI assisted in implementing several supporting systems:

- **Door progression** — triggering human pathfinding toward doorways when the player advances rooms, including pre-door waypoints to avoid wall collision
- **Difficulty system** — Easy, Normal, and Hard modes with treat multipliers and human speed scaling
- **Dog ability popup** — a cat character that appears on screen displaying ability activation messages
- **Z-index sorting** — dynamic rendering order based on Y position so characters correctly appear in front of and behind furniture
- **Speed scaling** — human speed tied to the selected dog breed's stats

### Debugging
A significant portion of AI usage was debugging — interpreting Godot error messages, fixing node path issues, resolving merge conflicts, and diagnosing why systems weren't behaving as expected.

---

## Student Contributions

- Game concept, design, and room layout
- All scene building, node placement, and navigation mesh drawing in the Godot editor
- Sprite and animation integration
- Waypoint and marker placement for navigation
- Dog breed designs, ability concepts, and naming
- UI layout and visual design
- Git version control and project management
- Final tuning of speeds, difficulty values, and gameplay feel

---

## Reflection

AI was most valuable as a teaching and debugging tool rather than a code generator. The student directed the development, made design decisions, and built the project in the editor — AI provided explanations, caught errors, and helped translate ideas into working code. Many systems went through multiple iterations based on what felt right during playtesting.
