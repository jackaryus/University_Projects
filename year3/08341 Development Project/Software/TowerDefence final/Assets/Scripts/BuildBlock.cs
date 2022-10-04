using UnityEngine;
using System.Collections;

public class BuildBlock : MonoBehaviour 
{
	public GameObject Tower;
	GameObject Networking;
    private GameObject myTower;
    bool tower = false;
    
    AstarPathfinding pathfinding;

	void Start()
	{
		Networking = GameObject.Find("Networking");
        pathfinding = GameObject.Find("Pathfinding").GetComponent<AstarPathfinding>();
	}

    public void OnMouseUpAsButton()
    {
        if (tower == false)
        {
            spawnTower();
            tower = true;
            MapGen.towerPlaced = true;
            MapGen.currentTowerBuildBlock = this;
            Networking.GetComponent<Networking>().broadcaster.SendUDPMessage(this.gameObject.name);
        }
        else
        {
            DestroyTower();
            MapGen.towerRemoved = true;
            Networking.GetComponent<Networking>().broadcaster.SendUDPMessage(this.gameObject.name);
        }
        pathfinding.FindPath();
        if (pathfinding.Path() == false)
        {
            Debug.Log("no path from start");
            DestroyTower();
            MapGen.towerPlaced = false;
            pathfinding.FindPath();
        }
    }

    public void MultiplayerHandle()
    {
        if (tower == false)
        {
            spawnTower();
            tower = true;
            MapGen.towerPlaced = true;
            MapGen.currentTowerBuildBlock = this;
        }
        else
        {
            DestroyTower();
            MapGen.towerRemoved = true;
        }
        pathfinding.FindPath();
        if (pathfinding.Path() == false)
        {
            Debug.Log("no path from start");
            DestroyTower();
            MapGen.towerPlaced = false;
            pathfinding.FindPath();
        }
    }

    public void spawnTower()
    {
        myTower = (GameObject)Instantiate(Tower);
        myTower.transform.position = transform.position + Vector3.up;
    }

    public bool hasTower()
    {
        return tower;
    }

    public void DestroyTower()
    {
        GameObject.Destroy(myTower);
        tower = false;
    }
}
