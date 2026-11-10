# GitHub Copilot Instructions for Reclaim, Reuse, Recycle (Continued) Mod

## Mod Overview and Purpose

**Reclaim, Reuse, Recycle (Continued)** is an update to DoctorVanGogh's original Reclaim, Reuse, Recycle mod. It introduces a new "Harvesting bench" with a job for harvesting corpses for artificial parts. These parts can then be refurbished at a "refurbishment bench," allowing colonists to reuse or sell them. The mod integrates with the game's research system, separating parts into three complexity tiers: primitive, advanced, and glittertech. It also introduces filters to the stockpile and recipe ingredients for corpses, and modifies the vanilla "butcher corpse" recipe to exclude corpses with parts.

## Key Features and Systems

- **Harvesting Bench**: A new building where colonists can harvest artificial parts from corpses.
- **Refurbishment Bench**: A building where harvested parts can be refurbished for reuse.
- **Research Integration**: Part harvesting and refurbishment are gated by tiered research projects.
- **Part Complexity**: Parts are categorized into three complexity tiers: primitive, advanced, and glittertech.
- **Sterility and Mangled Parts**: Harvested parts can be reclaimed as 'Non-Sterile' or 'Mangled' depending on their damage state.
- **Special Filters**: Filters added to the stockpile and recipe ingredients for corpses.
- **Modified Vanilla Recipe**: The vanilla "butcher corpse" recipe is modified to exclude corpses with parts.

## Coding Patterns and Conventions

- **Modding Framework**: The mod is built using the RimWorld modding framework, which follows standard C# practices.
- **Harmony Patching**: The mod uses Harmony patching to modify vanilla game behavior. Ensure that you understand Harmony's mechanics and conventions.
- **XML Definitions**: XML files are used to define new recipes, research projects, special thing filters, and stat categories.
- **Model Classes**: Model classes are used to represent complex data structures, such as part complexity and reclamation types.
- **Building Classes**: Building classes extend the base building class to include custom functionality, such as special display stats.

## XML Integration

XML files are used to define new game elements such as recipes, research projects, special thing filters, and stat categories. Key XML files in the mod include:

- **RecipeDefs**: Defines the recipes for harvesting and refurbishing parts.
- **ResearchProjectDefs**: Defines the research projects required to unlock part harvesting and refurbishment.
- **SpecialThingFilterDefs**: Defines filters for the stockpile and recipe ingredients.
- **StatDefs**: Defines new stat categories and related stats.

### Example XML Definition

xml
<RecipeDef>
  <label>Harvest Non-Sterile Part</label>
  <description>Harvest a non-sterile part from a corpse.</description>
  <workAmount>100</workAmount>
  <researchPrerequisite>ResearchHarvesting</researchPrerequisite>
  <products>
    <li>
      <thingDef>NonSterilePart</thingDef>
      <count>1</count>
    </li>
  </products>
  <ingredients>
    <li>
      <thingDef>Corpse</thingDef>
      <count>1</count>
    </li>
    <li>
      <thingDef>Medicine</thingDef>
      <count>1</count>
    </li>
  </ingredients>
</RecipeDef>


## Harmony Patching

Harmony patching is used to modify vanilla game behavior. Ensure that you understand Harmony's mechanics and conventions. Key harmony patches in the mod include:

- **Corpse_SpecialDisplayStats**: Patches the `SpecialDisplayStats` method of the `Corpse` class to include additional display stats for harvested parts.
- **Building_R3WorkTable**: Patches the `SpecialDisplayStats` method of the `Building_R3WorkTable` class to include additional display stats for the harvesting and refurbishment benches.

### Example Harmony Patch

csharp
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace ReclaimReuseRecycle.Patches
{
    [HarmonyPatch(typeof(Corpse), "SpecialDisplayStats", MethodType.Getter)]
    public static class Corpse_SpecialDisplayStats
    {
        public static void Postfix(Corpse __instance, ref IEnumerable<StatDrawEntry> __result)
        {
            var parts = __instance.InnerPawn.health.hediffSet.GetHediffs<Hediff_AddedPart>();
            if (parts.Count > 0)
            {
                foreach (var part in parts)
                {
                    var stat = new StatDrawEntry(StatCategoryDefOf.Basics, part.LabelCap, part.Severity.ToString("0.00"), null, 500, false);
                    __result = __result.Append(stat);
                }
            }
        }
    }
}


## Suggestions for Copilot

- **Familiarize Yourself with Harmony**: Ensure that you understand Harmony's mechanics and conventions to effectively use Copilot for patching vanilla game behavior.
- **Understand XML Definitions**: XML files are used to define new game elements. Ensure that you understand the structure and purpose of each XML file.
- **Review Existing Code**: Review existing C# and XML code to understand the mod's coding patterns and conventions.
- **Use Semantic Summary**: Use the semantic summary to quickly identify key classes, methods, and files in the mod.
- **Provide Detailed Instructions**: Provide detailed instructions to Copilot, including the context and purpose of the code you are working on.

By following these guidelines and suggestions, you can effectively use GitHub Copilot to assist with developing the Reclaim, Reuse, Recycle (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

