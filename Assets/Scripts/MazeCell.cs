using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    public GameObject _leftWall;

    [SerializeField]
    public GameObject _rightWall;

    [SerializeField]
    public GameObject _frontWall;

    [SerializeField]
    public GameObject _backWall;

    [SerializeField]
    public GameObject _unvisitedBlock;

    [SerializeField]
    public GameObject coinPrefab;



    public bool isVisited { get; private set; }
    public bool hasCoin { get; private set; }

    public void Visit()
    {
        isVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }

    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        _backWall.SetActive(false);

    }

    public void SetHasCoin()
    {
        hasCoin = true;
    }


}
