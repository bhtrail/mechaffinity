﻿using System;
using BattleTech;
using BattleTech.UI;
using BattleTech.UI.Tooltips;
using MechAffinity.Data;

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(SGBarracksRosterSlot), "Refresh")]
public static class SGBarracksRosterSlot_Refresh_Patch
{
    public static void Postfix(SGBarracksRosterSlot __instance)
    {
        if (__instance.Pilot == null)
            return;

        HBSTooltip tooltip = __instance.gameObject.GetComponent<HBSTooltip>() ?? __instance.gameObject.AddComponent<HBSTooltip>();

        Pilot pilot = __instance.Pilot;
        string Desc = tooltip.GetText();
        if (String.IsNullOrEmpty(Desc))
        {
            Desc = "";
        }
        
        foreach (PilotTooltipTag pqTag in Main.settings.quirkSettings.tooltipTags)
        {
            if (pilot.pilotDef.PilotTags.Contains(pqTag.tag))
            {
                Desc += $"{pqTag.tooltipText}\n\n";
            }
        }

        PilotIcon pilotIcon = PilotUiManager.Instance.GetPilotIcon(pilot);

        if (pilotIcon != null)
        {
            if (pilotIcon.HasColour())
            {
                Main.modLog.Debug?.Write("Setting Pilot Icon Colour!");
                __instance.pilotTypeBackground.SetUIColor(UIColor.Custom);
                __instance.pilotTypeBackground.OverrideWithColor(pilotIcon.GetColor());
            }
            
        }

        if (Main.settings.enablePilotQuirks)
        {
            Desc += PilotQuirkManager.Instance.getPilotToolTip(pilot);
        }

        if (Main.settings.enablePilotAffinity)
        {
            Desc += "<b>Pilot Affinities:</b>\n\n";
            Desc += PilotAffinityManager.Instance.getPilotToolTip(pilot);
        }

        var descriptionDef = new BaseDescriptionDef("Tags", pilot.Callsign, Desc, null);
        tooltip.SetDefaultStateData(TooltipUtilities.GetStateDataFromObject(descriptionDef));
    }
}
