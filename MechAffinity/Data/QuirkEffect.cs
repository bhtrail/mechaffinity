﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MechAffinity.Data
{
    class QuirkEffect
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EQuirkEffectType type = EQuirkEffectType.MedTech;
        public float modifier = 0;
        public float secondaryModifier = 0;
    }
}
