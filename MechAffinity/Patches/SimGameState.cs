﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BattleTech;
using BattleTech.Save;
using MechAffinity;

namespace MechAffinity.Patches
{

    [HarmonyPatch(typeof(SimGameState), "Rehydrate", typeof(GameInstanceSave))]
    class SimGameState_RehydratePatch
    {
        public static void Postfix(SimGameState __instance, GameInstanceSave gameInstanceSave)
        {
                PilotAffinityManager.Instance.setCompanyStats(__instance.CompanyStats);
        }
    }

    [HarmonyPatch(typeof(SimGameState), "InitCompanyStats")]
    class SimGameState_InitCompanyStatsPatch
    {
        public static void Postfix(SimGameState __instance)
        {

            PilotAffinityManager.Instance.setCompanyStats(__instance.CompanyStats);
        }
    }

    [HarmonyPatch(typeof(SimGameState), "ResolveCompleteContract")]
    class SimGameState_ResolveCompleteContract
    {
        public static void Prefix(SimGameState __instance)
        {
            if (__instance.CompletedContract != null)
            {
                List<UnitResult> results = __instance.CompletedContract.PlayerUnitResults;
                foreach (UnitResult result in results)
                {
                    PilotAffinityManager.Instance.incrementDeployCountWithMech(result);
                }
            }
        }
    }

}