﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleTech;
using MechAffinity.Data;

namespace MechAffinity
{
    public class BaseEffectManager
    {
        protected bool hasInitialized = false;
        protected List<PilotDelayedEffects> delayedEffectsList = new List<PilotDelayedEffects>();
        private List<AbstractActor> spawnedActors = new List<AbstractActor>();


        protected void resetDelayedEffects()
        {
           delayedEffectsList.Clear();
           spawnedActors.Clear();
        }

        protected void applyStatusEffects(AbstractActor actor, List<EffectData> effects)
        {
            List<PilotDelayedEffects> delayedEffectsFromActor = new List<PilotDelayedEffects>();
            foreach (EffectData statusEffect in effects)
            {
                string effectId = $"PassiveEffect_{actor.GUID}_{UidManager.Uid}";
                if (statusEffect.targetingData.effectTriggerType == EffectTriggerType.Passive)
                {
                    switch (statusEffect.targetingData.effectTargetType)
                    {
                        case EffectTargetType.Creator:
                            Main.modLog.LogMessage($"Applying affect {effectId}, effect ID: {statusEffect.Description.Id}, name: {statusEffect.Description.Name} to creator");
                            actor.Combat.EffectManager.CreateEffect(statusEffect, effectId, -1, actor,actor, new WeaponHitInfo(), 0, false);
                            break;
                        case EffectTargetType.AllLanceMates:
                            List<AbstractActor> lancemates =
                                spawnedActors.FindAll((x => x.team == actor.team));
                            foreach (var lancemate in lancemates)
                            {
                                Main.modLog.LogMessage($"Applying Lancemate effect {effectId}, effect ID: {statusEffect.Description.Id}, name: {statusEffect.Description.Name} to {lancemate.DisplayName} ");
                                actor.Combat.EffectManager.CreateEffect(statusEffect, effectId, -1, actor, lancemate,
                                    new WeaponHitInfo(), 0);
                            }
                            delayedEffectsFromActor.Add(new PilotDelayedEffects()
                            {
                                actor = actor,
                                effect = statusEffect,
                                effectTargetType = statusEffect.targetingData.effectTargetType
                            });
                            break;
                        case EffectTargetType.AllEnemies:
                            List<AbstractActor> allEnemies = spawnedActors.FindAll((x => x.IsEnemy(actor)));
                            foreach (var enemy in allEnemies)
                            {
                                Main.modLog.LogMessage($"Applying enemy effect {effectId}, effect ID: {statusEffect.Description.Id}, name: {statusEffect.Description.Name} to {enemy.DisplayName} ");
                                actor.Combat.EffectManager.CreateEffect(statusEffect, effectId, -1, actor, enemy,
                                    new WeaponHitInfo(), 0);
                            }
                            delayedEffectsFromActor.Add(new PilotDelayedEffects()
                            {
                                actor = actor,
                                effect = statusEffect,
                                effectTargetType = statusEffect.targetingData.effectTargetType
                            });
                            break;
                        default:
                            Main.modLog.LogError($"Unable to apply effect {effectId}, effect ID: {statusEffect.Description.Id}, name: {statusEffect.Description.Name} ");
                            break;
                    }
                }
            }
            //ToDo: add any existing delayed effects to this actor that apply
            delayedEffectsList.AddRange(delayedEffectsFromActor);
            spawnedActors.Add(actor);
        }

    }
}
