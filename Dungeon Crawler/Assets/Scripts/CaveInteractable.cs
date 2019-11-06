using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInteractable : Interactable
{
    public override void InteractedWith()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        DungeonGen dg = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGen>();

        player.GetComponent<PlayerMovement>().reset = true;
        player.transform.position = new Vector3(dg.m_startTile.worldPosition.x, (dg.m_startTile.worldPosition.y + player.GetComponent<CharacterController>().bounds.size.y / 2), dg.m_startTile.worldPosition.z);

    }
}
