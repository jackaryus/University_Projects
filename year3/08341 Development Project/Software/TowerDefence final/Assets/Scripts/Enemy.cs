using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
    private GameObject pathfinding;
    private Vector3 currentNode;
    private List<Vector3> pathList;
    public float distanceAccuracy = 0.1f;
    private int currentNodeIndex;
    public float walkSpeed;
    MapGen mapGen;

	void Start () 
    {
        pathfinding = GameObject.Find("Pathfinding");
        pathList = pathfinding.GetComponent<AstarPathfinding>().GetPathList();
        currentNodeIndex = pathList.Count - 1;
        currentNode = pathList[currentNodeIndex];
        mapGen = GameObject.Find("Building Blocks").GetComponent<MapGen>();
	}

    void Update()
    {
        if (MapGen.towerPlaced == true || MapGen.towerRemoved == true)
        {
            Debug.Log("recalculate path");
            bool pathNeedsCalc = false;
            if (MapGen.towerPlaced == true)
            {
                Vector3 blockPos = MapGen.currentTowerBuildBlock.transform.localPosition;
                for (int i = currentNodeIndex; i >= 0; i--)
                {
                    if (pathList[i].x == blockPos.x && pathList[i].z == blockPos.z)
                    {
                        pathNeedsCalc = true;
                        break;
                    }
                }
            }
            else 
            {
                pathNeedsCalc = true;
            }
            if (pathNeedsCalc == true)
            {
                Vector3 lastPos = pathList[currentNodeIndex + 1 < pathList.Count ? currentNodeIndex + 1 : currentNodeIndex];
                List<Vector3> newPathList = pathfinding.GetComponent<AstarPathfinding>().RecalculatePath(lastPos);
                if (pathfinding.GetComponent<AstarPathfinding>().Path() == true)
                {
                    pathList = newPathList;
                    currentNodeIndex = pathList.Count - 1;
                    currentNode = pathList[currentNodeIndex];
                }
                else
                {
                    MapGen.currentTowerBuildBlock.DestroyTower();
                    MapGen.towerPlaced = false;
                }
            }
        }
        Vector3 direction = (mapGen.transform.position + currentNode) - this.transform.position;
        direction.y = 0f;

        Vector3 currentPos = this.transform.position;
        Vector3 target = currentPos + direction.normalized;
       
        if (direction.magnitude <= distanceAccuracy)
        {
            Vector3 goalPos = mapGen.transform.position + mapGen.getTowerPos();
            if (currentNode.x != goalPos.x && currentNode.z != goalPos.z)
            {       
                currentNodeIndex--;
                currentNode = pathList[currentNodeIndex];
            }
        }
        this.transform.position = Vector3.MoveTowards(currentPos, target, walkSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider co)
    {
        if (co.name == "Castle")
        {
            co.GetComponentInChildren<Health>().DecreaseHealth();
            Destroy(gameObject);
        }
    }

}
