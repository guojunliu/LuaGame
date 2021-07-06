using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

// 礼物管理类
public class GameManager
{
    // ------------- 单例 -----------
    static GameManager instance = null;
    private static readonly object padlock = new object();

    public ArrayList floorCubeSpArray;

    private GameManager()
    {
    }

    // 单例
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameManager();
                    }
                }
            }
            return instance;
        }
    }

    // 变化cube
    public void ChangeCube (FloorCube sp)
    {
        ArrayList changeArray = new ArrayList();
        foreach (FloorCube fsp in floorCubeSpArray)
        {
            if (fsp.row == sp.row)
            {
                changeArray.Add(fsp.gameobject);
            }
            else if (fsp.column == sp.column)
            {
                changeArray.Add(fsp.gameobject);
            }
        }

        foreach (GameObject cube in changeArray)
        {
            cube.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }
}

