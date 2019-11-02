using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectInRoom : MonoBehaviour
{
    public GameObject enemy;
    public GameObject pickUp;
    private int maxEnemy = 3;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 centre = gameObject.transform.position;
            if (Random.Range(0,100) < 100/3)
            {
                Vector3 tempPos = new Vector3(centre.x + Random.Range(0, 220), centre.y, centre.z + Random.Range(0, 220));
                Instantiate(enemy, tempPos, Quaternion.identity);
            }
            if (Random.Range(0, 100) < 100 / 3)
            {
                Vector3 tempPos = new Vector3(centre.x - Random.Range(0, 220), centre.y, centre.z - Random.Range(0, 220));
                Instantiate(enemy, tempPos, Quaternion.identity);
            }
            if (Random.Range(0, 100) < 100 / 3)
            {
                Vector3 tempPos = new Vector3(centre.x - Random.Range(0, 220), centre.y, centre.z + Random.Range(0, 220));
                Instantiate(enemy, tempPos, Quaternion.identity);
            }
            if (Random.Range(0, 100) < 100 / 3)
            {
                Vector3 tempPos = new Vector3(centre.x + Random.Range(0, 220), centre.y, centre.z - Random.Range(0, 220));
                Instantiate(enemy, tempPos, Quaternion.identity);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
