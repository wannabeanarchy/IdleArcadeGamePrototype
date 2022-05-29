using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade
{
    public class DeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player"  )
                return;

            PlayerController.Instance().OnRespawn();
        }
    }
}