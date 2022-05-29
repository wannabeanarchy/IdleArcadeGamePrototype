using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class ResourcesItem : MonoBehaviour
    {
        [SerializeField] private float respawnTime = 3;
        [SerializeField] private int countResources = 10;
        [SerializeField] private TypeResources typeResources;

        [NonSerialized] private Animator animator;

          
        private bool isDead = false;

        private void Start()
        {
            animator = gameObject.GetComponentInChildren<Animator>(); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player" || isDead)
                return;

            IdleArcadeEvents.resourcesTriggerEvent?.Invoke(this);
            PlayerController.Instance().SetCurrentResourceItem(this); 
            PlayerController.Instance().OnShowWeapon(typeResources);
            PlayerController.Instance().OnResourcesTigger();
        }

        public void OnGettingResources()
        {
            animator.SetInteger(TextKeys.ANIM_DISSAPEARS_PARAMETR, 1);

            isDead = true;
        }

        private void OnDissapear()
        { 
            animator.SetInteger(TextKeys.ANIM_DISSAPEARS_PARAMETR, 0);

            IdleArcadeEvents.resourcesGettingEvent?.Invoke(typeResources, countResources);
            PlayerController.Instance().OnGettingResources(typeResources, countResources);
            StartCoroutine(WaitForSpawn());

        }

        private IEnumerator WaitForSpawn()
        { 
            yield return new WaitForSeconds(respawnTime);
            animator.SetInteger(TextKeys.ANIM_SPAWN_PARAMETR, 1);

        }

        private void OnSpawn()
        { 
            animator.SetInteger(TextKeys.ANIM_DISSAPEARS_PARAMETR, 0);
            animator.SetInteger(TextKeys.ANIM_SPAWN_PARAMETR, 0);
            isDead = false; 
        }
    }
}