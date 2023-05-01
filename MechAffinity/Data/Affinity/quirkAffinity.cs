using Newtonsoft.Json;
using System.Collections.Generic;

namespace MechAffinity.Data;

public class LeveledAffinity {
  [JsonProperty(Order = -2, NullValueHandling = NullValueHandling.Ignore)]
  public string id;
  [JsonProperty(Order = -1)]
  public List<AffinityLevel> affinityLevels = new();

}
public class QuirkAffinity: LeveledAffinity {
    public List<string> quirkNames = new();
}
