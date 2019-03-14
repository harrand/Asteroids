using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebula : MonoBehaviour
{

	void Start ()
    {
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0).normalized;
        this.GetComponent<Rigidbody2D>().angularVelocity = 64.0f * Random.Range(-Mathf.PI, Mathf.PI);
    }

    void Update()
    {
        Nebula prefabNebula = (Resources.Load("Prefabs/Nebula") as GameObject).GetComponent<Nebula>();
        float scale = (float)this.GetComponent<Damageable>().GetCurrentHealth / this.GetComponent<Damageable>().GetMaxHealth;
        this.transform.localScale = prefabNebula.transform.localScale * scale;
    }
	
	void OnTriggerEnter2D(Collider2D collider)
    {
        Rock rock = collider.gameObject.GetComponent<Rock>();
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        UFO ufo = collider.gameObject.GetComponent<UFO>();
        ControlledShooter player = collider.gameObject.GetComponent<ControlledShooter>();
        Nebula nebula = collider.gameObject.GetComponent<Nebula>();
        this.GetComponent<Damageable>().Damage(1);
        if (rock != null)
        {
            Color colour = rock.gameObject.GetComponent<SpriteRenderer>().color;
            colour.g *= 0.75f;
            rock.gameObject.GetComponent<SpriteRenderer>().color = colour;
            rock.gameObject.GetComponent<Rigidbody2D>().mass *= 0.5f;
            Destroy(rock.gameObject.GetComponent<ScreenWrapped>());
        }
        else if (bullet != null)
        {
            bullet.gameObject.AddComponent<ScreenWrapped>();
            Color colour = bullet.gameObject.GetComponent<SpriteRenderer>().color;
            colour.g *= 0.75f;
            bullet.gameObject.GetComponent<SpriteRenderer>().color = colour;
            bullet.damage *= 2;
        }
        else if (ufo != null)
        {
            ufo.gameObject.AddComponent<ScreenWrapped>();
            Color colour = ufo.gameObject.GetComponent<SpriteRenderer>().color;
            colour.g *= 0.75f;
            ufo.gameObject.GetComponent<SpriteRenderer>().color = colour;
            ufo.shoot_rate *= 100.0f;
            ufo.shoot_accuracy *= 0.01f;
            ufo.GetComponent<Rigidbody2D>().freezeRotation = false;
            ufo.gameObject.GetComponent<Rigidbody2D>().velocity *= 8.0f;
            if (ufo.gameObject.GetComponent<Damageable>().current_health == 1)
                Destroy(ufo.gameObject);
            else
                ufo.gameObject.GetComponent<Damageable>().current_health = 1;
        }
        else if (player != null)
        {
            player.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity * 4.0f;
            player.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 256.0f * Random.Range(-Mathf.PI, Mathf.PI);
            player.gameObject.GetComponent<Damageable>().Heal(1);
            Destroy(this.gameObject);
        }
        else if(nebula != null)
        {
            Damageable nebula_dmg = nebula.GetComponent<Damageable>();
            nebula_dmg.max_health *= 2;
            nebula_dmg.current_health = nebula_dmg.max_health;
            nebula.GetComponent<Rigidbody2D>().velocity *= 4.0f;
            this.GetComponent<Rigidbody2D>().velocity *= 0.25f;
        }
        else
            this.GetComponent<Damageable>().Heal(1);
    }
}
