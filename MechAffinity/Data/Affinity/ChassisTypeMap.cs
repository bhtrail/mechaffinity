using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MechAffinity.Data;

public class ChassisTypeMap
{
    public List<string> chassisIds = new();
    
    [JsonConverter(typeof(StringEnumConverter))]
    public EIdType idType = EIdType.AssemblyVariant;
}