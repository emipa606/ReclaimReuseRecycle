using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle
{
    public class PackedThingDef : ThingDef
    {
        public ThingDef SpawnOnUnpack { get; set; }

        public ReclamationType? ReclamationType { get; set; }

        public Complexity Complexity { get; set; }
    }
}