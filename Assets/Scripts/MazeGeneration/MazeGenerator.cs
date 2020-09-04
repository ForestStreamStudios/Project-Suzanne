using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    #region Variables
    [Header("Maze Variables")]
    [Range(3, 2000)] public int columsCount = 3;
    [Range(3, 2000)] public int rowsCount = 3;
    public static int cols, rows;

    [Header("Cell Variables")]
    public float cellSize = 1;
    public GameObject[] cellPrefabs;
    public Quaternion[] cellPrefabsRotations;
    public static List<Cell> grid = new List<Cell>();
    public static List<Cell> stack = new List<Cell>();
    public static List<Cell> correctPath = new List<Cell>();
    private Cell current;
    private Cell next;
    #endregion

    #region Methods
    private void Start()
    {
        SetupGrid();

        do
        {
            MazeLogic();
        } while (stack.Count > 0);

        SetupWalls();
    }

    //Basic Methods
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

    private void SetupWalls()
    {
        Vector3 angle = Vector3.zero;
        int n, j;

        for (int i = 0; i < grid.Count; i++)
        {
            n = 0;
            
            for(j = 0; j<grid[i].walls.Length; j++) {
                n = (n<<1) + (grid[i].walls[j] ? 1 : 0);
            }
            
            Debug.Log(n + " : " + cellPrefabs[n].name + " " + cellPrefabsRotations[n].eulerAngles);
            grid[i].prefab = Instantiate(cellPrefabs[n], new Vector3(grid[i].x*cellSize, 0, grid[i].z*cellSize), cellPrefabsRotations[n], transform);
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