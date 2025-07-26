using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

public class Filter_Harvested() : Filter_Corpse(null)
{
    public override bool CanEverMatch(ThingDef def)
    {
        return def.IsWithinCategory(ThingCategoryDefOf.Corpses);
    }

    public override bool Matches(Thing t)
    {
        return CanEverMatch(t.def) && base.Matches(t);
    }

    protected override bool DoesMatch(Corpse corpse)
    {
        return corpse == null || base.DoesMatch(corpse);
    }
}