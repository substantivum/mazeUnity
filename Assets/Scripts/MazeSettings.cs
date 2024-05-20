using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSettings : MonoBehaviour
{
    public int mazeDepth;
    public int mazeWidth;
    public int complexity;
    public Vector3 cameraPosition;

    public MazeSettings(int mazeDepth, int mazeWidth, int complexity, Vector3 cameraPosition)
    {
        this.mazeDepth = mazeDepth;
        this.mazeWidth = mazeWidth;
        this.complexity = complexity;
        this.cameraPosition = cameraPosition;
    }
}
