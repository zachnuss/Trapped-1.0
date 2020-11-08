using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public LevelSetup myLevelSetUp;
    public GameObject mortalPref;

    public int width;
    public int height;
    public GameObject[,] grid2DArray;

    Transform startingPoint;
    public float distanceBetweenNodes = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetGridStartPoint();

        grid2DArray = new GameObject[width, height];
        //create size of grid based on size of level
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                //Debug.Log(x + " " + y);

                nodeTile.name = "Grid_Node_" + x.ToString() + ":" + y.ToString();

                nodeTile.transform.localPosition = new Vector3(x, 0, y);
                
                grid2DArray[rows, col] = nodeTile;
                nodeTile.transform.parent = this.transform;
                
            }
        }

       // Destroy(node);
    }

    //sets start point depending on level cube size
   void SetGridStartPoint()
   {
        switch (myLevelSetUp.type)
        {
            case levelTypeE.EasyLevel:
                this.transform.localPosition = new Vector3(-18, 40, -18);
                break;
            case levelTypeE.MidLevel:

                break;
            case levelTypeE.Hardlevel:

                break;
            case levelTypeE.none:

                break;
            default:
                break;
        }
    }
}

public class NodeRef : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}


