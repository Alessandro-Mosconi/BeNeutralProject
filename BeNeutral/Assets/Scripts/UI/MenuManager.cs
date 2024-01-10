using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuManager : Singleton<MenuManager>
    {
        [Header("GROUPS")]
        [SerializeField] private Canvas Menu;
        [SerializeField] private Canvas MainMenu;
        [SerializeField] private Canvas Options;
        [SerializeField] private Canvas GameMenu;
        [SerializeField] private Canvas Feedback;
        [SerializeField] private Canvas Commands;
        [SerializeField] private Canvas Restart;
        
        [Header("BUTTONS")]
        [SerializeField] private  Button CloseButton;

        // - if true mainMenu if false gameMenu
        private bool MenuType = true;
        
        public void OpenMainMenu()
        {
            MenuType = true;
            Menu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(true);
            
            Options.gameObject.SetActive(false);
            GameMenu.gameObject.SetActive(false);
            Feedback.gameObject.SetActive(false);
            Commands.gameObject.SetActive(false);
        }
        
        public void OpenGameMenu()
        {
            MenuType = false;
            Menu.gameObject.SetActive(true);
            GameMenu.gameObject.SetActive(true);
            MainMenu.gameObject.SetActive(false);
            
            Options.gameObject.SetActive(false);
            Feedback.gameObject.SetActive(false);
            Commands.gameObject.SetActive(false);
            
            CloseButton.gameObject.SetActive(false);
        }

        public void CloseMenu()
        {
            Menu.gameObject.SetActive(false);
        }

        public void OpenSingleGroup()
        {
            if (MenuType)
            {
                MainMenu.gameObject.SetActive(false);
            }
            else
            {
                GameMenu.gameObject.SetActive(false);
            }
            CloseButton.gameObject.SetActive(true);
        }
        
        public void CloseSingleGroup()
        {
            if (MenuType)
            {
                MainMenu.gameObject.SetActive(true);
            }
            else
            {
                GameMenu.gameObject.SetActive(true);
            }
            CloseButton.gameObject.SetActive(false);
            Options.gameObject.SetActive(false);
            Feedback.gameObject.SetActive(false);
            Commands.gameObject.SetActive(false);
        }

        public void OpenOptions()
        {
            OpenSingleGroup();
            Options.gameObject.SetActive(true);
        }
        
        public void OpenFeedback()
        {
            OpenSingleGroup();
            Feedback.gameObject.SetActive(true);
        }
        
        public void OpenCommands()
        {
            OpenSingleGroup();
            Commands.gameObject.SetActive(true);
        }

        public void RestartGame()
        {
            Restart.gameObject.SetActive(true);
        }

        public void ConfirmRestartGame()
        {
            OpenMainMenu();
            GameManager.instance.ShowStartScreen();
            Restart.gameObject.SetActive(false);
        }
        public void UndoRestartGame()
        {
            Restart.gameObject.SetActive(false);
        }
        
        
        
    }
}
