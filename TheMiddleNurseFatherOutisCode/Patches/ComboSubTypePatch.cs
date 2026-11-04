using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheMiddleNurseFatherOutis.TheMiddleNurseFatherOutisCode.Patches;


[HarmonyPatch(typeof(NCard), "UpdateTypePlaque")]
public class ComboSubTypePatch
{
    [HarmonyPostfix]
    public static void ComboSubTypePostfix(NCard __instance)
    {
        CardModel? cardModel = __instance.Model;
        if (cardModel != null && (cardModel.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch) ||
                                  cardModel.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick) ||
                                  cardModel.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Swing)))
        {
            LocString combostring;
            if (cardModel.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Punch))
            {
                combostring = new LocString("gameplay_ui", "THEMIDDLENURSEFATHEROUTIS-CARD_TYPE.PUNCH");
            }
            else if (cardModel.Keywords.Contains(TheMiddleNurseFatherOutisKeywords.Kick))
            {
                combostring = new LocString("gameplay_ui", "THEMIDDLENURSEFATHEROUTIS-CARD_TYPE.KICK");
            }
            else
            {
                combostring = new LocString("gameplay_ui", "THEMIDDLENURSEFATHEROUTIS-CARD_TYPE.SWING");
            }
            combostring.Add("Type",cardModel.Type.ToLocString());
            __instance._typeLabel.SetTextAutoSize(combostring.GetFormattedText());
            Callable.From(new Action(__instance.UpdateTypePlaqueSizeAndPosition)).CallDeferred();
        }
    }
}
