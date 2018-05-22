using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisibilityToggle : MonoBehaviour
{
    public GameObject canvas_object;

    void Start()
    {
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

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
