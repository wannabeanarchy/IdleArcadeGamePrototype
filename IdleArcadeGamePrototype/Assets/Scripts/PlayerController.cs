using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class PlayerController : SingletonOneScene<PlayerController>
    {
        [SerializeField] private GameObject weaponPickAxe;
        [SerializeField] private GameObject weaponAxe;
        [SerializeField] private GameObject weaponSword;

        [NonSerialized] private Animator animatorPlayer;
        [NonSerialized] private Dictionary<TypeResources, int> playerResources;

        [NonSerialized] private ResourcesItem currentItem;

        void Start()
        {
            playerResources = new Dictionary<TypeResources, int>();
            animatorPlayer = gameObject.GetComponentInChildren<Animator>();  
        }

        public void OnShowWeapon(TypeResources typeResources)
        {
            switch (typeResources) {
                case TypeResources.Stone:
                    weaponPickAxe.SetActive(true);
                    break;
                case TypeResources.Wood:
                    weaponAxe.SetActive(true);
                    break;
            } 
        }
        public void HideWeapon()
        {
            weaponPickAxe.SetActive(false);
            weaponAxe.SetActive(false);
            weaponSword.SetActive(false);
        }

        public void SetCurrentResourceItem(ResourcesItem resourcesItem)
        {
            currentItem = resourcesItem;
        }

        public void OnCuttingAnimationEnd()
        {
            currentItem.OnGettingResources();
            currentItem = null;
        }

        public void OnResourcesTigger()
        { 
            animatorPlayer.SetInteger(TextKeys.ANIM_CUTTING_PARAMETR, 1);
            animatorPlayer.SetInteger(TextKeys.ANIM_RUNNING_PARAMETR, 0);
        }

        public void OnGettingResources(TypeResources resource, int count)
        {
            animatorPlayer.SetInteger(TextKeys.ANIM_CUTTING_PARAMETR, 0);
            AddResource(resource, count);
            HideWeapon();
        }

        public void AddResource(TypeResources resource, int count)
        {
            if (playerResources.ContainsKey(resource))
            {
                playerResources[resource] += count;
            }
            else
            {
                playerResources.Add(resource, count);
            }
        }

        public Dictionary<TypeResources, int> GetResourcesPlayer()
        {
            return playerResources;
        }

        public int GetResourcesPlayerByType(TypeResources type)
        {
            if (playerResources.ContainsKey(type))
                return playerResources[type];
            else
                return 0;
        }
    }
}