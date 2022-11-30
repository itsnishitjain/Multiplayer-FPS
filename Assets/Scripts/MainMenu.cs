﻿using UnityEngine;
using Photon.Pun;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        Join();
        base.OnConnectedToMaster();
    }
    public override void OnJoinedRoom()
    {
        StartGame();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public void Connect()
    {
        Debug.Log("trying to connect.....");
       PhotonNetwork.GameVersion = "0.0.0"; 
       PhotonNetwork.ConnectUsingSettings(); 
    }
    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void Create()
    {
        PhotonNetwork.CreateRoom("");
    }
    public void StartGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
