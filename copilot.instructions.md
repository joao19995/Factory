# Industrial Genesis Code Generation Template

Use this template when I give you a CSV row with class info.

## CSV Format

Category,Filename,ClassRole,Brief Description,Primary Behavior,Secondary Behavior,Tertiary Behavior,ClassName,BaseClass,Priority,Dependencies

## Class Template (Godot 4.4 C#)

Replace the placeholders using the CSV row I give you.

```csharp
// ========================================
// Project: Industrial Genesis (Godot 4.4)
// Category: {Category}
// File: {Filename}.cs
// Role: {ClassRole} – {Brief Description}
// Priority: {Priority}
// Dependencies: {Dependencies}
// Architecture: Component-based, modular, signal-driven
// Key Behaviors:
// 1. {Primary Behavior}
// 2. {Secondary Behavior}
// 3. {Tertiary Behavior}
// Guidelines:
// - Use clean, idiomatic Godot C#
// - Use signals for communication
// - Avoid logic inside _Process unless it's time-sensitive
// - Comment all key methods and purpose-driven logic
// ========================================

using Godot;
using System;

public partial class {class_name} : {node_type}
{
    ${0} // Your code goes here
}
When given a single CSV row, substitute each field into the corresponding {placeholder} and generate a full Godot C# class using best practices.

---

### ✅ `settings.json` (include the instruction)

```json
"github.copilot.chat.codeGeneration.instructions": [
  { "file": "copilot.instructions.md" },
  { "file": "factory_simulation.instructions.md" }
]