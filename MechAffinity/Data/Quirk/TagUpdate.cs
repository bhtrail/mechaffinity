using System.Collections.Generic;

namespace MechAffinity.Data;

public class TagUpdate
{
    public List<string> addTags = new();
    public List<string> removeTags = new();
    public string selector = "";
}