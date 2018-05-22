using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UFO : MonoBehaviour
{
    public Damageable damageable { get; private set; }
    public float speed;
    public uint score_on_hit, score_on_kill;
    public float shoot_rate, shoot_accuracy;

    // Use this for initialization
    void Start()
    {
        this.damageable = this.gameObject.GetComponent<Damageable>();
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        this.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0).normalized;
        InvokeRepeating("ShootPlayer", 0.5f, 1.0f / this.shoot_rate);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        if (!this.damageable.IsAlive())
        {
            CancelInvoke();
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

    void ShootPlayer()
    {
        Shooter shooter = this.GetComponent<Shooter>();
        if (shooter == null || this.GetPlayer() == null)
            return;
        this.gameObject.transform.up = (this.GetPlayer().gameObject.transform.position - this.gameObject.transform.position).normalized;
        // if accuracy is between 0-1, have a random angle offset between 0-90 corresponding to the accuracy.
        float angle = 0.0f;
        if (this.shoot_accuracy <= 1.0f)
        {
            angle = 90 + (this.shoot_accuracy * -90);
            angle = Random.Range(-angle, angle);
            this.gameObject.transform.Rotate(0, 0, angle);
        }
        else
        {
            float speed = 10.0f;
            Vector3 current_forward = this.gameObject.transform.up;
            Vector3 player_velocity = this.GetPlayer().GetComponent<Rigidbody2D>().velocity;
            float distance_from_player = (this.gameObject.transform.position - this.GetPlayer().gameObject.transform.position).magnitude;
            float time = (distance_from_player / speed) + 1 / this.shoot_accuracy;
            // calculated predicted displacement, but weight it depending on how much greater
            Vector3 predicted_player_displacement = player_velocity * time;
            Vector3 predicted_player_location = this.GetPlayer().gameObject.transform.position + predicted_player_displacement;
            this.gameObject.transform.up = (predicted_player_location - this.gameObject.transform.position).normalized;
        }
        // but if the accuracy is greater than 1, have it predict where the player will go, with prediction quality increasing as accuracy increases.
        shooter.Shoot();
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
        if (this.GetPlayer() == null)
            return;
        Vector3 player_position = this.GetPlayer().gameObject.transform.position;
        // playerpos = thispos + toplayer
        // thus: toplayer = playerpos - thispos
        Vector3 to_player = (player_position - this.gameObject.transform.position).normalized;
        this.GetComponent<Rigidbody2D>().AddForce(to_player * this.speed);
    }
}
