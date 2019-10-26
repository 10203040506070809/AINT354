using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionDetector : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Obstructable m_lastObstructable;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetectPlayerObstruction());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DetectPlayerObstruction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            Vector3 direction = (player.transform.position - Camera.main.transform.position).normalized;
            RaycastHit m_rayCastHit;

            if (Physics.Raycast(Camera.main.transform.position, direction, out m_rayCastHit, Mathf.Infinity))
            {
                Obstructable obstruction = m_rayCastHit.collider.gameObject.GetComponent<Obstructable>();
                if (obstruction)
                {
                    obstruction.SetTransparant();
                    m_lastObstructable = obstruction;
                }
                else
                {
                    m_lastObstructable.SetNormal();
                }
            }
        }
    }
}
