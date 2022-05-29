using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleArcade
{
    public class PlayerController : SingletonOneScene<PlayerController>
    {
        [SerializeField] private GameObject weaponPickAxe;
        [SerializeField] private GameObject weaponAxe;
        [SerializeField] private GameObject weaponSword; 
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private Vector3 healthBarOffset = new Vector3(0f, 75f, 0f);

        [NonSerialized]  private GameObject healthBar;
        [NonSerialized]  private Image healthBarProgress;

        [SerializeField] private float health;
        [SerializeField] private int attack;
        [SerializeField] private float attackTime;
        [SerializeField] private float timeHealth;
        [NonSerialized] private float currentHealth;
        [NonSerialized] private DateTime timeDamageTake;

        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 12, -10);

        [NonSerialized] private Animator animatorPlayer;
        [NonSerialized] private Dictionary<TypeResources, int> playerResources;

        [NonSerialized] private ResourcesItem currentItem;
        [NonSerialized] private Camera mainCamera;

        private bool isAttack = false;

        void Start()
        {
            IdleArcadeEvents.startGameEvent += InitResourcesSave;
            animatorPlayer = gameObject.GetComponentInChildren<Animator>(); 
            mainCamera = Camera.main;
            OnRespawn();
            currentHealth = health;

            healthBar = Instantiate(healthBarPrefab, GameUI.Instance().transform);
            healthBarProgress = healthBar.transform.GetChild(0).GetComponent<Image>();
            healthBar.SetActive(false);  
        }

        protected override void OnDestroy()
        {
            _instance = null;
            IdleArcadeEvents.startGameEvent -= InitResourcesSave;
        }

        private void InitResourcesSave()
        {
            playerResources = new Dictionary<TypeResources, int>();
            foreach (var res in Enum.GetValues(typeof(TypeResources)))
            {
                int countRes = PlayerPrefs.GetInt(TextKeys.PREF_RES + res, 0);
                playerResources.Add((TypeResources)res, countRes);
            }

            OnRespawn();
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
            if (currentItem)
            {
                currentItem.OnGettingResources();
                currentItem = null;
            }
        }

        public void OnResourcesTigger()
        {
            animatorPlayer.Play("Cutting");
            animatorPlayer.SetInteger(TextKeys.ANIM_RUNNING_PARAMETR, 0);
        }

        public void OnGettingResources(TypeResources resource, int count)
        { 
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

            PlayerPrefs.SetInt(TextKeys.PREF_RES + resource, playerResources[resource]);
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

    
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Enemy")
            {
                if (isAttack)
                    return;

                Enemy enemy = other.gameObject.GetComponent<Enemy>();

                if (!enemy.isDead)
                {
                    isAttack = true;
                    StartCoroutine(DoAttack(enemy));
                }
                else if (enemy.isDead)
                { 
                    HideWeapon();
                    animatorPlayer.Play("Idle");
                }
            }
         
        }

        IEnumerator DoAttack(Enemy enemy)
        {
            animatorPlayer.Play("AttackAnimation");

            IdleArcadeEvents.onAttackSound?.Invoke();
            yield return new WaitForSeconds(attackTime);
            weaponSword.SetActive(true);
            enemy.TakeDamage(attack);
            isAttack = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Enemy")
            {
                isAttack = false;
            }
            if (other.tag == "Resource")
            {
            }

            StopAllCoroutines();

            HideWeapon();
            animatorPlayer.Play("Idle"); 
        }

        public void TakeDamage(int amount)
        { 
            currentHealth -= amount;
            timeDamageTake = DateTime.Now;

            if (currentHealth <= 0)
            {
                OnRespawn();
            }
        }

        public void OnRespawn()
        {
            this.transform.position = new Vector3(spawnPoint.transform.position.x, 1.21f, spawnPoint.transform.position.z);
            animatorPlayer.Play("Flip"); 
            ReplaceCamera();
            currentHealth = health;
        }

        public void ReplaceCamera()
        {
            mainCamera.transform.position = (cameraOffset + this.transform.position);
        }

        private void Update()
        {
            if (currentHealth != health)
            {
                healthBar.SetActive(true);
                healthBar.transform.position = mainCamera.WorldToScreenPoint(this.transform.position) + healthBarOffset;
                healthBarProgress.fillAmount = currentHealth / health;

                if (timeDamageTake.AddSeconds(timeHealth) < DateTime.Now)
                {
                    currentHealth += 1;
                }
            }
            else if (healthBar.activeSelf) {
                healthBar.SetActive(false);
            }
        }
    }
}