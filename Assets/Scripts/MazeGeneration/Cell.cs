using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    #region Variables
    public int x;
    public int z;
    // -x, +z, +x, -z
    public bool[] walls = { true, true, true, true };
    public bool visited = false;

    public GameObject prefab;
    public List<Cell> neighbors = new List<Cell>();
    public Cell[] directions = new Cell[4];
    #endregion

    #region Methods
    /*public void CalculateWalls()
    {
        for (int i = 0; i < 4; i++)
        {
            if (walls[i] == false)
            {
                GameObject wall = prefab.transform.GetChild(1).GetChild(i).gameObject;

                wall.SetActive(false);
                MazeGenerator.deleteCell.Add(wall);
            }
        }
    }*/
    public Cell CheckNeighbors()
    {
        if (Index(x, z - 1) != -1)
        {
            directions[0] = MazeGenerator.grid[Index(x, z - 1)];
        }
        if (Index(x + 1, z) != -1)
        {
            directions[1] = MazeGenerator.grid[Index(x + 1, z)];
        }
        if (Index(x, z + 1) != -1)
        {
            directions[2] = MazeGenerator.grid[Index(x, z + 1)];
        }
        if (Index(x - 1, z) != -1)
        {
            directions[3] = MazeGenerator.grid[Index(x - 1, z)];
        }

        for (int i = 0; i < 4; i++)
        {
            if (directions[i] != null && directions[i].visited == false)
            {
                neighbors.Add(directions[i]);
            }
        }

        if (neighbors.Count > 0)
        {
            int r = Random.Range(0, neighbors.Count);
            return neighbors[r];
        }
        else
        {
            return null;
        }
    }
    public int Index(int i, int j)
    {
        if (i < 0 || j < 0 || i > MazeGenerator.cols - 1 || j > MazeGenerator.rows - 1)
        {
            return -1;
        }
        return i + j * MazeGenerator.cols;
    }
    public int CountWalls() {
        int res = 0;
        foreach(bool i in walls) {
            if(i)
                res++;
        }

        return res;
    }
    #endregion
}
