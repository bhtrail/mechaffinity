using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MechAffinity.Data;

class LegacySettings
{
    public bool debug = false;
    public int missionsBeforeDecay = -1;
    public int lowestPossibleDecay = 0;
    public int removeAffinityAfter = 100;
    public int maxAffinityPoints = 1000;
    public bool decayByModulo = false;
    public string debugForceTag = "";
    public int defaultDaysBeforeSimDecay = -1;
    public bool showQuirks = false;
    public bool showDescriptionsOnChassis = false;
    public bool trackSimDecayByStat = true;
    public bool trackLowestDecayByStat = false;
    public bool showAllPilotAffinities = true;
    public int topAffinitiesInTooltipCount = 3;

    public bool enablePilotQuirks = false;
    public bool playerQuirkPools = false;
    public bool pqArgoAdditive = true;
    public bool pqArgoMultiAutoAdjust = true;
    public float pqArgoMin = 0.0f;

    public bool enablePilotSelect = false;
    public bool enableMonthlyMoraleReset = false;
    public bool enableStablePiloting = false;
    public StablePilotingSettings stablePilotingSettings = new();
    
    [JsonIgnore]
    public Dictionary<string, Color> iconColoursMap = new();


    public List<QuirkPool> quirkPools = new();
    public List<PilotTooltipTag> pqTooltipTags = new();

    public List<PilotQuirk> pilotQuirks = new();
    public List<AffinityLevel> globalAffinities = new();
    public List<ChassisSpecificAffinity> chassisAffinities = new();
    public List<QuirkAffinity> quirkAffinities = new();
    public List<TaggedAffinity> taggedAffinities = new();

    public List<PrefabOverride> prefabOverrides = new();
    public List<AffinityGroup> affinityGroups = new();
    public List<PilotIcon> iconColours = new();
    public List<String> addTags = new();
    

public void InitLookups()
{
  foreach (PilotIcon pilotIcon in iconColours)
  {
    iconColoursMap.Clear();
    if (iconColoursMap.ContainsKey(pilotIcon.tag)) continue;
    iconColoursMap.Add(pilotIcon.tag, pilotIcon.GetColor());
  }
}

}
