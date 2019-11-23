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
    [SerializeField] private float m_ChanceOfEnemySpawn = 0;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfBarrelSpawn = 0;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfSecondBarrelSpawn = 0;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfThirdBarrelSpawn = 0;
    private float m_tileWidth;
    private float m_enemyWidth;
    public GameObject m_barrel;
    private float m_barrelWidth;
    private float m_barrelHeight;
    // Start is called before the first frame update
    void Start()
    {
        int hasBarrel = -1;
        m_tileWidth = gameObject.GetComponentInChildren<Renderer>().bounds.size.x;
        m_enemyWidth = m_enemy.GetComponent<Renderer>().bounds.size.x;
        m_barrelWidth = m_barrel.GetComponentInChildren<Renderer>().bounds.size.x;
        m_barrelHeight = m_barrel.GetComponentInChildren<Renderer>().bounds.size.y;
        /// Gets the coordinate position of the tile the script is attached to.
        Vector3 centre = gameObject.transform.position;
        if (Random.Range(0, 100) < m_ChanceOfBarrelSpawn)
        {
            int quadrant = Random.Range(0, 3);
            if (quadrant == 0)
            {
                hasBarrel = 0;
                Vector3 tempPos = new Vector3((centre.x + m_tileWidth / 2) - (m_tileWidth/13), centre.y + m_barrelHeight / 2, (centre.z + m_tileWidth / 2) - (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 1)
            {
                hasBarrel = 1;
                Vector3 tempPos = new Vector3((centre.x + m_tileWidth / 2) - (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z - m_tileWidth / 2) + (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 2)
            {
                hasBarrel = 2;
                Vector3 tempPos = new Vector3((centre.x - m_tileWidth / 2) + (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z - m_tileWidth / 2) + (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }
            if (quadrant == 3)
            {
                hasBarrel = 3;
                Vector3 tempPos = new Vector3((centre.x - m_tileWidth / 2) + (m_tileWidth / 13), centre.y + m_barrelHeight / 2, (centre.z + m_tileWidth / 2) - (m_tileWidth / 13));
                Instantiate(m_barrel, tempPos, Quaternion.identity);
                if (Random.Range(0, 100) < m_ChanceOfSecondBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.x += m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
                if (Random.Range(0, 100) < m_ChanceOfThirdBarrelSpawn)
                {
                    Vector3 newTempPos = tempPos;
                    newTempPos.z -= m_barrelWidth;
                    Instantiate(m_barrel, newTempPos, Quaternion.identity);
                }
            }

        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 0)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth/2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range((m_enemyWidth / 2) + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 2)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 3)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
        /// Calculates (based on probability) if an enemy will spawn.
        if (Random.Range(0, 100) < m_ChanceOfEnemySpawn && hasBarrel != 1)
        {
            /// Generates a random coordinate in first quadrant of the tile.
            Vector3 tempPos = new Vector3(centre.x + Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) -( m_enemyWidth / 2) - (m_tileWidth / 20)), centre.y, centre.z - Random.Range(m_enemyWidth / 2 + (m_tileWidth / 16), (m_tileWidth / 2) - (m_enemyWidth / 2) - (m_tileWidth / 20)));
            /// Spawns enemy in a random point in first quadrant of the tile.
            Instantiate(m_enemy, tempPos, Quaternion.identity);
        }
    }
}
