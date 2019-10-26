using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstructable : MonoBehaviour
{
    private Color m_transparantColour;
    private Color m_originalColour;
    private Renderer renderer;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        m_originalColour = renderer.material.color;
    }

    public void SetTransparant()
    {
        renderer.material.color = m_transparantColour;
    }
    public void SetNormal()
    {
        renderer.material.color = m_originalColour;
    }
}
