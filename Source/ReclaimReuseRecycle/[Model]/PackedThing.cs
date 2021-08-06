using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle
{
    public class PackedThing : ThingWithComps
    {
        public PackedThingDef packedDef;

        public ThingDef SpawnOnUnpack => packedDef.SpawnOnUnpack;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            packedDef = def as PackedThingDef;
            if (packedDef == null)
            {
                Util.Error(
                    $"{nameof(PackedThing)}: {nameof(packedDef)} is null - missing a class definition in the xml files?");
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            yield return new StatDrawEntry(
                R3DefOf.ReclaimedItem,
                LanguageKeys.r3.R3_OriginalThing.Translate(),
                SpawnOnUnpack.LabelCap,
                SpawnOnUnpack.description,
                0
            );
            yield return new StatDrawEntry(
                R3DefOf.ReclaimedItem,
                LanguageKeys.r3.R3_Complexity.Translate(),
                packedDef.Complexity.ToString(),
                null,
                0);
        }
    }
}