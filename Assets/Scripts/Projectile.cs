using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Shooter shooter;
    /**  The prefab being loaded MUST have:
        - Rigidbody Component
        - Some Collider Component
    */
    public static Projectile Create(Shooter shooter, Vector3 position, Vector3 direction, float speed, string prefab_path = "Prefabs/DefaultProjectile")
    {
        GameObject projectile_object = Instantiate(Resources.Load(prefab_path)) as GameObject;
        projectile_object.transform.position = position;
        Projectile projectile = projectile_object.AddComponent<Projectile>();
        Physics2D.IgnoreCollision(shooter.GetComponent<BoxCollider2D>(), projectile.GetComponent<BoxCollider2D>());
        projectile.shooter = shooter;
        projectile_object.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed) + shooter.GetComponent<Rigidbody2D>().velocity;
        return projectile;
    }

    void Update()
    {
        if (!this.GetComponent<SpriteRenderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
    }
	
	void OnTriggerStay2D(Collider2D other)
    {        
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Debug.Log(other.name);
        /// The projectile collided with something that's Damageable!
        if (damageable != null)
        {
            Debug.Log("die");
            damageable.Damage(1);
            Destroy(this.gameObject);
        }
    }
}
