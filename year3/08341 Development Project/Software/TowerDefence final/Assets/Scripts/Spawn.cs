using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
    public GameObject enemyPrefab, networking;
    public float interval = 1.0f;
    private bool timer = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (timer == true)
                timer = false;
            else
                timer = true;
        }
        if (timer == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && interval <= 0.0f)
            {
                SpawnEnemy();
                interval = 1.0f;
                networking.GetComponent<Networking>().broadcaster.SendUDPMessage("1");
            }
            interval -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpawnEnemy();
                networking.GetComponent<Networking>().broadcaster.SendUDPMessage("1");
            }
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
