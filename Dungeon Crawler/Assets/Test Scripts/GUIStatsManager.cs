using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatsManager : MonoBehaviour
{
    /// <summary>
    /// A slider variable denoting the health slider UI element.
    /// </summary>
    [SerializeField] private Slider m_healthSlider;
    /// <summary>
    /// A slider variable denoting the corruption slider UI element.
    /// </summary>
    [SerializeField] private Slider m_corruptionSlider;
    /// <summary>
    /// A CharacterStats  variable denoting the entitites stats class.
    /// </summary>
    [SerializeField] private CharacterStats m_charStats;
    // Start is called before the first frame update
    void Start()
    {
        ///Initialise sliders to max values
        m_healthSlider.maxValue = m_charStats.m_maxHealth;
        m_corruptionSlider.maxValue = m_charStats.m_maxInsanity.GetValue();
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
        m_healthSlider.value = m_charStats.m_currentHealth;
        m_corruptionSlider.value = m_charStats.m_currentInsanity;
    }
}
