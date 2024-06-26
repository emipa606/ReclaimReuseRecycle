using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

public static class ThingDefGenerator_Reclaimed
{
    public static readonly string NonSterileDefNameFormat = @"NonSterile_{0}";

    public static readonly string MangledDefNameFormat = @"Mangled_{0}";

    public static readonly Color MangledColor = new Color(1f, 1f, 0.67f, 1f);
    public static readonly Color NonSterileColor = new Color(0.67f, 1f, 0.67f, 1f);

    public static readonly string MangledColorHex = MangledColor.ToHexColor();
    public static readonly string NonSterileColorHex = NonSterileColor.ToHexColor();


    private static readonly Type[] validHediffs = [typeof(Hediff_AddedPart), typeof(Hediff_Implant)];

    public static Dictionary<ThingDef, PackedThingDef[]> LookupCache;

    public static ModContentPack contentPack;


    [DebuggerHidden]
    public static IEnumerable<ThingDef> ImpliedReclaimableDefs()
    {
        // can't go ThingDef => isBodyPartOrImplant=true because vanilla "WoodLog" counts as BodyPart.... hrmpf
        var thingDefs = DefDatabase<HediffDef>.AllDefs
            .Where(d => validHediffs.Contains(d.hediffClass)
                        && d.spawnThingOnRemoved != null
                        && true != d.spawnThingOnRemoved.thingCategories?.Contains(R3DefOf.BodyPartsNatural))
            .Select(d => d.spawnThingOnRemoved)
            .Distinct();
        // prepare recipes/research to have researchproject's techlevel as fallback values
        var recipeThings = DefDatabase<RecipeDef>.AllDefs
            .SelectMany(
                rcd => rcd.products
                    ?.Select(tcc => tcc.thingDef)
                    .Select(td => new { ResearchTechLevel = rcd.researchPrerequisite?.techLevel, Def = td }))
            .GroupBy(t => t.Def)
            .Select(g => new
            {
                Def = g.Key,
                MinResearchTechLevel = g.Where(t => t.ResearchTechLevel != TechLevel.Undefined)
                    .Min(t => t.ResearchTechLevel)
            }); // 'undefined' level doesn't help - strip it out                                                          

        // left outer join hediff things onto research/recipe things
        var candidates = from td in thingDefs
            join rt in recipeThings
                on td equals rt.Def into comb
            from x in comb.DefaultIfEmpty()
            select new
            {
                Def = td,
                x?.MinResearchTechLevel
            };

        contentPack = LoadedModManager.GetMod<R3Mod>().ContentPack;

        foreach (var t in candidates.ToList())
        {
            yield return GenerateImpliedNonSterileDef(t.Def, t.MinResearchTechLevel);
            yield return GenerateImpliedMangledDef(t.Def, t.MinResearchTechLevel);
        }
    }

    private static PackedThingDef GenerateImpliedNonSterileDef(ThingDef t, TechLevel? researchTechLevel = null)
    {
        return GenerateImpliedPackedDef(
            t,
            NonSterileDefNameFormat,
            LanguageKeys.r3.R3_NonSterile_Description,
            NonSterileColor,
            ReclamationType.NonSterile,
            researchTechLevel);
    }

    private static PackedThingDef GenerateImpliedMangledDef(ThingDef t, TechLevel? researchTechLevel = null)
    {
        return GenerateImpliedPackedDef(
            t,
            MangledDefNameFormat,
            LanguageKeys.r3.R3_Mangled_Description,
            MangledColor,
            ReclamationType.Mangled,
            researchTechLevel);
    }

    private static PackedThingDef GenerateImpliedPackedDef(ThingDef t, string defFormat, string descriptionKey,
        Color color,
        ReclamationType type, TechLevel? researchTechlevel = null)
    {
        var d = new PackedThingDef
        {
            thingClass = typeof(PackedThing),
            defName = string.Format(CultureInfo.InvariantCulture, defFormat, t.defName),
            label = LanguageKeys.r3.R3_Label.Translate(t.LabelCap, color.ToHexColor(), type.Translate()),
            description = descriptionKey.Translate(t.LabelCap),
            graphicData = new GraphicData
            {
                texPath = "Things/Item/Health/HealthItem",
                color = color,
                graphicClass = typeof(Graphic_Single)
            },
            category = ThingCategory.Item,
            useHitPoints = true,
            selectable = true,
            altitudeLayer = AltitudeLayer.Item,
            tickerType = TickerType.Never,
            alwaysHaulable = true,
            tradeTags = [],
            comps = [new CompProperties_Forbiddable()],
            thingCategories = [],
            pathCost = 10,
            techHediffsTags = t.techHediffsTags != null ? [..t.techHediffsTags] : null,
            statBases = [],
            //SpawnOnUnpack = t,
            ReclamationType = type,

            modContentPack = contentPack
        };
        d.SetStatBaseValue(StatDefOf.MaxHitPoints, 50f);
        d.SetStatBaseValue(StatDefOf.DeteriorationRate, 2f);
        d.SetStatBaseValue(StatDefOf.Beauty, -8f);

        float? marketValue = null;

        if (t.statBases.StatListContains(StatDefOf.MarketValue))
        {
            marketValue = t.statBases.GetStatValueFromList(StatDefOf.MarketValue, 0f);
            d.SetStatBaseValue(StatDefOf.MarketValue, marketValue.Value);
        }

        d.SetStatBaseValue(StatDefOf.Mass, t.statBases.GetStatValueFromList(StatDefOf.Mass, 0.2f));
        d.Complexity = GetComplexity(d, marketValue, d.techLevel, researchTechlevel);

        DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories,
            GetThingCategoryDef(t, d.Complexity, type).defName, mayRequireMod: R3Mod.PackageId);

