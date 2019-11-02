using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatsManager : MonoBehaviour
{
    [SerializeField] private Slider m_HealthSlider;
    [SerializeField] private Slider m_CorruptionSlider;
    [SerializeField] private PlayerStats m_PlayerStats;
    // Start is called before the first frame update
    void Start()
    {
        m_HealthSlider.maxValue = m_PlayerStats.m_maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
    }

    void UpdateGUI()
    {

    }
}
