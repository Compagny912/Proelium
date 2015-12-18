using System.IO;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using System;

public class RoundTime : PunBehaviour
{
    public GUISkin skin;
    private const string TimeToStartProp = "st";
    private static double timeToStart = 0.0f;
    public double SecondsBeforeStart = 5.0f;   // set in inspector

    public static bool IsItTimeYet
    {
        get { return IsTimeToStartKnown && PhotonNetwork.time > timeToStart; }
    }

    public static bool IsTimeToStartKnown
    {
        get { return timeToStart > 0.001f; }
    }

    public double SecondsUntilItsTime
    {
        get
        {
            if (IsTimeToStartKnown)
            {
                double delta = timeToStart - PhotonNetwork.time;
                return (delta > 0.0f) ? delta : 0.0f;
            }

            else
            {
                return 0.0f;
            }
        }
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.room != null && (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers))
            {
                PhotonNetwork.room.visible = false;
            }

            // the master client checks if a start time is set. we check a min value
            if (!IsTimeToStartKnown && PhotonNetwork.time > 0.0001f) //&& PhotonNetwork.room.playerCount >= 2
            {
                // no startTime set for room. calculate and set it as property of this room
                timeToStart = PhotonNetwork.time + SecondsBeforeStart;

                Hashtable timeProps = new Hashtable() { { TimeToStartProp, timeToStart } };
                PhotonNetwork.room.SetCustomProperties(timeProps);
            }
        }
    }

    public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(TimeToStartProp))
        {
            timeToStart = (double)propertiesThatChanged[TimeToStartProp];
            Debug.Log("Got StartTime: " + timeToStart + " is it time yet?! " + IsItTimeYet);
        }
    }

    void OnGUI()
    {
        if (PhotonNetwork.inRoom)
        {
            if (this.SecondsUntilItsTime >= 0.0001f)
            {
                Score.gameState = enumGameState.InGameLobby;
                GUI.Label(new Rect(Screen.width - 250, 25, 230, 50), "Temps Restant: " + (int)this.SecondsUntilItsTime, skin.customStyles[0]);
            }
            if (IsItTimeYet)
            {
                Score.gameState = enumGameState.InGame;
            }
        }
    }

    void OnPhotonPlayerDisconnected()
    {
        //if (IsTimeToStartKnown && PhotonNetwork.room.playerCount < 2)
        //{
        //    timeToStart = 0.0f;
        //}
    }
}