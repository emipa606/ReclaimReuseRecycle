using System.Collections.Generic;
using RimWorld;

namespace DoctorVanGogh.ReclaimReuseRecycle
{
    internal class Building_R3WorkTable : Building_WorkTable
    {
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            foreach (var displayStat in base.SpecialDisplayStats())
            {
                yield return displayStat;
            }


            yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.MedicalSurgerySuccessChance);
            // TODO: mechanoids
            //yield return new StatDrawEntry(StatCategoryDefOf.Building, StatDefOf.MedicalSurgerySuccessChance);
        }
    }
}