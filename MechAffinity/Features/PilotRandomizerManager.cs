﻿using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using MechAffinity.Data;

namespace MechAffinity;

static class ExtensionsClass
{
    private static Random rng = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}

public class PilotRandomizerManager : BaseEffectManager
{
    private static PilotRandomizerManager _instance;

    public static PilotRandomizerManager Instance
    {
        get
        {
            _instance ??= new PilotRandomizerManager();
            return _instance;
        }
    }


    private static List<T> GetRandomSubList<T>(List<T> list, int number)
    {
        var subList = new List<T>();

        if (list.Count <= 0 || number <= 0)
            return subList;

        var randomizeMe = new List<T>();

        randomizeMe.AddRange(list);
        randomizeMe.Shuffle();

        int count = Math.Min(number, randomizeMe.Count);

        for (var i = 0; i < count; i++)
            subList.Add(randomizeMe[i]);

        return subList;
    }

    public void setStartingRonin(SimGameState simGameState)
    {
        Main.modLog.Info?.Write(
            $"PS settings, RL: {Main.pilotSelectSettings.RoninFromList}, RR: {Main.pilotSelectSettings.RandomRonin}, PP: {Main.pilotSelectSettings.ProceduralPilots} ");
        if (Main.pilotSelectSettings.RandomRonin + Main.pilotSelectSettings.ProceduralPilots +
            Main.pilotSelectSettings.RoninFromList > 0)
        {
            // remove all pilot quirks that generate effects
            if (Main.settings.enablePilotQuirks)
            {
                foreach (Pilot pilot in simGameState.PilotRoster.rootList)
                {
                    PilotQuirkManager.Instance.proccessPilot(pilot.pilotDef, false);
                }
            }

            // now actually remove them
            while (simGameState.PilotRoster.Count > 0)
            {
                simGameState.PilotRoster.RemoveAt(0);
            }

            List<PilotDef> newPilots = new();

            if (Main.pilotSelectSettings.PossibleStartingRonin != null)
            {
                Main.modLog.Info?.Write($"Selecting {Main.pilotSelectSettings.RoninFromList} list ronin");
                var RoninRandomizer = GetRandomSubList(Main.pilotSelectSettings.PossibleStartingRonin,
                    Main.pilotSelectSettings.RoninFromList);
                foreach (var roninID in RoninRandomizer)
                {
                    var pilotDef = simGameState.DataManager.PilotDefs.Get(roninID);

                    // add directly to roster, don't want to get duplicate ronin from random ronin
                    if (pilotDef != null)
                    {
                        Main.modLog.Info?.Write($"Adding Starting Ronin {pilotDef.Description.Id}, to roster");
                        simGameState.AddPilotToRoster(pilotDef, true);
                    }
                }
            }

            if (Main.pilotSelectSettings.RandomRonin > 0)
            {
                Main.modLog.Info?.Write($"Selecting {Main.pilotSelectSettings.RandomRonin} random ronin");
                List<PilotDef> randomRonin = new(simGameState.RoninPilots);
                for (int m = randomRonin.Count - 1; m >= 0; m--)
                {
                    for (int n = 0; n < simGameState.PilotRoster.Count; n++)
                    {
                        // remove any ronin from the selection pool if they are already hired
                        if (randomRonin[m].Description.Id == simGameState.PilotRoster[n].Description.Id)
                        {
                            Main.modLog.Info?.Write(
                                $"Removing Ronin {randomRonin[m].Description.Id}, already in pool");
                            randomRonin.RemoveAt(m);
                            break;
                        }
                    }
                }

                var rnd = new Random();
                var randomized = randomRonin.OrderBy(item => rnd.Next());
                int count = 0;
                Dictionary<string, PilotRestrictionCounter> restrictions = new();
                foreach (var value in randomized)
                {
                    List<string> tagsToIncrement = new();
                    List<string> usedRestrictions = new();
                    bool isAllowed = true;
                    foreach (var tag in value.PilotTags)
                    {
                        foreach (var restriction in Main.pilotSelectSettings.RandomRestrictions)
                        {
                            if (usedRestrictions.Contains(restriction.restrictionId)) continue;
                            if (restriction.tags.Contains(tag))
                            {
                                Main.modLog.Debug?.Write($"Pilot: {value.Description.Callsign} has restricted tag: {tag}");
                                if (restriction.limit == 0)
                                {
                                    Main.modLog.Debug?.Write($"Rejecting pilot as a starting pilot, tag not allowed");
                                    isAllowed = false;
                                    break;
                                }

                                usedRestrictions.Add(restriction.restrictionId);

                                if (!restrictions.ContainsKey(restriction.tags[0]))
                                {
                                    PilotRestrictionCounter counter = new()
                                    {
                                        restriction = restriction
                                    };
                                    foreach (var restrictionTag in restriction.tags)
                                    {
                                        restrictions.Add(restrictionTag, counter);
                                    }
                                }

                                if (restrictions[tag].currentCount < restrictions[tag].restriction.limit)
                                {
                                    tagsToIncrement.Add(tag);
                                }
                                else
                                {
                                    Main.modLog.Debug?.Write($"Rejecting pilot as a starting pilot, tag has reached its limits already");
                                    isAllowed = false;
                                    break;
                                }

                            }
                        }

                        if (!isAllowed) break;

                    }

                    if (Main.settings.enablePilotManagement)
                    {
                        if (!PilotManagementManager.Instance.IsPilotAvailable(value, simGameState.CurSystem,
                                simGameState, true, true, out string rejectMsg))
                        {
                            isAllowed = false;
                            Main.modLog.Debug?.Write($"Rejecting pilot: {value.Description.Callsign}, {rejectMsg}");
                        }
                    }

                    // pilot isn't allowed, move on to the next
                    if (!isAllowed) continue;

                    // now increment all the tag restrictions this pilot has
                    foreach (var tag in tagsToIncrement)
                    {
                        restrictions[tag].currentCount += 1;
                    }

                    Main.modLog.Info?.Write($"Adding Random Ronin {value.Description.Id}, to roster");
                    newPilots.Add(value);
                    count++;
                    if (count >= Main.pilotSelectSettings.RandomRonin)
                    {
                        break;
                    }
                }
            }

            if (Main.pilotSelectSettings.ProceduralPilots > 0)
            {
                Main.modLog.Info?.Write($"Generating {Main.pilotSelectSettings.ProceduralPilots} proc pilots");
                List<PilotDef> collection =
                    simGameState.PilotGenerator.GeneratePilots(Main.pilotSelectSettings.ProceduralPilots, 1, 0f,
                        out List<PilotDef> list3);
                newPilots.AddRange(collection);
            }

            Main.modLog.Info?.Write($"Pilots to add to roster: {newPilots.Count}");
            foreach (PilotDef def in newPilots)
            {
                Main.modLog.Info?.Write($"Adding {def.Description.Callsign} to pilot roster");
                simGameState.AddPilotToRoster(def, true);
            }
        }

    }
}