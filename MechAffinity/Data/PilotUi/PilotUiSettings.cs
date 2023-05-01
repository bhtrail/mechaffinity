using System.Collections.Generic;

namespace MechAffinity.Data;

public class PilotUiSettings
{
    public List<PilotIcon> pilotIcons = new();
    public List<PilotAffinityColour> pilotAffinityColours = new();
    public bool enableAffinityColour = false;
    public bool orderByAffinity = false;
}