using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IdleArcade
{
    public class GateItem : MonoBehaviour
    {
        [SerializeField] private float pause = 0.1f;
        [SerializeField] private TextMeshPro textCost;

        private GameObject linkCellHex;  
        private bool isTriggerActive = false; 

        private List<GateCellCost> gateCostList;
        private float pauseTMP = 0;
      
        private void Start()
        {
            pauseTMP = pause;
            UpdateTextCost();
        }

        private void UpdateTextCost()
        {
            string strText = string.Empty;

            foreach (var res in gateCostList)
            {
                strText += String.Format("[{0}]: {1}/{2} ", res.typeResources.ToString(), res.currentCost, res.cost);
            }
            textCost.text = strText;
        }

        public void SetLinkHex(GameObject hex)
        {
            linkCellHex = hex;
        }

        public void SetGateCellCost(List<GateCellCost> resCostList)
        {
            gateCostList = resCostList;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
                return; 

            isTriggerActive = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player")
                return; 
         
        }

        private void OnTriggerExit(Collider other)
        { 
            isTriggerActive = false;
        }

        private void GateOpenningTrigger()
        {
            if (isTriggerActive)
            {
                int countFull = 0;

                foreach (var res in gateCostList)
                {
                    if (res.currentCost == res.cost)
                    {
                        countFull++;
                    }
                    else
                    {
                        if (PlayerController.Instance().GetResourcesPlayerByType(res.typeResources) > 0)
                        {
                            IdleArcadeEvents.resourcesGettingEvent?.Invoke(res.typeResources, -1);
                            PlayerController.Instance().AddResource(res.typeResources, -1);
                            res.currentCost++;
                            UpdateTextCost();
                        }
                    }
                }

                if (countFull == gateCostList.Count)
                {
                    linkCellHex.SetActive(true);
                    this.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (isTriggerActive)
            {
                if (pause > 0)
                {
                    pause -= Time.deltaTime;
                    return;
                }

                pause = pauseTMP;
                GateOpenningTrigger(); 
            }
     }
    }
}