using System;
using System.Collections.Generic;
using BattleTech;

namespace MechAffinity.Data;

public class QuirkPool
{
    public string tag = "";
    public int quirksToPick = 0;
    public int defaultQuirkWeight = 3;
    public bool drawUntilPicksFull = true;
    public List<string> quirksAvailable = new();
    public Dictionary<string, int> weightedQuirks = new();

    public List<string> GetQuirks()
    {
        List<string> quirkList = new();
        WeightedList<string> quirkPool = new(WeightedListType.WeightedRandomUseOnce);

        foreach (var keyPair in weightedQuirks)
        {
            quirkPool.Add(keyPair.Key, keyPair.Value);
        }

        foreach (var quirk in quirksAvailable)
        {
            quirkPool.Add(quirk, defaultQuirkWeight);
        }

        return quirkList;
    }
}