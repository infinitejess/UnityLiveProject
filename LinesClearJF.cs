using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LinesClearJF : MonoBehaviour
{
    public static int LinesCleared = 0; //initialize a lines clear global variable/way to keep score

    public Text lineScore; //ref UI text in scene

    private void Start() //at the beginning, if you're in the game scene, start LinesClear varibale at 0
    {
        if (SceneManager.GetActiveScene().name == "JFGame")
        {
            LinesCleared = 0;
        }
    }

    private void Update() //update score via UI text
    {
        lineScore.text = LinesCleared.ToString();
    }
}
