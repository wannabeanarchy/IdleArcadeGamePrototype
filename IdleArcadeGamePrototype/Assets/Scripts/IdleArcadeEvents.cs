using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleArcade
{ 
    public static class IdleArcadeEvents
    {
        public static ResourcesTriggerEvent resourcesTriggerEvent;
        public static ResourcesGettingEvent resourcesGettingEvent;

        public delegate void ResourcesTriggerEvent(ResourcesItem resourcesObject);
        public delegate void ResourcesGettingEvent(TypeResources resources, int count);
    }
}
