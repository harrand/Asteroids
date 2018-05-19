using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Damageable damageable;

    public enum RockType { SMALL = 0, MEDIUM = 1, LARGE = 2 }
    public RockType rock_type;
    public Sprite large_rock_sprite, medium_rock_sprite, small_rock_sprite;

    // Use this for initialization
    void Start()
    {
        this.damageable = this.gameObject.GetComponent<Damageable>();
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.damageable.IsAlive())
        {
            if (this.rock_type > RockType.SMALL)
            {
                // split into smaller fragments. this becomes the first daughter, and spawn a second daughter
                this.damageable.Heal(1);
                this.rock_type--;
                GameObject daughter = Instantiate(this.gameObject) as GameObject;
                //Physics2D.IgnoreCollision(daughter.GetComponent<BoxCollider2D>(), this.gameObject.GetComponent<BoxCollider2D>());
                // split their trajectories
                this.gameObject.transform.Rotate(Vector3.forward * 45);
                daughter.gameObject.transform.Rotate(Vector3.forward * -45);
            }
            else
            {
                // just die
                Destroy(this.gameObject);
            }
        }
    }

    void LateUpdate()
    {
        SpriteRenderer sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        switch (this.rock_type)
        {
            case RockType.LARGE:
                this.gameObject.name = "Large Rock";
                this.GetComponent<Rigidbody2D>().mass = 5.0f;
                sprite_renderer.sprite = this.large_rock_sprite;
                break;
            case RockType.MEDIUM:
                this.gameObject.name = "Medium Rock";
                this.GetComponent<Rigidbody2D>().mass = 3.0f;
                sprite_renderer.sprite = this.medium_rock_sprite;
                break;
            case RockType.SMALL:
                this.gameObject.name = "Small Rock";
                this.GetComponent<Rigidbody2D>().mass = 2.0f;
                sprite_renderer.sprite = this.small_rock_sprite;
                break;
        }
        this.GetComponent<BoxCollider2D>().size = sprite_renderer.bounds.size / 4.0f;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        Rock rock = other.gameObject.GetComponent<Rock>();
        /// Damage it only if it's not a rock either.
        if (damageable != null && rock == null)
        {
            /// Hurt the collidee.
            damageable.Damage(1);
        }
    }
}
