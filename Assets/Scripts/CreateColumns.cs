using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColumns : MonoBehaviour
{
    public GameObject columnPrefab;                                 //The column game object.
    public int columnPoolSize = 10;                                  //How many columns to keep on standby.

    public float cloumnInterval = 10f;                                    //How quickly columns spawn.
    public float columnMin = -1f;                                   //Minimum y value of the column position.
    public float columnMax = 3.5f;                                  //Maximum y value of the column position.

    public void GenerateColumns()
    {
        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < columnPoolSize; i++)
        {
            // 앞선 애들의 기둥 좌표값을 확인
            //Vector3 befoPos = newCube.transform.localPosition;

            // 설정한 난이도에 따라 이전꺼와
            float yPosition = Random.Range(columnMin, columnMax);

            var newCube = Instantiate(columnPrefab);
            newCube.transform.SetParent(gameObject.transform);

            // 난이도 설정

            //

            newCube.transform.localPosition = new Vector3(i * cloumnInterval, yPosition, 0f);
            newCube.transform.localRotation = Quaternion.identity;
        }
    }
}