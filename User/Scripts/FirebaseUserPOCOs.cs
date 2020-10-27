using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

[FirestoreData]
public class FireStoreUserInfo
{
    [FirestoreProperty]
    public string userId { get; set; }
    [FirestoreProperty]
    public DateTime dateCreated { get; set; }
    [FirestoreProperty]
    public string password { get; set; }
    [FirestoreProperty]
    public string fullName { get; set; }
    [FirestoreProperty]
    public string displayName { get; set; }
    [FirestoreProperty]
    public string email { get; set; }
}
