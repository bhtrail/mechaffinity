using System;
using System.Collections.Generic;
using System.Linq;
using BattleTech;
using Newtonsoft.Json;

namespace MechAffinity.Data;

public class QuirkPool
{
    public string tag = "";
    public int quirksToPick = 0;
    public int defaultQuirkWeight = 3;
    public bool drawUntilPicksFull = true;
    public int maxDrawAttempts = 5;
    public List<string> quirksAvailable = new();
    public Dictionary<string, int> weightedQuirks = new();

    [JsonIgnore] private WeightedList<string> quirkPool;

    public List<string> GetQuirks()
    {
        HashSet<string> quirkList = new HashSet<string>();
        if (quirkPool == null)
        {
            quirkPool = new WeightedList<string>(WeightedListType.WeightedRandomUseOnce);

            foreach (var keyPair in weightedQuirks)
            {
                quirkPool.Add(keyPair.Key, keyPair.Value);
            }

            foreach (var quirk in quirksAvailable)
            {
                quirkPool.Add(quirk, defaultQuirkWeight);
            }
        }
        else
        {
            quirkPool.Reset();
        }

        int quirksDrawn = 0;
        int maxAttempts = maxDrawAttempts + quirksToPick;
        while (quirkList.Count < quirksToPick && quirksDrawn < maxAttempts)
        {
            quirkList.Add(quirkPool.GetNext());
            quirksDrawn++;
        }

        return quirkList.ToList();
    }
}