using HarmonyLib;
using RimWorld;

namespace DoctorVanGogh.ReclaimReuseRecycle;

[HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
internal class DefGenerator_GenerateImpliedDefs_PreResolve
{
    public static void Prefix()
    {
        foreach (var def in ThingDefGenerator_Reclaimed.ImpliedReclaimableDefs())
        {
            DefGenerator.AddImpliedDef(def);
        }
    }
}