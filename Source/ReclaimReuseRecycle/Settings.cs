using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

internal class Settings : ModSettings
{
    private static readonly FloatRange Default_NonSterile = new(0.85f, 1f);
    private static readonly FloatRange Default_Mangled = new(0.5f, .85f);

    public static FloatRange NonSterileRange = Default_NonSterile;
    public static FloatRange MangledRange = Default_Mangled;


    public override void ExposeData()
    {
        base.ExposeData();

        Scribe_Values.Look(ref NonSterileRange, nameof(NonSterileRange), Default_NonSterile);
        Scribe_Values.Look(ref MangledRange, nameof(MangledRange), Default_Mangled);
    }
}