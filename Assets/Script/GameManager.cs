using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public delegate void ActivateTiang(int nomorTiang, int tinggiTiang);
    public ActivateTiang OnTiangActivate;
    public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
}
