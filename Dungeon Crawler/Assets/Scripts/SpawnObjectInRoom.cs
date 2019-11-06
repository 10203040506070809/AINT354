using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectInRoom : MonoBehaviour
{
    /// <summary>
    /// Enemy asset.
    /// </summary>
    public GameObject m_enemy;
    /// <summary>
    /// Percentage chance of enemy spawning.
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfEnemySpawn;
    private float m_tileWidth;
    private float m_enemyWidth;
    // Start is called before the first frame update
    void Start()
    {
        m_tileWidth = gameObject.GetComponentInChildren<Renderer>().bounds.size.x;
        m_enemyWidth = m_enemy.GetComponent<Renderer>().bounds.size.x;
        /// Gets the coordinate position of the tile the script is attached to.
        Vector3 centre = gameObject.transform.position;
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth/2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range((m_enemyWidth / 2) + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) -( m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
    }
}
