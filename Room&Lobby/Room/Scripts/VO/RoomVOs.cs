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
    public int maxPlayerPerTeam { get; set; }
}