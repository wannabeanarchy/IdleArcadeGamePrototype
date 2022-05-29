using System; 
using UnityEngine;

namespace IdleArcade
{
    public class JoystickPlayer : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float height;
        [SerializeField] private VariableJoystick variableJoystick;
        [NonSerialized]  private Animator animatorPlayer;
        [NonSerialized]  private Camera cameraMain;

        void Start()
        {
            cameraMain = Camera.main; 
            animatorPlayer = gameObject.GetComponentInChildren<Animator>();
        }

        public void FixedUpdate()
        {
            if (variableJoystick.Vertical != 0 || variableJoystick.Horizontal != 0)
            {
                animatorPlayer.SetInteger(TextKeys.ANIM_RUNNING_PARAMETR, 1);

                Transform camTransform = cameraMain.transform;
                Vector3 camPosition = new Vector3(camTransform.position.x, transform.position.y, camTransform.position.z);
                Vector3 direction = (transform.position - camPosition).normalized;
                Vector3 forwardMovement = direction * variableJoystick.Vertical;
                Vector3 horizontalMovement = camTransform.right * variableJoystick.Horizontal;
                Vector3 movement = Vector3.ClampMagnitude(forwardMovement + horizontalMovement, 1);

                transform.Translate(movement * speed * Time.deltaTime, Space.World);
                transform.eulerAngles = new Vector3(0, Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * 180 / Mathf.PI, 0);

                PlayerController.Instance().ReplaceCamera();
            }
            else
            {
                animatorPlayer.SetInteger(TextKeys.ANIM_RUNNING_PARAMETR, 0);
            }
        }

      

        
    }
}