using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleArcade
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] public int health = 100;
        [SerializeField] public float distanceAttack = 0.3f;
        [SerializeField] public int attack = 10;
        [SerializeField] public float attackTime = 0.25f;
        [SerializeField] public float respawnTime = 10;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private Vector3 healthBarOffset = new Vector3(0f, 75f, 0f);

        [NonSerialized] private GameObject healthBar;
        [NonSerialized] private Image healthBarProgress;

        [NonSerialized] private Animator animator;
        [NonSerialized] private float currentHealth;

        [NonSerialized] public bool isDead = false;
        [NonSerialized] public DateTime deadTime;
        [NonSerialized] private bool isAttack = false;
        [NonSerialized] private Camera mainCamera;

        void Start()
        {
            animator = GetComponent<Animator>();
            currentHealth = health; 
            mainCamera = Camera.main;
            healthBar = Instantiate(healthBarPrefab, GameUI.Instance().transform);
            healthBarProgress = healthBar.transform.GetChild(0).GetComponent<Image>();
            healthBar.SetActive(false);
            IdleArcadeEvents.onBackLobbyEvent += OnBackClick;
            IdleArcadeEvents.onBackLobbyEvent += OnStartGame;

        }

        private void OnDestroy()
        {
            IdleArcadeEvents.onBackLobbyEvent -= OnBackClick;
            IdleArcadeEvents.onBackLobbyEvent -= OnStartGame;
        }

        private void OnStartGame()
        {
            currentHealth = health;
            isDead = false;
        }

        private void OnBackClick()
        {
            healthBar.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
                return;

            if (isDead)
                return;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player")
                return;

            if (isDead)
                return;

            animator.Play("IdleNormal");
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player")
                return;

            if (isDead) 
                return; 

            if (!isAttack)
            {
                isAttack = true;
                StartCoroutine(DoAttack());
            }

        }

        IEnumerator DoAttack()
        {
            float distancePlayer = Vector3.Distance(PlayerController.Instance().transform.position, this.transform.position);
            this.transform.LookAt(PlayerController.Instance().transform);
            if (distancePlayer < distanceAttack)
            {
                animator.Play("Attack01");
            }
            else
            {
                animator.Play("Attack02");
            }

            yield return new WaitForSeconds(attackTime);
            PlayerController.Instance().TakeDamage(attack);
            isAttack = false;
        }


        public void TakeDamage(int amount)
        {  
            if (isDead)
                return;

            currentHealth -= amount; 

            if (currentHealth <= 0)
            {
                Death();
            }
        }


        void Death()
        {
            isDead = true;
            animator.Play("Die");
            StartCoroutine(WaitForSpawn());
        }

        private IEnumerator WaitForSpawn()
        {
            yield return new WaitForSeconds(respawnTime);
            animator.Play("IdleNormal");
            isDead = false;
            currentHealth = health;
        }

        private void Update()
        {
            if (healthBar == null)
                return;

            if (currentHealth != health)
            {
                healthBar.SetActive(true);
                healthBar.transform.position = mainCamera.WorldToScreenPoint(this.transform.position) + healthBarOffset;
                healthBarProgress.fillAmount = currentHealth / health;
            }
            else if ((healthBar.activeSelf && (currentHealth >= health)) || isDead)
            {
                healthBar.SetActive(false);
            }
        }
    }
}