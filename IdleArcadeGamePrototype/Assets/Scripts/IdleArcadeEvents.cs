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
        public static StartGame startGameEvent;
        public static GenerateGates generateGatesEvent;
        public static OnBackLobby onBackLobbyEvent;

        public static OnButtonClick onButtonClick;
        public static OnAttackSound onAttackSound;
        public static OnResourcesSound onResourcesSound;

        public delegate void ResourcesTriggerEvent(ResourcesItem resourcesObject);
        public delegate void ResourcesGettingEvent(TypeResources resources, int count);
        public delegate void StartGame();
        public delegate void GenerateGates();
        public delegate void OnBackLobby();

        public delegate void OnButtonClick();
        public delegate void OnAttackSound();
        public delegate void OnResourcesSound();
    }
}
