using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using System;
using System.Security.AccessControl;

public class BirdAgent : Agent
{
    private BirdControl birdControl;
    private EventLog eventLog = new EventLog();
    private int modelNum = 0;
    private int overNum = 0;
    private int clearNum = 0;
    private bool nextGame = false;

    public override void Initialize()
    {
        birdControl = GetComponent<BirdControl>();
    }

    public void Start()
    {
        if (GameControl.instance.swapNNModel)
        {
            modelNum = GameControl.instance.startModelIndex;
            SetModel(modelNum);
        }
    }

    public override void OnActionReceived(float[] actionBuffers)
    {
        //float plus = birdControl.birdSpeed * Time.fixedDeltaTime;
        //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (plus), gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

        transform.localPosition = new Vector3(gameObject.transform.localPosition.x + birdControl.birdSpeed, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        var movement = actionBuffers[0];
        bool jump = false;

        switch (movement)
        {
            case 1:
                jump = true;
                eventLog.jump = eventLog.step;
                //Debug.Log("점프: " + eventLog.jump);
                break;
        }

        AddReward(0.01f);

        if (birdControl.checkColumn)
        {
            AddReward(0.1f);
            birdControl.checkColumn = false;
        }

        // 죽음
        if (birdControl.isDead)
        {
            //Debug.Log("죽음");
            eventLog.gameoverStep = eventLog.step;
            // 학습 종료
            AddReward(-1f);
            EndEpisode();
        }
        else
        {
            if (birdControl.isClear)
            {
                //Debug.Log("클리어");
                eventLog.clearStep = eventLog.step;
                // 학습 종료
                AddReward(1f);
                EndEpisode();
            }
            else
            {
                // 점프
                if (jump)
                {
                    birdControl.JumpBrid();
                }
            }
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[0] = 1;
        }
    }

    public override void OnEpisodeBegin()
    {
        nextGame = true;

        if (GameControl.instance.record == false)
        {
            NextGame();
        }
    }

    private void NextGame()
    {
        eventLog.gameNum += 1;
        Debug.Log("다음 게임: " + eventLog.gameNum);
        eventLog.step = 0;

        if (eventLog.gameNum > GameControl.instance.maxRecordGame)
        {
            GameControl.instance.AppExit();
        }

        if (GameControl.instance.swapNNModel)
        {
            if (eventLog.gameNum > GameControl.instance.maxRecordGame)
            {
                if (GameControl.instance.swapNNModel)
                {
                    SetModel(modelNum);
                }
            }
        }

        gameObject.transform.localPosition = new Vector3(-21.9f, -2.45f, -9);
        gameObject.transform.rotation = Quaternion.identity;

        var bridControl = GetComponent<BirdControl>();
        bridControl.isDead = false;
        bridControl.isClear = false;
        bridControl.SetTrigger("Idle");
        bridControl.ZeroVelocity();

        if (GameControl.instance.resetColumns)
        {
            gameObject.transform.parent.GetComponent<BirdAgentScean>().createColumns.GenerateColumns();
        }

        if (GameControl.instance.playerPlay)
        {
            if (eventLog.gameNum > GameControl.instance.minRecordGame)
            {
                int maxGame = GameControl.instance.maxRecordGame - GameControl.instance.minRecordGame;
                PlayerGUI.instance.ShowGameNum(eventLog.gameNum - GameControl.instance.minRecordGame, maxGame, overNum, clearNum);
            }
        }
        nextGame = false;
    }

    private void Update()
    {
        eventLog.step += 1;

        eventLog.xPos = gameObject.transform.localPosition.x;
        eventLog.yPos = gameObject.transform.localPosition.y;

        if (GameControl.instance.record)
        {
            if (eventLog.gameNum > GameControl.instance.minRecordGame)
            {
                Debug.Log("게임수: " + eventLog.gameNum);
                SqlLite.instance.WriteSql(eventLog);
            }

            // 다음 게임 으로 넘어갔다면?
            if (nextGame)
            {
                NextGame();

                if (eventLog.clearStep != 0)
                {
                    eventLog.clearStep = 0;
                    if (eventLog.gameNum > GameControl.instance.minRecordGame)
                    {
                        clearNum++;
                    }
                }

                if (eventLog.gameoverStep != 0)
                {
                    eventLog.gameoverStep = 0;
                    if (eventLog.gameNum > GameControl.instance.minRecordGame)
                    {
                        overNum++;
                    }
                }
            }

            // 스탭에서 수정된 값을 먼저 쓰고 0으로
            if (eventLog.jump != 0)
            {
                eventLog.jump = 0;
            }
        }
    }

    private void SetModel(int trainNum)
    {
        GetComponent<BehaviorParameters>().Model = GameControl.instance.nnmodelList[trainNum];
        eventLog.ID = (modelNum + 1).ToString();
        modelNum += GameControl.instance.plusModelIndex;
        Debug.Log("모델명: " + GetComponent<BehaviorParameters>().Model.name + " 모델번호: " + modelNum + " 성공수: " + clearNum);
        eventLog.gameNum = 0;
        overNum = 0;
        clearNum = 0;
    }
}