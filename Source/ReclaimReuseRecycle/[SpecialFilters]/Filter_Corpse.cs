using System.Collections.Generic;
using System.Linq;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

public abstract class Filter_Corpse : SpecialThingFilterWorker
{
    private readonly Complexity? _complexity;

    protected Filter_Corpse(Complexity? complexity)
    {
        _complexity = complexity;
    }

    public override bool Matches(Thing t)
    {
        return DoesMatch(t as Corpse);
    }

    public override bool AlwaysMatches(ThingDef def)
    {
        return false;
    }

    public override bool CanEverMatch(ThingDef def)
    {
        return def.IsCorpse;
    }

    protected virtual bool DoesMatch(Corpse corpse)
    {
        if (corpse == null)
        {
            return false;
        }

        var race = corpse.InnerPawn.RaceProps;
        var healthTracker = corpse.InnerPawn.health;
        var diffSet = healthTracker.hediffSet;


        if (_complexity == null)
        {
            return race.IsMechanoid && !GetReclaimablePartsMechanoid(diffSet).Any()
                   || (race.Humanlike || race.Animal) &&
                   !GetReclaimablePartsOrganic(diffSet).Any();
        }

        return race.IsMechanoid && GetReclaimablePartsMechanoid(diffSet)
                   .Any(pd => pd.Complexity == _complexity)
               || (race.Humanlike || race.Animal) && GetReclaimablePartsOrganic(diffSet)
                   .Any(pd => pd.Complexity == _complexity);
    }

    public static IEnumerable<PackedThingDef> GetReclaimablePartsMechanoid(HediffSet diffSet)
    {
        return diffSet.GetNotMissingParts()
            .Where(bpr => bpr.def.spawnThingOnRemoved != null)
            .Select(bpr =>
                ThingDefGenerator_Reclaimed.GetExtractableDef(bpr.def.spawnThingOnRemoved,
                    Util.HitpointsFactor(bpr, diffSet)))
            .Where(d => d != null);
    }


    public static IEnumerable<PackedThingDef> GetReclaimablePartsOrganic(HediffSet diffSet)
    {
        return diffSet.hediffs
            .Where(d => d is Hediff_Implant && d.def.spawnThingOnRemoved != null)
            .Select(d =>
                ThingDefGenerator_Reclaimed.GetExtractableDef(d.def.spawnThingOnRemoved,
                    Util.HitpointsFactor(d.Part, diffSet)))
            .Where(d => d != null);
    }
}