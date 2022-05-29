using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class CellHexItem : MonoBehaviour
    {
        [SerializeField] private bool isEnable;
        [SerializeField] private GameObject prefabGate; 
        [SerializeField] private List<GateCellSettings> gatesList;

        void Start()
        {
            if (!isEnable)
                this.gameObject.SetActive(false);

            CreateGates();
        }

        private void CreateGates()
        {
            foreach (var gate in gatesList)
            {
                var _gate = Instantiate(prefabGate, this.transform);
                _gate.transform.Rotate(new Vector3(0, gate.AngleGate, 0));
                _gate.GetComponent<GateItem>().SetLinkHex(gate.linkHex);
                _gate.GetComponent<GateItem>().SetGateCellCost(gate.gateCellCost);
            }
        } 
    } 
}