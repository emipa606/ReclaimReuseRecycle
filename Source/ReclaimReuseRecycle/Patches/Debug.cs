namespace DoctorVanGogh.ReclaimReuseRecycle.Patches;

/*
[HarmonyPatch(typeof(ThingMaker), nameof(ThingMaker.MakeThing))]
class Debug {
    public static void Prefix(ThingDef def, ThingDef stuff) {
        Log.Message($"Make {def?.defName} from {stuff?.defName}");
    }
}
*/