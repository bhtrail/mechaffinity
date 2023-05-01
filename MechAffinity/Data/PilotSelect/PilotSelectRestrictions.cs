using System.Collections.Generic;

namespace MechAffinity.Data;

public class PilotSelectRestrictions
{
    public List<string> tags = new();
    public int limit = 0;
    public string restrictionId = "";
}