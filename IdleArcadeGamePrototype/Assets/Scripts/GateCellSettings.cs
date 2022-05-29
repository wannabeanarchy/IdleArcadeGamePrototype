using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    [Serializable]
    public class GateCellSettings
    {
        public GameObject linkHex;
        public List<GateCellCost> gateCellCost;
        public int AngleGate; 
    }

    [Serializable]
    public class GateCellCost
    {
        public TypeResources typeResources;
        public int cost;
        public int currentCost; 
    }
}