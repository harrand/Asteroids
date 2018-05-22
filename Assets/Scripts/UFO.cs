using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UFO : MonoBehaviour
{
    public Damageable damageable { get; private set; }
    public float speed;
    public uint score_on_hit, score_on_kill;

    // Use this for initialization
    void Start()
    {
        this.damageable = this.gameObject.GetComponent<Damageable>();
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = this.gameObject.transform.rotation.eulerAngles;
        rotation.z = 0.0f;
        this.gameObject.transform.rotation = Quaternion.Euler(rotation);
        if (!this.damageable.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }

    void LateUpdate()
    {
        /// Sometimes collisions move the z-coordinate past the camera far-clip, so we lock the z coordinate at 0.
        Vector3 position = this.gameObject.transform.position;
        position.z = 0;
        this.gameObject.transform.position = position;

        SpriteRenderer sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.GetComponent<BoxCollider2D>().size = sprite_renderer.bounds.size / 4.0f;
        this.HandleMovement();
    }

    private ControlledShooter GetPlayer()
    {
        foreach (GameObject game_object in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            ControlledShooter player_component = game_object.GetComponent<ControlledShooter>();
            if (player_component != null)
                return player_component;
        }
        return null;
    }

    void HandleMovement()
    {
        Vector3 player_position = this.GetPlayer().gameObject.transform.position;
        // playerpos = thispos + toplayer
        // thus: toplayer = playerpos - thispos
        Vector3 to_player = (player_position - this.gameObject.transform.position).normalized;
        this.GetComponent<Rigidbody2D>().AddForce(to_player * this.speed);
    }
}
