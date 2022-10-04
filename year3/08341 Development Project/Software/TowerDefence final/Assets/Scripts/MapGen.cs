using UnityEngine;
using System.Collections;

public class MapGen : MonoBehaviour {
    public static MapGen instance = null;
    public int width = 10, height= 10;
    public int towerx=0, towerz=0;
    public GameObject[,] blockArray;
    public static bool shooting = true;
    public static bool towerPlaced = false;
    public static bool towerRemoved = false;
    public static BuildBlock currentTowerBuildBlock = null;

	void Start () {        
        instance = this;
        if (width % 2 != 1)
            width--;
        blockArray = new GameObject[width, height];
	    for (int y = 0; y < height; y++)
        {
            if (y == 0)
            {
                GameObject obj = (GameObject)Instantiate(Resources.Load("Build Block"));
                obj.transform.parent = this.transform;
                obj.transform.localPosition = new Vector3((width / 2), 0.5f, y);
                obj.name = "SpawnBlock";
                blockArray[(width / 2), y] = obj;
                Debug.Log("created spawn block at: [" + (width / 2) + "," + y + "]");
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject obj = (GameObject)Instantiate(Resources.Load("Build Block"));
                    obj.transform.parent = this.transform;
                    obj.transform.localPosition = new Vector3(x, 0.5f, y);
                    int id = x + (y * width);
                    obj.name = "BuildingBlock" + id;
                    blockArray[x, y] = obj;
                }
            }
        }
	}

    public GameObject GetBlock(int x, int y)
    {
        if (x >= width || y >= height || y < 0 || x < 0)
            return null;
        else
            return blockArray[x, y];
    }

    void LateUpdate()
    {
        currentTowerBuildBlock = null;
        towerPlaced = false;
        towerRemoved = false;
        if (Input.GetKeyDown(KeyCode.S))
        {
            shooting = shooting ? false : true;
        }
    }

    public Vector3 getTowerPos()
    {
        return new Vector3(towerx, 0.0f, towerz);
    }
}
