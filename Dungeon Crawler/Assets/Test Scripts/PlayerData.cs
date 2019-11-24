using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// An int denoting the players level.
    /// </summary>
    public int m_level;
    /// <summary>
    /// An int denoting the players current gold.
    /// </summary>
    public int m_gold;
    /// <summary>
    /// An int denoting the players current experience.
    /// </summary>
    public int m_experience;
    /// <summary>
    /// A gameobject array, holding the current items within the players hotbar.
    /// </summary>
    public GameObject[] m_hotbarItems;

    /// <summary>
    /// Takes in a playerhotbar, and sets m_hotBarItems.
    /// </summary>
    /// <param name="playerHotbar"></param>
    public PlayerData(PlayerHotbar playerHotbar)
    {
        m_hotbarItems = playerHotbar.m_hotBarItems;
    }
    /// <summary>
    /// Stores the players current stats, ready to be saved.
    /// </summary>
    /// <param name="playerStats"></param>
    public PlayerData(PlayerStats playerStats)
    {
        m_level = playerStats.m_level;
        m_gold = playerStats.m_gold.GetValue();
        m_experience = playerStats.m_experience.GetValue();
    }
}
