using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.FrameCapturer;

public class BirdAgentScean : MonoBehaviour
{
    public bool mainScean = false;
    public bool playScean = false;

    public CreateColumns createColumns;

    private void Start()
    {
        // 기록용 ID 전달
        gameObject.transform.GetComponentInChildren<VideoRecorder>().enabled = false;
        gameObject.transform.GetComponentInChildren<MovieRecorder>().enabled = false;

        if (playScean == false)
        {
            if (mainScean)
            {
                gameObject.transform.GetComponentInChildren<VideoRecorder>().enabled = true;
                gameObject.transform.GetComponentInChildren<MovieRecorder>().enabled = true;
            }

            if (GameControl.instance.resetColumns)
            {
                createColumns = gameObject.GetComponentInChildren<CreateColumns>();
            }
        }
    }
}