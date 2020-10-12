using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{

    //Authors: Lugrim
    //additional changes by Grant Guenter
    #region Variables
    [Header("Maze Variables")]
    [Range(3, 60)] public int columsCount = 3;
    [Range(3, 60)] public int rowsCount = 3;
    [Range(1, 4)] public int exitsCount = 3;
    public static int cols, rows;

    [Header("Cell Variables")]
    public float cellSize = 1;
    [Header("Cells Prefabs")]
    public int[] cellTypes;
    public CellType[] alternativeCells;
    
    public Quaternion[] cellPrefabsRotations;
    public static List<Cell> grid = new List<Cell>();

    [Header("Door Game Object")]
    public GameObject doorObject;
    List<GameObject> doorPrefabs = new List<GameObject>();

    [Header("Key Game Object")]
    public GameObject keyObj;
    GameObject keyInstance;


    // Testing structures for generating maze with standard cells in order to correctly orient prefabs.
    // public CellType[] TESTCELLS;
    // public static List<Cell> TESTGRID = new List<Cell>();


    List<KeyValuePair<int, int>> exits = new List<KeyValuePair<int, int>>();

    public static List<Cell> stack = new List<Cell>();
    public static List<Cell> correctPath = new List<Cell>();
    private Cell current;
    private Cell next;
    #endregion

    #region Methods
    private void Start()
    {
        DestroyMaze();
        grid = new List<Cell>();
        stack = new List<Cell>();
        correctPath = new List<Cell>();
        for(int i = 0; i< exitsCount; i++)
        {
            doorPrefabs.Add(new GameObject());
        }
        SetupGrid();

        do
        {
            MazeLogic();
        } while (stack.Count > 0);

        MakeExits();
        SetupWalls();
        PlaceDoors();
        PlaceKey();
       
    }


    //Basic Methods

    public void DestroyMaze()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i].DestroyCell();
        }
        foreach(GameObject obj in doorPrefabs)
        {
            Destroy(obj);
        }
        Destroy(keyInstance);
    }

    private void SetupGrid()
    {
        cols = columsCount;
        rows = rowsCount;

        for (int q = 0; q < rows; q++)
        {
            for (int i = 0; i < cols; i++)
            {
                Cell cell = new Cell
                {
                    x = i,
                    z = q
                };

                grid.Add(cell);
            }
        }
        
        current = grid[0];
    }

    private void MakeExits() {
        // key : index of an exit cell in the cells list
        // value : index of its wall to remove to make an exit
        List<KeyValuePair<int, int>> indexAndWallToRemove = new List<KeyValuePair<int, int>> {
            new KeyValuePair<int, int>(Cell.Index(cols/2, 0), 3),
            new KeyValuePair<int, int>(Cell.Index(cols/2, rows-1), 1),
            new KeyValuePair<int, int>(Cell.Index(0, rows/2), 0),
            new KeyValuePair<int, int>(Cell.Index(cols-1, rows/2), 2)
        };

        for(int i = 0; i<exitsCount; i++) {;
            KeyValuePair<int, int> kvp = indexAndWallToRemove[UnityEngine.Random.Range(0, indexAndWallToRemove.Count)];
            grid[kvp.Key].walls[kvp.Value] = false;
            indexAndWallToRemove.Remove(kvp);
            exits.Add(kvp);
        }
    }

    private void SetupWalls()
    {
        Vector3 angle = Vector3.zero;
        GameObject[] prefabs;

        //Testing structure for orientation of prefabs.
        //GameObject[] prefabs2;
        int n, j;

        for (int i = 0; i < grid.Count; i++)
        {
            n = 0;
            
            for(j = 0; j<grid[i].walls.Length; j++) {
                n = (n<<1) + (grid[i].walls[j] ? 1 : 0);
            }
            prefabs = alternativeCells[cellTypes[n]].CellList;


            //Diagnostic for orienting cell prefabs
            /*prefabs2 = TESTCELLS[cellTypes[n]].CellList;
            TESTGRID = (List<Cell>)grid;
            TESTGRID[i].prefab = Instantiate(prefabs2[UnityEngine.Random.Range(0, prefabs.Length)], new Vector3(grid[i].x * cellSize, 0, grid[i].z * cellSize), cellPrefabsRotations[n], transform);
            Debug.Log(n);*/

            //Debug.Log(n + " : " + prefabs[0].name + " " + cellPrefabsRotations[n].eulerAngles);
            int p = UnityEngine.Random.Range(0, prefabs.Length);
            grid[i].prefab = Instantiate(prefabs[p], new Vector3(grid[i].x*cellSize, 0, grid[i].z*cellSize), cellPrefabsRotations[n]*prefabs[p].transform.rotation, transform);
        }
    }

    private void MazeLogic()
    {
        current.visited = true;

        next = current.GetRandomNeighbor();

        if (next != null)
        {
            stack.Add(current);
            RemoveWalls(current, next);
            current = next;
        }
        else if (stack.Count > 0)
        {
            current = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
        }
    }
    private void RemoveWalls(Cell a, Cell b)
    {
        int x = a.x - b.x;
        if (x == 1)
        {
            a.walls[0] = false;
            b.walls[2] = false;
        }
        else if (x == -1)
        {
            a.walls[2] = false;
            b.walls[0] = false;
        }

        int z = a.z - b.z;
        if (z == 1)
        {
            a.walls[3] = false;
            b.walls[1] = false;
        }
        else if (z == -1)
        {
            a.walls[1] = false;
            b.walls[3] = false;
        }
    }

    private Cell GetCell(int index)
    {
        return grid[index];
    }

    private void PlaceDoors()
    {
        for(int i = 0; i< exits.Count; i++)
        {
            PlaceDoor(GetCell(exits[i].Key), exits[i].Key, i);
        }
    }

    private void PlaceDoor(Cell doorCell, int index, int exitInd)
    {
        float xOffset = 0;
        float zOffset = 0;
        int rotation = 0;
        if(doorCell.x == 0 && doorCell.z == 0)
        {
            //Bottom Left
            if(!doorCell.walls[0])
            {
                zOffset = -(0.5f * cellSize);
            }
            else
            {
                xOffset = -(0.5f * cellSize);
                rotation = 0;
            }
        }else
        if (doorCell.x == cols-1 && doorCell.z == 0)
        {
            //Bottom Right
            if (!doorCell.walls[0])
            {
                zOffset = -(0.5f * cellSize);
            }
            else
            {
                xOffset = (0.5f * cellSize);
                rotation = 90;
            }
        }else
        if(doorCell.x == 0 && doorCell.z == rows-1)
        {
            //top left
            if (!doorCell.walls[2])
            {
                zOffset = (0.5f * cellSize);
            }
            else
            {
                xOffset = -(0.5f * cellSize);
                rotation = 90;
            }
        }else
        if (doorCell.x == cols-1 && doorCell.z == rows-1)
        {
            //top right
            if (!doorCell.walls[2])
            {
                zOffset = (0.5f * cellSize);
            }
            else
            {
                xOffset = (0.5f * cellSize);
                rotation = 90;
            }
        }else
        if (doorCell.x == 0)
        {   
            //left edge
            xOffset = -(0.5f * cellSize);
            rotation = 90;
        }else
        if (doorCell.x == cols-1)
        {   
            //right edge
            xOffset = (0.5f * cellSize);
            rotation = 90;
        }else
        if (doorCell.z == 0)
        {
            //bottom edge
            zOffset = -(0.5f * cellSize);
        }
        else
        if (doorCell.z == rows-1)
        {
            //left edge
            zOffset = (0.5f * cellSize);
        }
        Quaternion doorRotation = new Quaternion();
        doorRotation.eulerAngles = new Vector3(0, rotation+90, 0);

        doorPrefabs[exitInd] = Instantiate(doorObject, new Vector3(grid[index].x * cellSize+xOffset, 0, grid[index].z * cellSize+zOffset), doorRotation, transform);
    }

    public void PlaceKey()
    {
        float x = UnityEngine.Random.Range(1, cols * cellSize -1);
        float z = UnityEngine.Random.Range(1, rows * cellSize -1);
        Debug.Log(x + " " + z);
        x -= cellSize / 2;
        z -= cellSize / 2;
        keyInstance = Instantiate(keyObj,new Vector3(x,2.5f,z), new Quaternion(), transform);
    }

    #endregion
}

// Required to see it in editor
 [System.Serializable]
 public class CellType
 {
     public GameObject[] CellList;
 }
