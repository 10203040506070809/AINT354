using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int m_level;
    public int m_gold;
    public int m_experience;
    
    public GameObject[] m_hotbarItems;

    public PlayerData(PlayerHotbar playerHotbar)
    {
        m_hotbarItems = playerHotbar.m_hotBarItems;
    }

    public PlayerData(PlayerStats playerStats)
    {
        m_level = playerStats.m_level;
        m_gold = playerStats.m_gold.GetValue();
        m_experience = playerStats.m_experience.GetValue();
    }
}
