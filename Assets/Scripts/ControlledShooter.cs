using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledShooter : Shooter
{
    public string shoot_key;

	void Update ()
    {
        if (Input.GetKeyDown(shoot_key))
            this.Shoot();
	}
}
