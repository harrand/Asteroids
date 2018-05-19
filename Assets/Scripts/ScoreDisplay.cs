using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Should be attached to a Text object.
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    public Scored target;
    private Text text;

    void Start()
    {
        this.text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
        this.text.text = Convert.ToString(target.score);
    }
}
