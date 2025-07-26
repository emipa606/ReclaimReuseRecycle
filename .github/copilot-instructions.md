# GitHub Copilot Instructions for RimWorld Modding Project in C#

## Mod Overview and Purpose

**Mod Name:** Reclaim Reuse Recycle

The "Reclaim Reuse Recycle" mod for RimWorld introduces an immersive recycling system into the game. Its primary purpose is to enhance resource management by allowing players to reclaim materials from used items, harvested corpses, and various other sources. This mod aims to expand gameplay by adding depth to crafting and production processes, encouraging efficient use of resources and strategic planning.

## Key Features and Systems

1. **Recycling Workstations:**
   - Players can build and upgrade special workstations (`Building_R3WorkTable`) to process recyclable materials.
   
2. **Harvest and Reclamation:**
   - New recipes are integrated (`RecipeWorker_Harvest`) to facilitate harvesting and reclaim materials from corpses and other items.
   
3. **Advanced Filtering Systems:**
   - Filter mechanisms (`Filter_Corpse`, `Filter_Harvested`, etc.) are implemented to manage and streamline recyclable items.

4. **Complexity and Stat Calculations:**
   - Introduces new complexity levels (`Complexity.cs`) and statistical evaluation (`StatPart_Reclaimed`) to determine reclaim potential.

5. **Integration with Existing Defs:**
   - The mod seamlessly integrates with existing game definitions, leveraging XML files for ease of patching and extension.

## Coding Patterns and Conventions

- **Naming Conventions:**
  - Classes follow PascalCase, and methods are structured using CamelCase.
  - Internal classes, such as `Building_R3WorkTable`, are suffixed with the object type they extend or relate to.
- **Use of Static Classes and Methods:**
  - Utilitarian functions are placed in static classes like `HarvestUtility`.
  
- **Modular Structure:**
  - Each feature and system is encapsulated (e.g., `InjuryDebug` for injury processing), enhancing code readability and maintainability.

## XML Integration

The mod relies on XML files to define new content and modify existing definitions. This includes:

- **Defining New Recipes and Workbenches:**
  - XML files are used for defining new crafting recipes associated with the `PackedThing` and `PackedThingDef`.
  
- **Stat Modifications and Filtering Rules:**
  - Specific XML configurations manage stat alterations and filtering logic as stipulated in `ThingDefGenerator_Reclaimed`.

## Harmony Patching

- **Tool for Compatibility:**
  - Harmony is used extensively to patch existing game methods without editing the base game code directly, ensuring broad compatibility with other mods.
  
- **Examples of Patched Classes:**
  - `DefGenerator_GenerateImpliedDefs_PostResolve` and `DefGenerator_GenerateImpliedDefs_PreResolve` demonstrate pre- and post-resolution patching to extend game functionalities.

## Suggestions for Copilot

1. **Code Suggestions for Naming:**
   - Encourage Copilot to suggest method names that are descriptive and adhere to the naming conventions outlined (e.g., actions and results in method names).
   
2. **Harmony Patch Templates:**
   - Generate templates for creating new Harmony patches quickly, including usage patterns for prefix and postfix injections.

3. **XML Snippet Generation:**
   - Assist in generating XML snippets for new definitions, ensuring they align with the mod's existing framework.

4. **Refactoring and Optimization:**
   - Suggest optimizations for existing methods to improve performance or reduce redundancy, especially in loops or complex operations.

5. **Test Method Generation:**
   - Auto-generate test methods following mod-specific scenarios to maintain stability and functionality.

This guide provides a comprehensive framework to utilize GitHub Copilot effectively, ensuring consistent and high-quality contributions to the RimWorld modding project.
