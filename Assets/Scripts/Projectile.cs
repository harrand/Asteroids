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

    public static Projectile Create(ControlledShooter shooter, Vector3 position, Vector3 direction, float speed, string prefab_path = "Prefabs/DefaultProjectile")
    {
        Projectile projectile = Projectile.Create((Shooter) shooter, position, direction, speed, prefab_path);
        Physics2D.IgnoreCollision(shooter.GetComponent<BoxCollider2D>(), projectile.GetComponent<BoxCollider2D>());
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
        if (this.shooter == null)
            return;
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Rock rock = other.gameObject.GetComponent<Rock>();
        UFO ufo = other.gameObject.GetComponent<UFO>();
        ControlledShooter controlled_shooter = this.shooter.gameObject.GetComponent<ControlledShooter>();
        /// The projectile collided with something!
        if (damageable != null)
        {
            damageable.Damage(1);
            if (rock != null)
            {
                if (controlled_shooter != null && controlled_shooter.GetComponent<Scored>() != null)
                {
                    uint value = 0;
                    switch (rock.rock_type)
                    {
                        case Rock.RockType.LARGE:
                            value = controlled_shooter.score_per_large_asteroid_kill;
                            break;
                        case Rock.RockType.MEDIUM:
                            value = controlled_shooter.score_per_medium_asteroid_kill;
                            break;
                        case Rock.RockType.SMALL:
                            value = controlled_shooter.score_per_small_asteroid_kill;
                            break;
                    }
                    controlled_shooter.GetComponent<Scored>().score += value;
                }
            }
            else if(ufo != null && controlled_shooter != null)
            {
                Scored scored = controlled_shooter.GetComponent<Scored>();
                if (scored != null)
                {
                    scored.score += ufo.score_on_hit;
                    if (!ufo.damageable.IsAlive())
                        controlled_shooter.GetComponent<Scored>().score += ufo.score_on_kill;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
