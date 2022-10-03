using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverResult : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "You have survived "+ScoreController.GetInstance().GetScore() + " seconds.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
