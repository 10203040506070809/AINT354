using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> itemList;
    [Range(0.0f, 100.0f)]
    [SerializeField] private float m_ChanceOfItemSpawn = 0;
    public void DropItem(Vector3 position, GameObject source)
    {
        if (Random.Range(0, 100) < m_ChanceOfItemSpawn)
        {
            int dropItem = Random.Range(0, itemList.Count - 1);
            float moveUp = itemList[dropItem].GetComponent<Renderer>().bounds.size.y;
            position.y += moveUp;
            Instantiate(itemList[dropItem], position, Quaternion.identity);
        }
    }
}
