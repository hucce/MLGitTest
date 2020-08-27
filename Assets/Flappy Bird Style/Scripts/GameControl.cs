using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using Unity.Barracuda;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;         //A reference to our game control script so we can access it statically.
    public GameObject mainCamera;
    private GameObject mainObjCamera;
    public bool playerPlay = false;
    public bool record = false;
    public bool resetColumns = false;
    public bool swapNNModel = false;
    public int minRecordGame = 2;
    public int maxRecordGame = 12;
    public string folderName = "";
    public int maxModel = 30;
    public List<NNModel> nnmodelList = new List<NNModel>();

    private void Awake()
    {
        ListupModel();
        maxRecordGame = minRecordGame + maxRecordGame;

        if (record)
        {
            QualitySettings.vSyncCount = 0; // vsync 사용안함
            Application.targetFrameRate = 30;
        }

        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    private void Start()
    {
        GameObject[] brids = GameObject.FindGameObjectsWithTag("BirdAgentScean");
        for (int i = 0; i < brids.Length; i++)
        {
            if (brids[i].GetComponent<BirdAgentScean>().mainScean)
            {
                mainObjCamera = brids[i];
                break;
            }
        }
        mainObjCamera = mainObjCamera.transform.GetComponentInChildren<Camera>().gameObject;
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = new Vector3(mainObjCamera.transform.position.x, mainObjCamera.transform.position.y, mainObjCamera.transform.position.z);
    }

    public void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ListupModel()
    {
        var resourcesArray = Resources.LoadAll<NNModel>(folderName);
        nnmodelList = resourcesArray.ToList();

        nnmodelList.Sort(delegate (NNModel first, NNModel second)
        {
            var name1 = int.Parse(first.name.Split('-')[1]);
            var name2 = int.Parse(second.name.Split('-')[1]);

            if (name1 > name2)
            {
                return 1;
            }
            else if (name1 < name2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        Debug.Log("리소스정렬");
    }
}