using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probability
{
    public string getProbability(Dictionary<string, int> states){
        List<String> probabilities = new List<string>();
        int randNumber = UnityEngine.Random.Range(1,101);
        string key = null;
        foreach (string stateKey in states.Keys)
        {
            if (randNumber <= states[stateKey]){
                probabilities.Add(stateKey);
            }
        }
        if(probabilities.Count != 0){
            key = probabilities[UnityEngine.Random.Range(0,probabilities.Count)];
        }
        return key;
    }
}
