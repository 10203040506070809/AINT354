using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatsManager : MonoBehaviour
{
    /// <summary>
    /// A slider variable denoting the health slider UI element.
    /// </summary>
    [SerializeField] private Slider m_healthSlider = null;
    /// <summary>
    /// A slider variable denoting the corruption slider UI element.
    /// </summary>
    [SerializeField] private Slider m_corruptionSlider = null;
    /// <summary>
    /// A CharacterStats  variable denoting the entitites stats class.
    /// </summary>
    private CharacterStats m_charStats = null;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    
    /// <summary>
    /// Called once per frame, calls the UpdateGUI method.
    /// </summary>
    void Update()
    {
        UpdateGUI();
    }
    /// <summary>
    /// Updates all GUI elements.
    /// </summary>
    void UpdateGUI()
    {
        if(m_charStats == null)
        {
            m_charStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            ///Initialise sliders to max values
            m_healthSlider.maxValue = m_charStats.m_maxHealth;
            m_corruptionSlider.maxValue = m_charStats.m_maxInsanity.GetValue();
        }
        m_healthSlider.value = m_charStats.m_currentHealth;
        m_corruptionSlider.value = m_charStats.m_currentInsanity;
    }
}
