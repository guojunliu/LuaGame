using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using DG.Tweening;

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
        ArrayList east = new ArrayList();
        ArrayList west = new ArrayList();
        ArrayList south = new ArrayList();
        ArrayList north = new ArrayList();

        foreach (FloorCube fsp in floorCubeSpArray)
        {
            if (fsp.row == sp.row)
            {
                if (fsp.column < sp.column)
                {
                    west.Add(fsp.gameobject);
                }
                else if (fsp.column > sp.column)
                {
                    east.Add(fsp.gameobject);
                }
            }
            else if (fsp.column == sp.column)
            {
                if (fsp.row < sp.row)
                {
                    south.Add(fsp.gameobject);
                }
                else if (fsp.row > sp.row)
                {
                    north.Add(fsp.gameobject);
                }
            }
        }

        float speed = 0.1f;
        float duration = 0.3f;

        for (int i = 0; i < west.Count; i++)
        {
            GameObject cube = (GameObject)west[i];
            cube.transform.DOLocalRotate(new Vector3(0, 0, 180), duration, RotateMode.WorldAxisAdd).SetDelay(speed * (west.Count - i));
            cube.GetComponent<MeshRenderer>().material.DOColor(Color.blue, duration).SetDelay(speed * (west.Count - i));
        }

        for (int i = 0; i < east.Count; i++)
        {
            GameObject cube = (GameObject)east[i];
            cube.transform.DOLocalRotate(new Vector3(0, 0, -180), duration, RotateMode.WorldAxisAdd).SetDelay(speed * i);
            cube.GetComponent<MeshRenderer>().material.DOColor(Color.blue, duration).SetDelay(speed * i);
        }
        for (int i = 0; i < south.Count; i++)
        {
            GameObject cube = (GameObject)south[i];
            cube.transform.DOLocalRotate(new Vector3(-180, 0, 0), duration, RotateMode.WorldAxisAdd).SetDelay(speed * (south.Count - i));
            cube.GetComponent<MeshRenderer>().material.DOColor(Color.blue, duration).SetDelay(speed * (south.Count - i));
        }

        for (int i = 0; i < north.Count; i++)
        {
            GameObject cube = (GameObject)north[i];
            cube.transform.DOLocalRotate(new Vector3(180, 0, 0), duration, RotateMode.WorldAxisAdd).SetDelay(speed * i);
            cube.GetComponent<MeshRenderer>().material.DOColor(Color.blue, duration).SetDelay(speed * i);
        }

        sp.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}

