using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class SqlLite : MonoBehaviour
{
    public static SqlLite instance;         //A reference to our game control script so we can access it statically.

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

    public void WriteSql(EventLog _eventLog)
    {
        // conn = "URI=file:" + Application.dataPath + "/DB Browser로 만든 데이터베이스 이름.s3db";
        string conn = "URI=file:" + Application.streamingAssetsPath + "/sqlSetML.db";
        //string conn = "URI=file:" + Application.streamingAssetsPath + "/test.db";

        // IDbConnection
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery;
        sqlQuery = "INSERT INTO ML (ID, gameNum, step, clearStep, overStep, jumpStep, xPos, yPos, time)";
        sqlQuery += "VALUES ('" + _eventLog.ID + "'," + (_eventLog.gameNum - (GameControl.instance.minRecordGame - 1)) + "," + _eventLog.step + "," + _eventLog.clearStep + ","
            + _eventLog.gameoverStep + "," + _eventLog.jump + "," + _eventLog.xPos + "," + _eventLog.yPos + ",'" + _eventLog.dateTime.ToString() + "');";

        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        // 닫아주고 초기화 시켜주는 곳
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}