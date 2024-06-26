﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace DoctorVanGogh.ReclaimReuseRecycle;

public static class HarvestUtility
{
    // <see creg="Recipe_Surgery.CheckSurgeryFail" />
    internal static Thing TryExtractPart(Pawn worker, Corpse corpse, RaceProperties race, HediffSet diffSet,
        string label, BodyPartRecord part, ThingDef spawn)
    {
        var num = 1f;
        //StatDef operationStat = !race.IsMechanoid ? StatDefOf.MedicalSurgerySuccessChance : StatDefOf.MechanoidOperationSuccessChance;
        var operationStat = StatDefOf.MedicalSurgerySuccessChance;

        // basic worker stat
        num *= worker.GetStatValue(operationStat);
        // apply linked workbench offsets
        num *= 1.0f +
            (corpse.Map.thingGrid.ThingAt(corpse.Position, R3DefOf.R3_TableHarvesting) as Building_WorkTable)
            ?.TryGetComp<CompAffectedByFacilities>()?.GetStatOffset(operationStat) ?? 0f;

        if (Rand.Value > num)
        {
            if (Rand.Value < 0.5f)
            {
                if (Rand.Value < 0.1f)
                {
                    Messages.Message(
                        $"{worker.LabelShort} has failed in a ridiculous way while trying to harvest {label}.",
                        MessageTypeDefOf.NegativeEvent);
                    GiveInjuriesOperationFailureRidiculousUnrestricted(corpse.InnerPawn);
                    return null;
                }

                Messages.Message(
                    $"{worker.LabelShort} has failed in a catastrophic way while trying to harvest {label}.",
                    MessageTypeDefOf.NegativeEvent);
                GiveInjuriesOperationFailureCatastrophicUnrestricted(corpse.InnerPawn, part);
                return null;
            }

            Messages.Message($"{worker.LabelShort} has failed in a minor way while trying to harvest {label}.",
                MessageTypeDefOf.NegativeEvent);
            GiveInjuriesOperationFailureMinorUnrestricted(corpse.InnerPawn, part);
        }

        var hpFactor = Util.HitpointsFactor(part, diffSet);

        if (!(hpFactor > 0))
        {
            return null;
        }

        var d = ThingDefGenerator_Reclaimed.GetExtractableDef(spawn, hpFactor);

        if (d == null)
        {
            return null;
        }

        FloatRange r;
        switch (d.ReclamationType)
        {
            case ReclamationType.NonSterile:
                r = Settings.NonSterileRange;
                break;
            case ReclamationType.Mangled:
                r = Settings.MangledRange;
                break;
            default:
                Util.Warning(
                    $"Got def {d.defName} with unknown reclamation type {d.ReclamationType} - not extracting.");
                return null;
        }

        var result = Util.CreateThing(d, GenMath.LerpDouble(r.min, r.max, 0.5f, 1f, hpFactor));
        corpse.InnerPawn.health.AddHediff(HediffDefOf.MissingBodyPart, part);

        return result;
    }

    public static void GiveInjuriesOperationFailureMinorUnrestricted(Pawn p, BodyPartRecord part)
    {
        GiveRandomSurgeryInjuriesUnrestricted(p, 20, part);
    }

    public static void GiveInjuriesOperationFailureCatastrophicUnrestricted(Pawn p, BodyPartRecord part)
    {
        GiveRandomSurgeryInjuriesUnrestricted(p, 65, part);
    }

    public static void GiveInjuriesOperationFailureRidiculousUnrestricted(Pawn p)
    {
        GiveRandomSurgeryInjuriesUnrestricted(p, 90, null);
    }

    public static void GiveRandomSurgeryInjuriesUnrestricted(Pawn p, int totalDamage, BodyPartRecord part)
    {
        var targets = p.health.hediffSet.GetNotMissingParts();

        if (part != null)
        {
            targets = targets.Where(pa => pa == part || pa.parent == part || part.parent == pa);
        }

        var bodyPartRecords = targets.ToList();

        var start = DateTime.Now;

        while (totalDamage > 0 && bodyPartRecords.Count != 0)
        {
            if ((DateTime.Now - start).TotalSeconds > 2)
            {
                return;
            }

            /* HACK:
             * workaround for case with totally everything destroyed in this corpse except
             * "Waist" which has a coverage of 0% which makes GetRandomElementByWeight blow
             * because.... .... *duh*(?)
             */
            if (!bodyPartRecords.TryRandomElementByWeight(x => x.coverageAbs, out var bodyPartRecord))
            {
                Util.Trace($"Trying to apply {totalDamage} damage - no valid part found. Skipping.");
                return;
            }

            var partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);

            Util.Trace(
                $"Trying to apply {totalDamage} damage - current Target: {bodyPartRecord.def.LabelCap} ({partHealth:n0} HP)");

            if (partHealth < float.Epsilon)
            {
                bodyPartRecords.Remove(bodyPartRecord);
                continue;
            }

            var num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));

            if (num <= 0)
            {
                break;
            }

            var def = Rand.Element(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);

            var hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(def, p, bodyPartRecord);

            var injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, p);
            injury.Part = bodyPartRecord;
            injury.Severity = num;

            p.health.AddHediff(injury, null, new DamageInfo(def, num, -1f, -1, null, part));
            GenLeaving.DropFilthDueToDamage(p, num);

            totalDamage -= num;
        }
    }


    public static IEnumerable<PackedThingDef> GetExtractableThings(Corpse corpse)
    {
        if (corpse == null)
        {
            return Enumerable.Empty<PackedThingDef>();
        }

        var race = corpse.InnerPawn.RaceProps;
        var healthTracker = corpse.InnerPawn.health;
        var diffSet = healthTracker.hediffSet;

        if (race.IsMechanoid)
        {
            return Filter_Corpse.GetReclaimablePartsMechanoid(diffSet);
        }

        if (race.Humanlike || race.Animal)
        {
            return Filter_Corpse.GetReclaimablePartsOrganic(diffSet);
        }

        return Enumerable.Empty<PackedThingDef>();
    }
}