        d.SpawnOnUnpack = t;

        return d;
    }

    private static ThingCategoryDef GetThingCategoryDef(ThingDef t, Complexity complexity, ReclamationType type)
    {
        switch (type)
        {
            case ReclamationType.NonSterile:
                switch (complexity)
                {
                    case Complexity.Primitive:
                        return R3DefOf.BodyPartsNonSterile_Primitive;
                    case Complexity.Advanced:
                        return R3DefOf.BodyPartsNonSterile_Advanced;
                    case Complexity.Glittertech:
                        return R3DefOf.BodyPartsNonSterile_Glittertech;
                    default:
                        Util.Warning($"Unknown complexity {complexity} used for {t.LabelCap}.");
                        return R3DefOf.BodyPartsNonSterile;
                }
            case ReclamationType.Mangled:
                switch (complexity)
                {
                    case Complexity.Primitive:
                        return R3DefOf.BodyPartsMangled_Primitive;
                    case Complexity.Advanced:
                        return R3DefOf.BodyPartsMangled_Advanced;
                    case Complexity.Glittertech:
                        return R3DefOf.BodyPartsMangled_Glittertech;
                    default:
                        Util.Warning($"Unknown complexity {complexity} used for {t.LabelCap}.");
                        return R3DefOf.BodyPartsMangled;
                }
            default:
                Util.Warning($"Unknown reclamation type {type} used for {t.LabelCap}.");
                return R3DefOf.BodyPartsReclaimed;
        }
    }

    private static Complexity GetComplexity(ThingDef d, float? value, params TechLevel?[] techLevels)
    {
        if (d.techHediffsTags?.Contains("Advanced") == true) // ex: vanilla bionics
        {
            return Complexity.Glittertech;
        }

        if (d.techHediffsTags?.Contains("Simple") == true) // ex: vanilla 'simple' prosthetics
        {
            return Complexity.Advanced;
        }

        if (d.techHediffsTags?.Contains("Poor") == true) // ex: vanilla peg legs/denture
        {
            return Complexity.Primitive;
        }

        var tech = (TechLevel?)techLevels.Where(tl => tl != TechLevel.Undefined).Cast<byte?>()
            .Min(); // min non 'undefined' techlevel

        switch (tech)
        {
            case TechLevel.Animal:
            case TechLevel.Neolithic:
            case TechLevel.Medieval:
                return Complexity.Primitive;
            case TechLevel.Industrial:
            case TechLevel.Spacer:
                return Complexity.Advanced;
            case TechLevel.Ultra:
            case TechLevel.Archotech:
                return Complexity.Glittertech;
            default:
                switch (value)
                {
                    case null:
                        try
                        {
                            var work = d.GetStatValueAbstract(StatDefOf.WorkToMake);
                            if (work >= 7500)
                            {
                                // EPOE synthetic, bionic & advanced
                                return Complexity.Glittertech;
                            }

                            if (work >= 4000)
                            {
                                // EPOE simple & surrogate
                                return Complexity.Advanced;
                            }

                            if (work > 0)
                            {
                                // EPOE basic
                                return Complexity.Primitive;
                            }
                        }
                        catch
                        {
                            // ignored
                        }

                        Util.Warning(
                            $"{d.LabelCap} has no discernable or undefined techlevel, no techHediffsTags, no market value and no production work cost - defaulting to max Complexity.");
                        return Complexity.Glittertech;
                    // vanilla power claw price
                    case >= 1500:
                        return Complexity.Glittertech;
                    // vanilla simple prosthetics price
                    case >= 400:
                        return Complexity.Advanced;
                    default:
                        return Complexity.Primitive;
                }
        }
    }

    internal static PackedThingDef GetExtractableDef(ThingDef def, float hitpointsFactor)
    {
        ReclamationType? t = null;

        if (Settings.NonSterileRange.IncludesEpsilon(hitpointsFactor))
        {
            t = ReclamationType.NonSterile;
        }
        else if (Settings.MangledRange.IncludesEpsilon(hitpointsFactor))
        {
            t = ReclamationType.Mangled;
        }


        if (t == null)
        {
            return null;
        }

        return LookupCache.TryGetValue(def, out var elements)
            ? elements.FirstOrDefault(p => p.ReclamationType == t.Value)
            :
            //Log.Message($"[ReclaimReuseRecycle]: Failed to find {def} in LookupCache");
            null;
    }
}