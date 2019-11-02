using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatsManager : MonoBehaviour
{
    [SerializeField] private Slider m_healthSlider;
    [SerializeField] private Slider m_corruptionSlider;
    [SerializeField] private CharacterStats m_charStats;
    // Start is called before the first frame update
    void Start()
    {
        ///Initialise sliders to max values
        m_healthSlider.maxValue = m_charStats.m_maxHealth;
        m_corruptionSlider.maxValue = m_charStats.m_maxInsanity.GetValue();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
    }
    void UpdateGUI()
    {
        m_healthSlider.value = m_charStats.m_currentHealth;
        m_corruptionSlider.value = m_charStats.m_currentInsanity;
    }
}
