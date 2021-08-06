using System.Linq;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle
{
    public class RecipeWorker_Harvest : RecipeWorker
    {
        public static RecipeDef[] HarvestFleshRecipes =
        {
            R3DefOf.R3_HarvestCorpseFlesh_Primitive,
            R3DefOf.R3_HarvestCorpseFlesh_Advanced,
            R3DefOf.R3_HarvestCorpseFlesh_Glittertech
        };

        public static RecipeDef[] HarvestMechanoidRecipes =
        {
            R3DefOf.R3_HarvestCorpseMechanoid_Primitive,
            R3DefOf.R3_HarvestCorpseMechanoid_Advanced,
            R3DefOf.R3_HarvestCorpseMechanoid_Glittertech
        };

        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {
            if ((HarvestFleshRecipes.Contains(recipe) || HarvestMechanoidRecipes.Contains(recipe)) &&
                ingredient is Corpse)
            {
                return;
            }

            base.ConsumeIngredient(ingredient, recipe, map);
        }
    }
}