﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.Networking;

public class HighScores : MonoBehaviour
{
    const string privateCode = "WwCyPKX43k-hX5r-rtOpnQEW_tOYdzM0iIH73S6lHMhw";  //Key to Upload New Info
    const string publicCode = "61b642938f40bb39202bafbb";   //Key to download
    const string webURL = "http://dreamlo.com/lb/"; //  Website the keys are for

    public PlayerScore[] scoreList;
    DisplayHighscores myDisplay;

    static HighScores instance; //Required for STATIC usability
    void Awake()
    {
        instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscores>();
    }
    
    public static void UploadScore(string username, int score)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        instance.StartCoroutine(instance.DatabaseUpload(username,score)); //Calls Instance
    }

    IEnumerator DatabaseUpload(string userame, int score) //Called when sending new score to Website
    {
        
        var loaded  = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(userame) + "/" + score);
        loaded.downloadHandler = new DownloadHandlerBuffer();
        yield return loaded.SendWebRequest();
        yield return loaded;

        if (string.IsNullOrEmpty(loaded.error))
        {
            print("Upload Successful");
            DownloadScores();
        }
        else print("Error uploading" + loaded.error);
    }

    public void DownloadScores()
    {
        StartCoroutine("DatabaseDownload");
    }
    IEnumerator DatabaseDownload()
    {
        //WWW www = new WWW(webURL + publicCode + "/pipe/"); //Gets the whole list
        var loaded = new UnityWebRequest(webURL + publicCode + "/pipe/0/10"); //Gets top 10
        loaded.downloadHandler = new DownloadHandlerBuffer();
        yield return loaded.SendWebRequest();
        yield return loaded;

        if (string.IsNullOrEmpty(loaded.error))
        {
            OrganizeInfo(loaded.downloadHandler.text);
            myDisplay.SetScoresToMenu(scoreList);
        }
        else print("Error uploading" + loaded.error);
    }

    void OrganizeInfo(string rawData) //Divides Scoreboard info by new lines
    {
        string[] entries = rawData.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        scoreList = new PlayerScore[entries.Length];
        for (int i = 0; i < entries.Length; i ++) //For each entry in the string array
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            scoreList[i] = new PlayerScore(username,score);
            print(scoreList[i].username + ": " + scoreList[i].score);
        }
    }
}

public struct PlayerScore //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;

    public PlayerScore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}