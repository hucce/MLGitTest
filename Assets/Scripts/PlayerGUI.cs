using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasObj = null;

    [SerializeField]
    private GameObject backgraound = null;

    [SerializeField]
    private Text text = null;

    public static PlayerGUI instance;         //A reference to our game control script so we can access it statically.

    private void Awake()
    {
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
        if (GameControl.instance.playerPlay)
        {
            canvasObj.SetActive(true);
            ShowLoding();
        }
    }

    public void ShowLoding()
    {
        backgraound.SetActive(true);
        text.alignment = TextAnchor.MiddleCenter;
        text.text = "Loading";
    }

    public void ShowGameNum(int current, int max, int dead, int clear)
    {
        backgraound.SetActive(false);
        text.alignment = TextAnchor.UpperLeft;
        text.text = "Game: " + current + " / " + max + "\n" + "Dead: " + dead + "\n" + "Clear: " + clear;
    }
}