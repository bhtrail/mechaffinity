using BattleTech;

namespace MechAffinity.Patches;

[HarmonyPatch(typeof(Mech), "AddInstability")]
class Mech_AddInstability
{
    public static bool Prepare()
    {
        return Main.settings.enableStablePiloting;
    }
    
    public static void Prefix(ref bool __runOriginal, Mech __instance, ref float amt)
    {
        if (!__runOriginal)
        {
            return;
        }
        
        amt *= StablePilotingManager.Instance.getStabilityModifier(__instance.pilot);
    }
}