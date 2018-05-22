using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodes : MonoBehaviour
{
    private GameObject explosion_object;

    void Start()
    {
        this.explosion_object = null;
    }

    IEnumerator DestroyExplosionObject()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.explosion_object);
        Destroy(this.gameObject);
    }

    public void Explode()
    {
        if (this.explosion_object == null)
        {
            // just die
            this.explosion_object = Instantiate(Resources.Load("Prefabs/Explosion")) as GameObject;
            explosion_object.transform.position = this.gameObject.transform.position;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(DestroyExplosionObject());
        }
    }
}
