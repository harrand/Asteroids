using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityToggle : MonoBehaviour
{
    GameObject canvas_object;

    void Start()
    {
        this.canvas_object = GameObject.FindGameObjectWithTag("Escape Menu");
        this.SetInvisible();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            this.ToggleVisible();
    }

    public void SetVisible()
    {
        this.canvas_object.SetActive(true);
    }

    public void SetInvisible()
    {
        this.canvas_object.SetActive(false);
    }

    public void ToggleVisible()
    {
        this.canvas_object.SetActive(!this.canvas_object.activeSelf);
    }
}
