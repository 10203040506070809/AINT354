using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool m_isPaused = false;
    [SerializeField] private GameObject m_pauseMenu;
    private void Start()
    {
        m_isPaused = false;
        m_pauseMenu.SetActive(false);
    }
    /// <summary>
    /// Checks for pause menu input, opens or closes pause menu and pauses the game if its found.
    /// </summary>
    /// 
    private void Update()
    {
        CheckForInput();
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckForInput()
    {
        if (m_isPaused == false)
        {
            if (Input.GetButtonDown("Pause"))
            {
                Time.timeScale = 0f;
                m_isPaused = true;
                m_pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                Time.timeScale = 1f;
                m_isPaused = false;
                m_pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;

            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        m_isPaused = false;
        m_pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
