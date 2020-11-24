using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThisPlayerInfo", menuName = "NFQ/User/Create AccountInfo")]
public class ThisPlayerAccountInfo : ScriptableObject
{
    public string Id;
    public string FullName;
    public string DisplayName;
    public string Email;
}
