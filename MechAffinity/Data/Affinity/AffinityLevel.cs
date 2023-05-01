using System.Collections.Generic;
using Newtonsoft.Json;
using BattleTech;
using Newtonsoft.Json.Linq;

namespace MechAffinity.Data;

public class AffinityLevel
{

    [JsonProperty(Order = -1, NullValueHandling = NullValueHandling.Ignore)]
    public string id;
    public int missionsRequired = 0;
    public string levelName = "sample";
    public string decription = "";
    public List<Affinity> affinities = new();
    [JsonIgnore]
    public List<EffectData> effects = new();
    public List<JObject> effectData = new();
}
