using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int m_maxHealth = 100;
    public int m_currentHealth { get; private set; }

    public Stat m_damage;
    public Stat m_armour;
}
