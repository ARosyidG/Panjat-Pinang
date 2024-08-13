using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TiangSwitch : MonoBehaviour
{   [SerializeField]
    private Button activateButton;
    [SerializeField]
    private Button deactivateButton;
    [SerializeField] int nomorTiang;
    [SerializeField] GameManager gameManager;
    [SerializeField] Tiang tiang;
    [SerializeField] TiangHandler tiangHandler;
    [SerializeField] TMP_InputField tinggiTiang;
    void Awake(){
        activateButton = transform.Find("Deactive").Find("Tambah").Find("Button").GetComponent<Button>();
        deactivateButton = transform.Find("Active").Find("Delete").GetComponent<Button>();
        tinggiTiang = transform.Find("Active").Find("TinggiTiang").GetComponent<TMP_InputField>();
        tiang = tiangHandler.tiangs[nomorTiang-1].GetComponent<Tiang>();
    }
    void Start(){
        if(nomorTiang == 1) activate();
        tinggiTiang.text = "4";
    }
    void OnEnable(){
        activateButton.onClick.AddListener(activate);
        deactivateButton.onClick.AddListener(deactivate);
        tinggiTiang.onValueChanged.AddListener(updateTinggiTiang);
    }

    private void updateTinggiTiang(string arg0)
    {
        if(tiang.enabled && int.TryParse(arg0, out _)){
            tiang.updateTinggiTiang(int.Parse(arg0));
        }
    }

    void activate(){
        transform.Find("Active").gameObject.SetActive(true);
        tiang.gameObject.SetActive(true);
        transform.Find("Deactive").gameObject.SetActive(false);
    }
    void deactivate(){
        transform.Find("Active").gameObject.SetActive(false);
        tiang.gameObject.SetActive(false);
        transform.Find("Deactive").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
