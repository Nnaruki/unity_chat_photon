using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalVariables : MonoBehaviour
{ 
    //現在のHP
    static public int currentHP = 100;

    // Use this for initialization
    void Start()
    {
        VariableReset();
    }

    static public void VariableReset() //変数初期化
    {
        currentHP = 100;
    }
}