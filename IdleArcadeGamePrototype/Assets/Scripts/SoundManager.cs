using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class SoundManager : MonoBehaviour
    { 
        [SerializeField] AudioSource buttonClick;
        [SerializeField] AudioSource attackSound;
        [SerializeField] AudioSource resourcesSound;

        private void Start()
        {
            IdleArcadeEvents.onButtonClick += OnButtonClick;
            IdleArcadeEvents.onAttackSound += OnAttackSound;
            IdleArcadeEvents.onResourcesSound += OnResourcesSound;
        }

        private void OnDestroy()
        {
            IdleArcadeEvents.onButtonClick -= OnButtonClick;
            IdleArcadeEvents.onAttackSound -= OnAttackSound;
            IdleArcadeEvents.onResourcesSound -= OnResourcesSound;
        }

        private void OnButtonClick()
        {
            buttonClick.Play();
        }

        private void OnAttackSound()
        {
            attackSound.Play();
        }

        private void OnResourcesSound()
        {
            resourcesSound.Play();
        }
    }
}