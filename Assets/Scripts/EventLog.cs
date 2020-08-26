using System;
using System.Collections.Generic;

public class EventLog
{
    public string ID;
    public int gameNum;
    public int clearStep;
    public int gameoverStep;
    public int step;
    public int jump;
    public float xPos;
    public float yPos;

    public DateTime dateTime;

    public EventLog()
    {
        ID = "";
        gameNum = 0;
        clearStep = 0;
        gameoverStep = 0;
        step = 0;
        jump = 0;
        xPos = 0;
        yPos = 0;
        dateTime = DateTime.Now;
    }
}