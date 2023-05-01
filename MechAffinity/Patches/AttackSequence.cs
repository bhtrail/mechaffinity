﻿using BattleTech;

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(AttackDirector.AttackSequence), "get_IsBreachingShot")]
class AttackSequence_IsBreachingShot
{
    public static readonly string superBreachingShot = "SuperPrecisionShot";
    public static void Postfix(AttackDirector.AttackSequence __instance, ref bool __result)
    {
        if (!__result && __instance.allSelectedWeapons.Count > 0)
        {
            __result = __instance.attacker.StatCollection.GetValue<bool>(superBreachingShot) && __instance.allSelectedWeapons[0].Type != WeaponType.Melee;
        }
    }
}