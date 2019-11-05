using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractable : Interactable
{
    /// <summary>
    /// The player.
    /// </summary>
    [SerializeField] private GameObject m_player;
    /// <summary>
    /// The dungeon generator script.
    /// </summary>
    [SerializeField] private DungeonGen m_dungeon;
    /// <summary>
    /// Checks if the player has interacted with the end.
    /// </summary>
    public override void InteractedWith()
    {
        /// Gets the dungeonGen script from the dungeon gen game object.
        m_dungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGen>();
        /// Gets the player object.
        m_player = GameObject.FindGameObjectWithTag("Player");
        /// Debug.
        base.InteractedWith();
        /// Rebuilds a new dungeon.
        m_dungeon.Start();
        /// Moves the player to the start position of the new dungeon.
        m_player.transform.position = new Vector3(m_dungeon.m_startTile.worldPosition.x, m_dungeon.m_startTile.worldPosition.y + (m_player.GetComponent<CharacterController>().bounds.size.y / 2), m_dungeon.m_startTile.worldPosition.y);
    }
}
