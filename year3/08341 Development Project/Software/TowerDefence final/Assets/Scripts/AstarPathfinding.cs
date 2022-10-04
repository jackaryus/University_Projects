using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class AstarPathfinding : MonoBehaviour {
    public GameObject Spawn, Castle, Blocks;
    public List<Node> openList, closedList; 
    public List<GameObject> adjacentNodes;
    public Node currentNode, newNode, pathNode;
    private List<Vector3> pathList, recalculatedPathList;
    private bool pathCheck;
    private MapGen mapGen;

    public class Node
    {
        public Vector3 pos;
        public float costFromStart;
        public float heuristic;
        public float score;
        public Node parent;
    }

	void Start () 
    {
        openList = new List<Node>();
        closedList = new List<Node>();
        adjacentNodes = new List<GameObject>();
        currentNode = new Node();
        pathList = new List<Vector3>();
        recalculatedPathList = new List<Vector3>();
        pathNode = new Node();
        mapGen = Blocks.GetComponent<MapGen>();
        FindPath();
	}

    public void FindPath()
    {
        pathCheck = false;
        currentNode.pos = Blocks.GetComponent<MapGen>().GetBlock(14, 0).transform.localPosition;
        currentNode.pos.y += 0.5f;
        currentNode.costFromStart = 0.0f;
        currentNode.heuristic = 0.0f;
        currentNode.score = 0.0f;
        currentNode.parent = null;
        openList.Clear();
        closedList.Clear();
        openList.Add(currentNode);
        while (openList.Count > 0)
        {                
            currentNode = openList[0];
            if (currentNode.pos.x == mapGen.getTowerPos().x && currentNode.pos.z == mapGen.getTowerPos().z)
            {
                pathCheck = true;
                break;
            }
            else
            {
                closedList.Add(currentNode);
                openList.Remove(currentNode);
                adjacentNodes.Clear();
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x), (int)(currentNode.pos.z + 1)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x + 1), (int)(currentNode.pos.z)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x), (int)(currentNode.pos.z - 1)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x - 1), (int)(currentNode.pos.z)));
                for (int i = 0; i < adjacentNodes.Count; i++)
                {
                    if (adjacentNodes[i] != null)
                    {
                        Vector3 nodePos = new Vector3(adjacentNodes[i].transform.localPosition.x, 1.5f, adjacentNodes[i].transform.localPosition.z);
                        if (!OpenListContainsPosition(nodePos) && !ClosedListContainsPosition(nodePos) && adjacentNodes[i].GetComponent<BuildBlock>().hasTower() == false)// and not null && !openList.Contains(adjacentNodes[i]) && !closedList.Contains(adjacentNodes[i])
                        {
                            newNode = new Node();
                            newNode.pos = new Vector3(adjacentNodes[i].transform.localPosition.x, 1.5f, adjacentNodes[i].transform.localPosition.z);
                            newNode.parent = currentNode;
                            float dx = Math.Abs(mapGen.getTowerPos().x - newNode.pos.x);
                            float dy = Math.Abs(mapGen.getTowerPos().z - newNode.pos.z);
                            newNode.heuristic = dx + dy;
                            newNode.costFromStart = currentNode.costFromStart + 1;
                            newNode.score = newNode.costFromStart + newNode.heuristic;
                            openList.Add(newNode);
                        }
                    }
                }
            }
            openList.OrderBy(o=>o.score); 
        }
        BackTrack();
    }

    private bool OpenListContainsPosition(Vector3 pos)
    {
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].pos == pos)
                return true;
        }
        return false;
    }

    private bool ClosedListContainsPosition(Vector3 pos)
    {
        for (int i = 0; i < closedList.Count; i++)
        {
            if (closedList[i].pos == pos)
                return true;
        }
        return false;
    }

    public List<Vector3> GetPathList()
    {
        return new List<Vector3>(pathList);
    }

    public List<Vector3> RecalculatePath(Vector3 currentPos)
    {
        pathCheck = false;
        currentNode.pos = currentPos;
        currentNode.costFromStart = 0.0f;
        currentNode.heuristic = 0.0f;
        currentNode.score = 0.0f;
        currentNode.parent = null;
        openList.Clear();
        closedList.Clear();
        openList.Add(currentNode);
        while (openList.Count > 0)
        {
            currentNode = openList[0];
            if (currentNode.pos.x == mapGen.getTowerPos().x && currentNode.pos.z == mapGen.getTowerPos().z)
            {
                pathCheck = true;
                break;
            }
            else
            {
                closedList.Add(currentNode);
                openList.Remove(currentNode);
                adjacentNodes.Clear();
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x), (int)(currentNode.pos.z + 1)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x + 1), (int)(currentNode.pos.z)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x), (int)(currentNode.pos.z - 1)));
                adjacentNodes.Add(Blocks.GetComponent<MapGen>().GetBlock((int)(currentNode.pos.x - 1), (int)(currentNode.pos.z)));
                for (int i = 0; i < adjacentNodes.Count; i++)
                {
                    if (adjacentNodes[i] != null)
                    {
                        Vector3 nodePos = new Vector3(adjacentNodes[i].transform.localPosition.x, 1.5f, adjacentNodes[i].transform.localPosition.z);
                        if (!OpenListContainsPosition(nodePos) && !ClosedListContainsPosition(nodePos) && adjacentNodes[i].GetComponent<BuildBlock>().hasTower() == false)// and not null && !openList.Contains(adjacentNodes[i]) && !closedList.Contains(adjacentNodes[i])
                        {
                            newNode = new Node();
                            newNode.pos = new Vector3(adjacentNodes[i].transform.localPosition.x, 1.5f, adjacentNodes[i].transform.localPosition.z);
                            newNode.parent = currentNode;
                            float dx = Math.Abs(mapGen.getTowerPos().x - newNode.pos.x);
                            float dy = Math.Abs(mapGen.getTowerPos().z - newNode.pos.z);
                            newNode.heuristic = dx + dy;
                            newNode.costFromStart = currentNode.costFromStart + 1;
                            newNode.score = newNode.costFromStart + newNode.heuristic;
                            openList.Add(newNode);
                        }
                    }
                }
            }
            openList.OrderBy(o => o.score);
        }
        recalculatedPathList.Clear();        
        while (currentNode.parent != null)
        {
            recalculatedPathList.Add(currentNode.pos);
            currentNode = currentNode.parent;
        }
        recalculatedPathList.Add(currentNode.pos);

        return new List<Vector3>(recalculatedPathList);
    }

    public bool Path()
    {
        return pathCheck;
    }

    void BackTrack()
    {
        pathList.Clear();
        //back track through parents
        while (currentNode.parent != null)
        {
            pathList.Add(currentNode.pos);
            currentNode = currentNode.parent; 
        }
        pathList.Add(currentNode.pos);
    }
}
