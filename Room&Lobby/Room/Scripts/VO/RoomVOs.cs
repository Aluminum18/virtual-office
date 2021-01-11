using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;

[FirestoreData]
public class FireStoreRoomData
{
    [FirestoreProperty]
    public string roomName { get; set; }
    [FirestoreProperty]
    public List<string> team1 { get; set; }
    [FirestoreProperty]
    public List<string> team2 { get; set; }
    [FirestoreProperty]
    public int readyOnRoyaleCount { get; set; }
    [FirestoreProperty]
    public int maxPlayerPerTeam { get; set; }

}

public class RTDBRoomData
{
    public string roomName;
    public List<string> teamOne;
    public List<string> teamTwo;
    public int readyOnRoyaleCount;
    public int maxPlayerPerTeam;
    public List<int> pos1Picks;
    public List<int> pos2Picks;
    public List<int> pos3Picks;
    public List<int> pos4Picks;
    public List<int> pos5Picks;
    public List<int> pos6Picks;

}