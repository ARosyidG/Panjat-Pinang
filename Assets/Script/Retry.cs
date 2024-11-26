using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Retry : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;

    [Obsolete]
    void Awake(){
        button = GetComponent<Button>();
        button.onClick.AddListener(mainLagi);
    }

    [Obsolete]
    private void mainLagi()
    {
        // throw new NotImplementedException();
        Application.LoadLevel(SceneManager.GetActiveScene().name);
    }
}
