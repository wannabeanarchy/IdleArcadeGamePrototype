using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleArcade
{
    public class GameUI : SingletonOneScene<GameUI>
    {
        [SerializeField] private GameObject lobbyUI; 
        [SerializeField] private GameObject joystickSetter;
        [SerializeField] private GameObject gameLogic;

        public void OnStartGameClick()
       {
            lobbyUI.SetActive(false);
            joystickSetter.SetActive(true);
            gameLogic.SetActive(true); 
            IdleArcadeEvents.startGameEvent?.Invoke();
            IdleArcadeEvents.generateGatesEvent?.Invoke();
            IdleArcadeEvents.onButtonClick?.Invoke();
        }

        public void OnRestartGameClick()
        {
            PlayerPrefs.DeleteAll();
            OnStartGameClick();
        }

        public void OnBackClick()
        {
            lobbyUI.SetActive(true);
            joystickSetter.SetActive(false);
            gameLogic.SetActive(false);
            IdleArcadeEvents.onBackLobbyEvent?.Invoke();
            IdleArcadeEvents.onButtonClick?.Invoke();
        }
    }
}