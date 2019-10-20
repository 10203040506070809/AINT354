
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }
}
