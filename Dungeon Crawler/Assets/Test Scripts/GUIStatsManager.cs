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
