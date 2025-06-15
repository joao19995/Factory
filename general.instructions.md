# General Code Generation Instructions for GitHub Copilot Chat

These guidelines help maintain consistency, clarity, and quality across all generated code.

## Coding Style

- Follow the official language style guides (C# in this project).
- Use clear, descriptive variable and method names.
- Keep functions and methods concise — ideally under 30 lines.
- Use PascalCase for class names and methods, camelCase for local variables and parameters.
- Include comments for any non-trivial logic or algorithms.
- Prefer immutability where applicable to reduce side effects.

## Architecture & Design

- Write modular, reusable code with single responsibility.
- Use interfaces and abstract classes when appropriate to enable extensibility.
- Follow SOLID principles.
- Favor composition over inheritance.
- Avoid global state unless explicitly required (e.g., singletons for managers).

## Error Handling & Debugging

- Use exceptions for error cases and handle them gracefully.
- Validate inputs and provide clear error messages.
- Use logging and debug statements to aid troubleshooting but avoid excessive logging.

## Performance & Scalability

- Write efficient code that scales well with data size.
- Avoid unnecessary allocations and copying.
- Use async/await patterns where applicable.
- Cache expensive computations if results do not change frequently.

## Testing & Maintenance

- Write code with testability in mind.
- Separate concerns to make unit testing easier.
- Keep dependencies minimal and explicit.
- Refactor regularly to improve readability and performance.

## Documentation

- Use XML doc comments (`///`) for public APIs.
- Maintain updated README and inline comments.
- Document any assumptions or side effects.

---

This file is included in GitHub Copilot Chat settings to ensure consistent, high-quality generated code.
