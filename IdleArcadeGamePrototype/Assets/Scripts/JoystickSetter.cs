using UnityEngine;
using UnityEngine.UI;

namespace IdleArcade
{
    public class JoystickSetter : MonoBehaviour
    {
        [SerializeField] public Joystick variableJoystick;
         
       
        public void SnapX(bool value)
        {
            variableJoystick.SnapX = value;
        }

        public void SnapY(bool value)
        {
            variableJoystick.SnapY = value;
        }
    }
}