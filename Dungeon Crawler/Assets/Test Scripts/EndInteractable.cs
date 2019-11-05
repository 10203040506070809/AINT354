using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractable : Interactable
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private DungeonGen m_dungeon;
    public override void InteractedWith()
    {
        m_dungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGen>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        base.InteractedWith();
        m_dungeon.Start();
        m_player.transform.position = m_dungeon.m_startTile.worldPosition;
    }
}
