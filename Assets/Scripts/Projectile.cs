using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Shooter shooter;
    private uint score_increase = 0;
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
        projectile.score_increase = 0;
        projectile_object.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed) + shooter.GetComponent<Rigidbody2D>().velocity;
        return projectile;
    }

    public static Projectile Create(ControlledShooter shooter, Vector3 position, Vector3 direction, float speed, string prefab_path = "Prefabs/DefaultProjectile")
    {
        Projectile projectile = Projectile.Create((Shooter) shooter, position, direction, speed, prefab_path);
        projectile.score_increase = shooter.score_per_kill;
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
        /// The projectile collided with something that's Damageable!
        if (damageable != null)
        {
            damageable.Damage(1);
            ControlledShooter controlled_shooter = this.shooter.gameObject.GetComponent<ControlledShooter>();
            if (controlled_shooter != null && controlled_shooter.GetComponent<Scored>() != null)
                controlled_shooter.GetComponent<Scored>().score += this.score_increase;
            Destroy(this.gameObject);
        }
    }
}
