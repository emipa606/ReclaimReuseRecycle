using System.Collections.Generic;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

public class HeDiffComparer_AddedPartsThenImplants : IComparer<Hediff>
{
    public int Compare(Hediff x, Hediff y)
    {
        return Rank(x).CompareTo(Rank(y));
    }

    private static int Rank(Hediff value)
    {
        switch (value)
        {
            case null:
                return -2;
            case Hediff_AddedPart:
                return -1;
            case Hediff_Implant:
                return 0;
            default:
                return 1;
        }
    }
}