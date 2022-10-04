using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour 
{
    public GameObject bullet, blocks;
    public float rotSpeed = 35;
    public float timer = 0.0f;
    private List<Collider> enemyInRange = new List<Collider>();
    public Queue<Collider> inRange = new Queue<Collider>();
    private BuildBlock buildingBlock;


    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed, Space.World);
        timer -= Time.deltaTime;
        if (enemyInRange.Count > 0)
        {
            while (enemyInRange[0] == null)
            {
                enemyInRange.RemoveAt(0);
                if (enemyInRange.Count == 0)
                    break;     
            }
            if (MapGen.shooting == true)
            {
                if (timer <= 0.0f && enemyInRange.Count != 0)
                {
                    GameObject g = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
                    g.GetComponent<Bullet>().target = enemyInRange[0].transform; 
                    timer = 1.0f;
                }
            } 
        }
    }


    void OnTriggerEnter(Collider co)
    {
        if (co.gameObject.name == "Monster(Clone)")
            enemyInRange.Add(co);
    }

    
    void OnTriggerStay(Collider co)
    {
        if (!enemyInRange.Contains(co) && co.gameObject.name == "Monster(Clone)")
        {
            enemyInRange.Add(co);
        }
    }
     

    void OnTriggerExit(Collider co)
    {
        enemyInRange.Remove(co);
    }
}
