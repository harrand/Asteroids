using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledShooter : Shooter
{
    public string shoot_key;
    public uint score_per_kill;

    public override void Shoot()
    {
        Projectile.Create(this, this.gameObject.transform.position, this.gameObject.transform.up, 10.0f);
    }

    void Update ()
    {
        if (Input.GetKeyDown(shoot_key))
            this.Shoot();
	}
}
