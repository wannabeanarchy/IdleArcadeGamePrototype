using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class CellHexItem : MonoBehaviour
    {
        [SerializeField] private int index;
        [SerializeField] public bool isEnable;
        [SerializeField] public bool isEnableInit;
        [SerializeField] private GameObject prefabGate; 
        [SerializeField] private List<GateCellSettings> gatesList;

        private List<GameObject> gates;

        void Start()
        { 
            IdleArcadeEvents.startGameEvent += CellHexInit;
            IdleArcadeEvents.generateGatesEvent += CreateGates;
            IdleArcadeEvents.onBackLobbyEvent += OnBackClick;
        }
 
        private void OnDestroy()
        { 
            IdleArcadeEvents.startGameEvent -= CellHexInit;
            IdleArcadeEvents.generateGatesEvent -= CreateGates;
            IdleArcadeEvents.onBackLobbyEvent -= OnBackClick;
        }

        private void CellHexInit()
        {
            int isEnableSave = PlayerPrefs.GetInt(TextKeys.PREF_CELLHEX + index, 0);

            isEnable = isEnableSave == 1 ? true : isEnableInit;

            if (!isEnable)
                this.gameObject.SetActive(false);
        }


        private void OnBackClick()
        {
            foreach (var gateObject in gates)
            {
                Destroy(gateObject);
            } 
        }

        private void CreateGates()
        {
            gates = new List<GameObject>();

            foreach (var gate in gatesList)
            { 
                if (!gate.linkHex.GetComponent<CellHexItem>().isEnable)
                {
                    var _gate = Instantiate(prefabGate, this.transform);
                    _gate.transform.Rotate(new Vector3(0, gate.AngleGate, 0));
                    _gate.GetComponent<GateItem>().SetLinkHex(gate.linkHex);
                    _gate.GetComponent<GateItem>().SetGateCellCost(gate.gateCellCost);
                    gates.Add(_gate);
                }
            }
        }

        public void SaveCellHexOpen()
        {
            PlayerPrefs.SetInt(TextKeys.PREF_CELLHEX + index, 1);
        }
    } 
}