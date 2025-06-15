# Factory Simulation Code Generation Instructions

These instructions guide GitHub Copilot Chat to generate code tailored for Industrial Genesis, a factory automation game built in Godot 4.4 with C#.

## Coding Style and Architecture

- Use **Godot 4.4 C#** idiomatic patterns.
- Follow **component-based design**: each machine, conveyor, or system should be a separate component/class.
- Use **signals/events** for communication between components, avoid tight coupling.
- Prefer **modular and reusable code** over monolithic scripts.
- Avoid putting heavy logic inside `_Process()` unless real-time frame updates are absolutely necessary.
- Use **async timers or signals** for processing delays and workflows.
- Add **summary comments** (`///`) on classes and methods describing their purpose briefly.

## Domain-Specific Guidelines

- Machines can have **single or multiple inputs and outputs**; code should reflect flexible input/output handling.
- Conveyors have different tiers (basic, fast, express, ultra); optimize code for different throughput speeds.
- Processing time must be configurable per machine and should not block the main thread.
- Include logic for **research upgrades** affecting speed, efficiency, and automation.
- Blueprint systems should support saving/loading configurations in a scalable manner.
- Code should assume **large-scale factories** with thousands of active entities — optimize for performance and memory usage.
- Implement **error handling** or fallback states for machine breakdowns or resource shortages.

## Testing and Maintenance

- Write code in a way that supports **unit testing** of core logic.
- Maintain **clear variable and method names** for readability.
- Comment complex or non-obvious logic generously.
- Use Godot's **logging** facilities for debug output where needed.

---

This file is part of the Copilot Chat configuration to help generate relevant, high-quality code for Industrial Genesis.
