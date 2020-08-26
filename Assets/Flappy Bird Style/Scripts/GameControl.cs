using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

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
    public int maxTrainNum = 20;
    public string folderName = "";

    private void Awake()
    {
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
}