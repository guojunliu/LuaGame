using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    float length = 5f;
    float interval = 0.5f;

    ArrayList floorCubeSpArray;

    // Start is called before the first frame update
    void Start()
    {
        floorCubeSpArray = new ArrayList();
        for (int i = 0; i< 20; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(i * (length + interval), 0, j * (length + interval));
                cube.transform.localScale = new Vector3(length, 1, length);
                cube.transform.SetParent(gameObject.transform);
                //cube.tag = (1000 * i + j).ToString();

                FloorCube floorSp = cube.AddComponent<FloorCube>();
                floorSp.column = i;
                floorSp.row = j;
                floorSp.gameobject = cube;

                floorCubeSpArray.Add(floorSp);
            }
        }

        GameManager.Instance.floorCubeSpArray = floorCubeSpArray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
