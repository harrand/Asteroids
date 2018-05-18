using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledSprite : MonoBehaviour
{
    private Rigidbody2D rigid_body;
    public enum MovementDirection : uint { UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3 };

    public float speed;
    public string up_key, down_key, left_key, right_key;

    public HashSet<MovementDirection> CurrentDirections { get; private set; }
    private Dictionary<MovementDirection, bool> AllowedDirections;

    public void SetDirectionAllowed(MovementDirection direction, bool enabled)
    {
        this.AllowedDirections[direction] = enabled;
    }

    public bool IsDirectionAllowed(MovementDirection direction)
    {
        return this.AllowedDirections[direction];
    }

	void Start ()
    {
        this.rigid_body = this.gameObject.AddComponent<Rigidbody2D>();
        this.rigid_body.gravityScale = 0.0f;

        this.CurrentDirections = new HashSet<MovementDirection>();
        this.AllowedDirections = new Dictionary<MovementDirection, bool>();
        this.SetDirectionAllowed(MovementDirection.UP, true);
        this.SetDirectionAllowed(MovementDirection.DOWN, true);
        this.SetDirectionAllowed(MovementDirection.LEFT, true);
        this.SetDirectionAllowed(MovementDirection.RIGHT, true);
    }
	
	void Update ()
    {
        // Handles key presses

		if(Input.GetKeyDown(up_key) && !this.CurrentDirections.Contains(MovementDirection.UP) && this.IsDirectionAllowed(MovementDirection.UP))
        {
            // Handle Move Up
            //this.rigid_body.velocity.Set(this.rigid_body.velocity.x, this.speed);
            this.CurrentDirections.Add(MovementDirection.UP);
        }
        if(Input.GetKeyDown(down_key) && !this.CurrentDirections.Contains(MovementDirection.DOWN) && this.IsDirectionAllowed(MovementDirection.DOWN))
        {
            // Handle Move Down
            this.CurrentDirections.Add(MovementDirection.DOWN);
        }
        if(Input.GetKeyDown(left_key) && !this.CurrentDirections.Contains(MovementDirection.LEFT) && this.IsDirectionAllowed(MovementDirection.LEFT))
        {
            // Handle Move Left
            this.CurrentDirections.Add(MovementDirection.LEFT);
        }
        if(Input.GetKeyDown(right_key) && !this.CurrentDirections.Contains(MovementDirection.RIGHT) && this.IsDirectionAllowed(MovementDirection.RIGHT))
        {
            // Handle Move Right
            this.CurrentDirections.Add(MovementDirection.RIGHT);
        }

        // Handles key releases

        if (Input.GetKeyUp(up_key) && this.CurrentDirections.Contains(MovementDirection.UP) && this.IsDirectionAllowed(MovementDirection.UP))
        {
            // Handle Move Up
            this.CurrentDirections.Remove(MovementDirection.UP);
        }
        if (Input.GetKeyUp(down_key) && this.CurrentDirections.Contains(MovementDirection.DOWN) && this.IsDirectionAllowed(MovementDirection.DOWN))
        {
            // Handle Move Down
            this.CurrentDirections.Remove(MovementDirection.DOWN);
        }
        if (Input.GetKeyUp(left_key) && this.CurrentDirections.Contains(MovementDirection.LEFT) && this.IsDirectionAllowed(MovementDirection.LEFT))
        {
            // Handle Move Left
            this.CurrentDirections.Remove(MovementDirection.LEFT);
        }
        if (Input.GetKeyUp(right_key) && this.CurrentDirections.Contains(MovementDirection.RIGHT) && this.IsDirectionAllowed(MovementDirection.RIGHT))
        {
            // Handle Move Right
            this.CurrentDirections.Remove(MovementDirection.RIGHT);
        }
    }

    void LateUpdate()
    {
        float vx = 0.0f, vy = 0.0f;
        foreach(MovementDirection direction in this.CurrentDirections)
        {
            switch(direction)
            {
                case MovementDirection.UP:
                    vy = this.speed;
                    break;
                case MovementDirection.DOWN:
                    vy = -this.speed;
                    break;
                case MovementDirection.LEFT:
                    vx = -this.speed;
                    break;
                case MovementDirection.RIGHT:
                    vx = this.speed;
                    break;
            }
        }
        this.rigid_body.velocity = new Vector2(vx, vy);
    }
}
