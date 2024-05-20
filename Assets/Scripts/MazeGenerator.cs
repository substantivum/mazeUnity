using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject camera;

    [SerializeField]
    private MazeCell mazeCellPrefab;

    [SerializeField]
    private int mazeWidth;
    [SerializeField]
    private int mazeDepth;
    private MazeCell[,] mazeGrid;

    [SerializeField]
    private int seed;

    public GameObject mazeParent;
    public MazeSettings ms;

    private void Start()
    {
        ms = GetComponent<MazeSettings>();
    }
    private enum MazeCorner
    {
        TopLeft,
        TopRight,
        BottomRight
    }

    public void CreateMaze(bool useSeed, MazeSettings mazeSettings)
    {
        
        if (useSeed)
        {
            Random.InitState(seed);
        }

        else
        {
            int randomSeed = Random.Range(1, 1000000);
            Random.InitState(randomSeed);
            seed = randomSeed;

            print(randomSeed);
        }

        

        mazeDepth = mazeSettings.mazeDepth;
        mazeWidth = mazeSettings.mazeWidth;
        print(mazeDepth);
        print(mazeWidth);
        camera.transform.position = mazeSettings.cameraPosition;
        mazeGrid = new MazeCell[mazeWidth, mazeDepth];
        mazeParent = new GameObject("Maze");

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, mazeParent.transform);
            }
        }

        SpawnCoins(3);
        GenerateMaze(null, mazeGrid[0, 0]);
        OpenRandomCorner();

    }


    private void OpenRandomCorner()
    {
        MazeCorner randomCorner = (MazeCorner)Random.Range(0, 3);
        print(randomCorner);

        switch (randomCorner)
        {
            case MazeCorner.TopLeft:
                OpenTopLeftCorner();
                break;
            case MazeCorner.TopRight:
                OpenTopRightCorner();
                break;
            case MazeCorner.BottomRight:
                OpenBottomRightCorner();
                break;
        }
    }

    private void OpenTopLeftCorner()
    {
        print("Top left is open");
        MazeCell topLeftCell = mazeGrid[0, mazeDepth - 1];
        topLeftCell._leftWall.SetActive(false);
    }

    private void OpenTopRightCorner()
    {
        print("Top right is open");
        MazeCell topRightCell = mazeGrid[mazeWidth - 1, mazeDepth - 1];
        topRightCell._rightWall.SetActive(false);
    }
    private void OpenBottomRightCorner()
    {
        print("Bottom right is open");
        MazeCell bottomRightCell = mazeGrid[mazeWidth - 1, 0];
        bottomRightCell._backWall.SetActive(false);
    }

    private void SpawnCoins(int numCoins)
    {
        List<MazeCell> emptyCells = GetEmptyCells();

        int coinsSpawned = 0;

        // Shuffle the emptyCells list to randomize the coin spawn locations
        emptyCells = ShuffleList(emptyCells);

        // Spawn coins up to the specified number or until there are no more empty cells
        foreach (MazeCell emptyCell in emptyCells)
        {
            if (coinsSpawned >= numCoins)
            {
                break;
            }


            // Instantiate the coin prefab at the cell's position
            GameObject coin = Instantiate(coinPrefab, emptyCell.transform.position, Quaternion.identity, mazeParent.transform);
            coin.transform.Rotate(90f, 0f, 0f);
            coin.transform.position += new Vector3(0f, 0.4f, 0f);
            // Mark the cell as having a coin
            emptyCell.SetHasCoin();

            coinsSpawned++;
        }
    }

    private List<MazeCell> GetEmptyCells()
    {
        List<MazeCell> emptyCells = new List<MazeCell>();

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                MazeCell currentCell = mazeGrid[x, z];

                // Check if the current cell has not been visited and all its walls are active
                if (!currentCell.isVisited &&
                    currentCell._leftWall.activeSelf &&
                    currentCell._rightWall.activeSelf &&
                    currentCell._frontWall.activeSelf &&
                    currentCell._backWall.activeSelf)
                {
                    emptyCells.Add(currentCell);
                }
            }
        }

        return emptyCells;
    }

    private List<MazeCell> ShuffleList(List<MazeCell> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            MazeCell temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);

    }


    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];

            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];

            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < mazeDepth)
        {
            var cellToFront = mazeGrid[x, z + 1];

            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];

            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }



}
