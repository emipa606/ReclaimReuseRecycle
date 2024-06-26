﻿using RimWorld;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

internal class StatPart_Reclaimed : StatPart
{
    public const float factorNonSterile = 0.65f;
    public const float factorMangled = 0.35f;


    private static float? GetValueMultiplier(PackedThingDef def)
    {
        switch (def?.ReclamationType)
        {
            case ReclamationType.NonSterile:
                return factorNonSterile;
            case ReclamationType.Mangled:
                return factorMangled;
            case null:
                return null;
            default:
                Util.Error($"Unexpected reclamation type: {def.ReclamationType}.");
                return null;
        }
    }


    public override void TransformValue(StatRequest req, ref float val)
    {
        if (!req.HasThing)
        {
            return;
        }

        var mul = GetValueMultiplier((req.Thing as PackedThing)?.packedDef);
        if (mul != null)
        {
            val *= mul.Value;
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        if (!req.HasThing)
        {
            return null;
        }

        var mul = GetValueMultiplier((req.Thing as PackedThing)?.packedDef);
        if (mul != null)
        {
            return LanguageKeys.r3.R3_StatsReport_ReclamationMultiplier.Translate() + ": x" +
                   mul.Value.ToStringPercent();
        }

        return null;
    }
}