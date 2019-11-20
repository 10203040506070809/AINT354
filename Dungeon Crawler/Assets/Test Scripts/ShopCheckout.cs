using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCheckout : MonoBehaviour
{
    private PlayerHotbar m_playerHotbar;

    public void Checkout(Item item)
    {
        item.OnPickUp();
        SaveSystem.SavePlayer(m_playerHotbar);
    }
}
