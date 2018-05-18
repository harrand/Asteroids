using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapped : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        Vector3 position_clip_space = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        if (position_clip_space.x > 1)
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, position_clip_space.y, position_clip_space.z));
        else if (position_clip_space.x < 0)
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, position_clip_space.y, position_clip_space.z));
        if(position_clip_space.y > 1)
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position_clip_space.x, 0, position_clip_space.z));
        else if (position_clip_space.y < 0)
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position_clip_space.x, 1, position_clip_space.z));
    }
}
