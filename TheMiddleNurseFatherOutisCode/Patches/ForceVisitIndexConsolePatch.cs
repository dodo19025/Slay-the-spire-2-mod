using TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Console;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Patches;

using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
 
[HarmonyPatch(typeof(AncientDialogueSet), nameof(AncientDialogueSet.GetValidDialogues))]
static class ForceVisitIndexConsolePatch
{
    static void Prefix(ref int charVisits, ref int totalVisits)
    {
        if (AncientDebug.ForcedVisitIndex is not { } v) return;
        charVisits = v;
        totalVisits = Math.Max(totalVisits, 1);
        AncientDebug.ForcedVisitIndex = null;   
    }
}