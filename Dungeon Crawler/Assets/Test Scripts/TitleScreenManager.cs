﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;




public class TitleScreenManager : MonoBehaviour
{
    /// <summary>
    /// An array to hold all canvas gameobjects
    /// </summary>
    [SerializeField] private GameObject[] m_screens;
    /// <summary>
    /// A reference to the main camera in the scene.
    /// </summary>
    private Camera m_cam;
    /// <summary>
    /// A Vector3 holding the position of the camera.
    /// </summary>
    private Vector3 m_pos;
    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
        m_pos = m_cam.transform.position;
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void OnQuitPress()
    {
        ///Quits application if run in the standalone version, otherwise debugs application quit
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Application closed.");
#endif
    }
    /// <summary>
    /// Starts a new game by opening the scene and discarding playerprefs.
    /// </summary>
    public void OnNewGamePress()
    {
        ///Open new scene, discard all playerprefs progress

        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs deleted");
        ///SceneManager.LoadScene("Overworld");
        
        Debug.Log("New Game Selected");
    }
    /// <summary>
    /// Continues the game on press and loads the Overworld scene.
    /// </summary>
    public void OnContinuePress()
    {
        ///Open new scene, keep all playerprefs progress
        ///SceneManager.LoadScene("Overworld");
        Debug.Log("Continue Game Selected");
    }
    /// <summary>
    /// Moves the camera to the options canvas.
    /// </summary>
    public void OnOptionsPress()
    {
        ///Goes to options canvas
        
        m_pos.y = m_screens[1].transform.position.y;
        StartCoroutine("MoveTo");

    }
    /// <summary>
    /// Moves the camera to the help canvas.
    /// </summary>
    public void OnHelpPress()
    {       
        m_pos.y = m_screens[2].transform.position.y;   
        StartCoroutine("MoveTo");
    }
    /// <summary>
    /// Moves the camera to the main menu canvas.
    /// </summary>
    public void OnMenuPress()
    {
        
        m_pos.y = m_screens[0].transform.position.y;
        StartCoroutine("MoveTo");
    }
    /// <summary>
    /// An enumerator which moves the camera over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveTo()
    {
        while(m_cam.transform.position != m_pos)
        {
            m_cam.transform.position = Vector3.Lerp(m_cam.transform.position, m_pos, 0.05f);
            yield return null;
        }
    }
}
