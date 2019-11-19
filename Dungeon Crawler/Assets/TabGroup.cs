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
    public TabButton m_selectedTab;
    public List<GameObject> m_objectsToSwap;
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
        button.m_background.sprite = m_tabHover;
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        m_selectedTab = button;
        ResetTabs();
        button.m_background.sprite = m_tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < m_objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                m_objectsToSwap[i].SetActive(true);
            }
            else
            {
                m_objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in m_tabButtons)
        {
            if (m_selectedTab != null && button == m_selectedTab)
            { continue; }
            button.m_background.sprite = m_tabIdle;
        }
    }
}
