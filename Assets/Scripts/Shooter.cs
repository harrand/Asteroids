using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public void Shoot()
    {
        Projectile.Create(this, this.gameObject.transform.position, this.gameObject.transform.up, 10.0f);
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
