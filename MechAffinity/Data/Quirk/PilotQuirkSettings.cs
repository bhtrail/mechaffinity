using System;
using System.Collections.Generic;

namespace MechAffinity.Data;

public class PilotQuirkSettings
{
    public bool playerQuirkPools = false;
    public bool argoAdditive = true;
    public bool argoMultiAutoAdjust = true;
    public float argoMin = 0.0f;
    
    public List<QuirkPool> quirkPools = new();
    public List<PilotTooltipTag> tooltipTags = new();
    public List<String> addTags = new();
    public List<TagUpdate> tagUpdates = new();
    public List<QuirkRestriction> restrictions = new();

}