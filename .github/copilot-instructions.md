# GitHub Copilot Instructions for "Reclaim, Reuse, Recycle (Continued)" RimWorld Mod

## Mod Overview and Purpose

"Reclaim, Reuse, Recycle (Continued)" is a mod for RimWorld that allows players to harvest implants and bionics from corpses and prepare them for reuse or sale. This feature enriches the gameplay by introducing a more sophisticated recycling system and adds depth to the management of resources.

## Key Features and Systems

- **Harvesting and Refurbishment**: Introduces a `Harvesting bench` and a `refurbishment bench`. Corpses can be harvested for artificial parts that can be refurbished and used by colonists.
- **Complexity Tiers**: Classifies parts into three complexity tiers - primitive, advanced, and glittertech, corresponding to different tech levels. All mod-added parts are supported.
- **Research Integration**: The ability to harvest and refurbish parts is gated by tiered research projects, adding a strategic layer to the gameplay.
- **Damage-Based Reclamation**: Parts can be reclaimed as 'Non-Sterile', 'Mangled', or not at all, based on their condition. The reclamation thresholds can be customized in the mod settings.
- **Risk of Failure**: Harvesting parts can fail similar to medical operations, depending on the skill level of the colonist performing the operation.

## Coding Patterns and Conventions

- **Naming Conventions**: Classes and methods use PascalCase, while parameters and variables use camelCase.
- **Static Utilities**: Frequently used functionalities are encapsulated in static classes, such as `HarvestUtility` and `Util`.
- **Inheritance and Interfaces**: Uses inheritance to create specialized filter classes such as `Filter_Corpse`, and implements interfaces like `IComparer` for custom sorting logic.

## XML Integration

- The mod relies on XML for defining many of its data structures, such as `ThingDefs`, which are essential for integration with RimWorld's modding framework. Make sure XML files correctly define necessary elements for parts and recipes.

## Harmony Patching

- The mod uses Harmony for patching RimWorld's methods to extend or modify the base game behavior, particularly for tasks like harvesting operations and failure handling.
- Ensure to correctly identify methods to patch and use appropriate Harmony annotations to avoid conflicts with other mods.

## Suggestions for Copilot

- **Auto-completion of Class Definitions**: When writing new classes, use Copilot to suggest base class methods and properties that need to be overridden or implemented.
- **Pattern Recognition**: Allow Copilot to recognize coding patterns, especially for repetitive structures in filter and utility classes.
- **XML Structure Handling**: Suggest common XML structure needs for defining new items or modifying existing game elements.
- **Harmony Annotations**: Use Copilot to recall proper syntax and practices for Harmony patches.
- **Complex Logic Assistance**: Seek suggestions for complex conditionals, especially pertaining to part reclamation logic and tier classification.

By following these detailed instructions, developers can effectively use GitHub Copilot to augment their modding workflow, creating a robust and integrated mod for RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
