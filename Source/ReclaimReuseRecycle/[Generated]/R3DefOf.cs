﻿using System.Diagnostics.CodeAnalysis;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

[DefOf]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class R3DefOf
{
    public static StatCategoryDef ReclaimedItem;
    public static ThingDef R3_TableHarvesting;

    public static RecipeDef R3_HarvestCorpseFlesh_Primitive;
    public static RecipeDef R3_HarvestCorpseFlesh_Advanced;
    public static RecipeDef R3_HarvestCorpseFlesh_Glittertech;
    public static RecipeDef R3_HarvestCorpseMechanoid_Primitive;
    public static RecipeDef R3_HarvestCorpseMechanoid_Advanced;
    public static RecipeDef R3_HarvestCorpseMechanoid_Glittertech;

    public static RecipeDef R3_Sterilize_Primitive;
    public static RecipeDef R3_Sterilize_Advanced;
    public static RecipeDef R3_Sterilize_Glittertech;
    public static RecipeDef R3_Refurbish_Primitive;
    public static RecipeDef R3_Refurbish_Advanced;
    public static RecipeDef R3_Refurbish_Glittertech;

    public static RecipeDef ButcherCorpseFlesh;
    public static RecipeDef ButcherCorpseMechanoid;

    public static ThingCategoryDef BodyPartsNatural;
    public static ThingCategoryDef BodyPartsReclaimed;
    public static ThingCategoryDef BodyPartsNonSterile;
    public static ThingCategoryDef BodyPartsNonSterile_Primitive;
    public static ThingCategoryDef BodyPartsNonSterile_Advanced;
    public static ThingCategoryDef BodyPartsNonSterile_Glittertech;
    public static ThingCategoryDef BodyPartsMangled;
    public static ThingCategoryDef BodyPartsMangled_Primitive;
    public static ThingCategoryDef BodyPartsMangled_Advanced;
    public static ThingCategoryDef BodyPartsMangled_Glittertech;

    public static SpecialThingFilterDef R3_AllowUnharvested_Primitive;
    public static SpecialThingFilterDef R3_AllowUnharvested_Advanced;
    public static SpecialThingFilterDef R3_AllowUnharvested_Glittertech;
}