using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public enum TypeResources
    {
        Wood,
        Stone
    }


    public class ResourceData 
    {
        public TypeResources typeResources;
        public string name;
        public int countResource;
    }
}