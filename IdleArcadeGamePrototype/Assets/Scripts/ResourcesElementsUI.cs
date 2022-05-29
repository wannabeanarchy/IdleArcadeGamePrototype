using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleArcade.UI
{
    public class ResourcesElementsUI : MonoBehaviour
    {
        [SerializeField] private TypeResources typeResources;

        private int value = 0;
        private Text targetText;


        private void Start()
        {
            targetText = GetComponent<Text>();
             
            IdleArcadeEvents.resourcesGettingEvent += OnGettingResources;
            IdleArcadeEvents.startGameEvent += ResourcesValueInit;
        }

        private void OnDestroy()
        {
            IdleArcadeEvents.resourcesGettingEvent -= OnGettingResources;
            IdleArcadeEvents.startGameEvent -= ResourcesValueInit;

        }

        private void ResourcesValueInit()
        { 
            value = PlayerPrefs.GetInt(TextKeys.PREF_RES + typeResources, 0);
            targetText.text = value.ToString();
        }

        private void OnGettingResources(TypeResources resource, int count)
        {
            if (resource != typeResources)
                return;

            SetCountText((value + count), value, 20.0f);
            value += count;
        }

        public void SetCountText(int newValue, int oldValue, float duration, float delay = 0, Action callback = null)
        {
            StartCoroutine(CountText(newValue, oldValue, duration, delay, callback));
        }

        private IEnumerator CountText(int newValue, int previousValue, float duration, float delay = 0, Action callback = null)
        { 
            yield return new WaitForSeconds(delay);
             
            int stepAmount;

            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / duration);
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / duration);
            }

            if (previousValue < newValue)
            {
                while (previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }
                    targetText.text = previousValue.ToString();

                    yield return new WaitForSeconds(1 / duration);
                }
            }
            else
            {
                while (previousValue > newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue < newValue)
                    {
                        previousValue = newValue;
                    }

                    targetText.text = previousValue.ToString();

                    yield return new WaitForSeconds(1 / duration);
                }
            }

            targetText.text = newValue.ToString();

            if (callback != null)
                callback?.Invoke();
        }
    }
}