using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Created using Game Dev Guide's 'Creating a Custom Tab System in Unity https://www.youtube.com/watch?v=211t6r12XPQ
/// </summary>
[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private TabGroup m_tabGroup;

    public Image m_background;

    // Start is called before the first frame update
    void Start()
    {
        m_background = GetComponent<Image>();
        m_tabGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_tabGroup.OnTabExit(this);
    }
}
