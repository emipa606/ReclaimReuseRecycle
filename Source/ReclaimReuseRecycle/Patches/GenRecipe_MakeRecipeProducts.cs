using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle
{
    [HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
    public class GenRecipe_MakeRecipeProducts
    {
        private static readonly Thing[] Empty = new Thing[0];

        private static readonly RecipeDef[] ReclamationRecipes =
        {
            R3DefOf.R3_Refurbish_Primitive,
            R3DefOf.R3_Refurbish_Advanced,
            R3DefOf.R3_Refurbish_Glittertech,
            R3DefOf.R3_Sterilize_Primitive,
            R3DefOf.R3_Sterilize_Advanced,
            R3DefOf.R3_Sterilize_Glittertech
        };

        public static void Postfix(ref IEnumerable<Thing> __result, RecipeDef recipeDef, Pawn worker,
            List<Thing> ingredients, Thing dominantIngredient)
        {
            if (RecipeWorker_Harvest.HarvestFleshRecipes.Contains(recipeDef))
            {
                var result = new List<Thing>(__result ?? Empty);

                if (!(dominantIngredient is Corpse corpse))
                {
                    Log.Warning("Harvesting without a corpse???");
                    return;
                }

                var race = corpse.InnerPawn.RaceProps;
                var healthTracker = corpse.InnerPawn.health;
                var diffSet = healthTracker.hediffSet;

                foreach (var hediff in diffSet.hediffs.Where(d => d is Hediff_Implant)
                    .Where(d => d.def.spawnThingOnRemoved != null).ToArray())
                {
                    var thing = HarvestUtility.TryExtractPart(worker, corpse, race, diffSet, hediff.Label, hediff.Part,
                        hediff.def.spawnThingOnRemoved);
                    if (thing != null)
                    {
                        result.Add(thing);
                    }
                }

                __result = result;
            }
            else if (RecipeWorker_Harvest.HarvestMechanoidRecipes.Contains(recipeDef))
            {
                var result = new List<Thing>(__result ?? Empty);

                if (!(dominantIngredient is Corpse corpse))
                {
                    Log.Warning("Harvesting without a corpse???");
                    return;
                }

                var race = corpse.InnerPawn.RaceProps;
                var healthTracker = corpse.InnerPawn.health;
                var diffSet = healthTracker.hediffSet;

                foreach (var bpr in diffSet.GetNotMissingParts().Where(bpr => bpr.def.spawnThingOnRemoved != null))
                {
                    var thing = HarvestUtility.TryExtractPart(worker, corpse, race, diffSet, bpr.def.LabelCap, bpr,
                        bpr.def.spawnThingOnRemoved);
                    if (thing != null)
                    {
                        result.Add(thing);
                    }
                }

                __result = result;
            }
            else if (ReclamationRecipes.Contains(recipeDef))
            {
                var result = new List<Thing>(__result ?? Empty);

                var reclaimedThing = ingredients.OfType<PackedThing>().First();

                result.Add(ThingMaker.MakeThing(reclaimedThing.SpawnOnUnpack));

                __result = result;
            }
        }
    }
}