﻿using System.Collections.Generic;
using Newtonsoft.Json;
using BattleTech;
using Newtonsoft.Json.Linq;

namespace MechAffinity.Data;

public class PilotQuirk
{
    public string id = "";
    public string tag = "";
    public string quirkName = "";
    public string description = "";
    public string restrictionCategory = "";

    [JsonIgnore]
    public List<EffectData> effects = new();
    public List<JObject> effectData = new();
    public List<QuirkEffect> quirkEffects = new();

    public void init()
    {
        foreach (JObject jObject in effectData)
        {
            EffectData effectData = new();
            effectData.FromJSON(jObject.ToString());
            effects.Add(effectData);
        }
    }
}
