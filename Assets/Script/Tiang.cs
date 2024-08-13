using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiang : MonoBehaviour
{
    [SerializeField] GameObject tiangPrefab;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTinggiTiang(int tinggiTiang){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
            // Debug.Log(child);
        }
        for (int i = 0; i < tinggiTiang; i++)
        {
            GameObject t = Instantiate(tiangPrefab,transform);
            t.transform.localPosition = new Vector3(0,i,0); 
        }
    }
}
