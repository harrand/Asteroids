using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Should be attached to a Text object.
/// </summary>
public class LevelDisplay : MonoBehaviour
{
    public LevelSystem system;
    private Text text;

    void Start()
    {
        this.text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (system.IsCompleted)
            this.text.text = "Complete!";
        else
            this.text.text = Convert.ToString(system.GetCurrentLevel.GetLevelID);
    }
}
