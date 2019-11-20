using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCheckout : MonoBehaviour
{
    public PlayerHotbar m_playerHotbar;

    public void Checkout(Item item)
    {
        m_playerHotbar = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerHotbar>();
        item.OnPickUp();
        SaveSystem.SavePlayer(m_playerHotbar);
    }
}
