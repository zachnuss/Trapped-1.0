using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MortarModBehavior : MonoBehaviour
{
    /// <summary>
    /// Depending on cube, depends size of grid that is located xx units in the local y of the NewPlayer obj
    /// 
    /// - mortars will spawn every x secs at a random node on the grid
    ///     - mortal behavior:
    ///         - will move downwards until it hits ground
    ///         - splash damage to player
    /// 
    /// </summary>

    LevelSetup myLevelSetUp;
    public GameObject mortalPref;

    public int width;
    public int height;
    public GameObject[,] grid2DArray;

    float distanceBetweenNodes = 5.0f;

    public int mortarShellDamage = 10;

    public float tracingLineSpeed = 13f;

    float minTime;
    float maxTime;

    /// <summary>
    /// Dylan Loe
    /// Updated 11-12-2020
    /// 
    /// Creates grid based on stats, starts bombing process
    /// </summary>
    void Start()
    {
        myLevelSetUp = FindObjectOfType<LevelSetup>();
        SetGridStartPoint();

        grid2DArray = new GameObject[width, height];
        //create size of grid based on size of level
        CreateGrid();

        StartCoroutine(SpawnMortar());
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Spawns the nodes and creates the grid
    /// </summary>
    void CreateGrid()
    {
        Debug.Log("Creating Mortar Grid");
        GameObject node = new GameObject("Grid_Node");
        //runs throug grid
        for (int rows = 0; rows < width; rows++)
        {
            for(int col = 0; col < height; col++)
            {
                GameObject nodeTile = (GameObject)Instantiate(node, transform);
                nodeTile.AddComponent<NodeRef>();

                float x = col * distanceBetweenNodes;
                
                float y = rows * distanceBetweenNodes;

                nodeTile.name = "Grid_Node_" + x.ToString() + ":" + y.ToString();

                nodeTile.transform.localPosition = new Vector3(x, 0, y);
                
                grid2DArray[rows, col] = nodeTile;
                nodeTile.transform.parent = this.gameObject.transform;
                
            }
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Initial setup based on size of level
    /// </summary>
   void SetGridStartPoint()
   {
        switch (myLevelSetUp.type)
        {
            case levelTypeE.EasyLevel:
                minTime = 4;
                maxTime = 9;
                break;
            case levelTypeE.MidLevel:
                minTime = 2;
                maxTime = 8;
                break;
            case levelTypeE.Hardlevel:
                minTime = 1;
                maxTime = 6;
                break;
            case levelTypeE.none:

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Dylan Loe
    /// Updated: 11-12-2020
    /// 
    /// Spawns a mortar prefab
    /// mortal spawning is done a timer that times a random range between 4 - 10 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnMortar()
    {
        float timeWaiting = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(timeWaiting);
        int wIndex = Random.Range(0, width);
        int hIndex = Random.Range(0, height);

        GameObject mortar = Instantiate(mortalPref, grid2DArray[wIndex, hIndex].transform.position, this.transform.rotation);
        mortar.GetComponent<MortarShell>().lineSpeed = tracingLineSpeed;
        StartCoroutine(SpawnMortar());
    }
}

/// <summary>
/// Dylan Loe
/// Updated: 11-12-2020
/// 
/// Each node is simply visible on gizmo
/// </summary>
public class NodeRef : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}


