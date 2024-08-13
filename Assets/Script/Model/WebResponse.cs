using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WebResponse<T>
{
    public T data;
    public String error;
}
