using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Created using Game Dev Guide's 'Creating a Custom Tab System in Unity' https://www.youtube.com/watch?v=211t6r12XPQ
/// </summary>
public class TabGroup : MonoBehaviour
{

    public List<TabButton> m_tabButtons;
    public Sprite m_tabIdle;
    public Sprite m_tabHover;
    public Sprite m_tabActive;

    public void Subscribe(TabButton button)
    {
        if(m_tabButtons == null)
        {
            m_tabButtons = new List<TabButton>();
        }
        m_tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach (TabButton button in m_tabButtons)
        {
            button.m_background.sprite = m_tabIdle;
        }
    }
}
