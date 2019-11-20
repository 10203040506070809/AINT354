using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHotbar : MonoBehaviour
{

    /// <summary>
    /// A reference to the hotbar items. 
    /// </summary>
    public GameObject[] m_hotBarItems = new GameObject[4];
    /// <summary>
    /// A reference to the hotbar icons. 
    /// </summary>
    public Image[] m_hotBarIcons = new Image[4];

    private void Start()
    {
        SaveSystem.LoadPlayerHotbar();
    }
    // Update is called once per frame
    void Update()
    {
        
        CheckForInput();
    }
    /// <summary>
    /// Checks for input, I.E whether the project defined inputs for hotbar slots have been pressed down. If so, activates whatever is in that slot.
    /// </summary>
    void CheckForInput()
    {
        if (Input.GetButtonDown("Hotbar Slot 1"))
        {
            if (m_hotBarItems[0] != null)
            {
                ActivateEffect(m_hotBarItems[0]);
                m_hotBarItems[0] = null;
                m_hotBarIcons[0].sprite = null;
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 2"))
        {
            if (m_hotBarItems[1] != null)
            {
                ActivateEffect(m_hotBarItems[1]);
                m_hotBarItems[1] = null;
                m_hotBarIcons[1].sprite = null;
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 3"))
        {
            if (m_hotBarItems[2] != null)
            {
                ActivateEffect(m_hotBarItems[2]);
                m_hotBarItems[2] = null;
                m_hotBarIcons[2].sprite = null;
            }
        }
        else if (Input.GetButtonDown("Hotbar Slot 4"))
        {
            if (m_hotBarItems[3] != null)
            {
                ActivateEffect(m_hotBarItems[3]);
                m_hotBarItems[3] = null;
                m_hotBarIcons[3].sprite = null;
            }
        }
    }
    /// <summary>
    /// Activates the effect of the item in the slot.
    /// </summary>
    /// <param name="item"></param>
    private void ActivateEffect(GameObject item)
    {
        item.GetComponent<Item>().ActivateItem();
        Destroy(item);
    }
}
