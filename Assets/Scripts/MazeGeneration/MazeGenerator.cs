using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{
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


    // Testing structures for generating maze with standard cells in order to correctly orient prefabs.
    // public CellType[] TESTCELLS;
    // public static List<Cell> TESTGRID = new List<Cell>();

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
        SetupGrid();

        do
        {
            MazeLogic();
        } while (stack.Count > 0);

        MakeExits();
        SetupWalls();
        
       
    }


    //Basic Methods

    public void DestroyMaze()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i].DestroyCell();
        }

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
    #endregion
}

// Required to see it in editor
 [System.Serializable]
 public class CellType
 {
     public GameObject[] CellList;
 }
