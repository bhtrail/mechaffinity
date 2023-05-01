using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace MechAffinity.Data;

public class ChassisSpecificAffinity: LeveledAffinity {
    public List<string> chassisNames = new();
    
    [JsonConverter(typeof(StringEnumConverter))]
    public EIdType idType = EIdType.AssemblyVariant;

    public List<ChassisTypeMap> altMaps = new();
}
