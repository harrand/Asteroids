using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicControl : MonoBehaviour
{
    private Rigidbody2D rigid_body;

    public enum RotationDirection : uint { NONE = 0, CLOCKWISE = 1, ANTICLOCKWISE = 2 }
    public RotationDirection GetRotationDirection { get; private set; }
    public bool IsAccelerating { get; private set; }

    public string clockwise_rotation_key, anticlockwise_rotation_key, accelerate_key;
    public float acceleration_force;
    public float angular_velocity;

	void Start ()
    {
        this.rigid_body = this.gameObject.AddComponent<Rigidbody2D>();
        this.rigid_body.gravityScale = 0.0f;
        this.rigid_body.drag = 1.0f;
        this.GetRotationDirection = RotationDirection.NONE;
    }
	
	void Update ()
    {
        Vector2 forward_direction = this.gameObject.transform.forward.normalized;
        if (Input.GetKeyDown(this.accelerate_key))
            this.IsAccelerating = true;
        if (Input.GetKeyUp(this.accelerate_key) && this.IsAccelerating)
            this.IsAccelerating = false;

        if (Input.GetKeyDown(this.clockwise_rotation_key))
        {
            // this.gameObject.transform.Rotate(Vector3.forward * this.angular_velocity);
            this.GetRotationDirection = RotationDirection.CLOCKWISE;
        }
        else if (Input.GetKeyDown(this.anticlockwise_rotation_key))
        {
            //this.gameObject.transform.Rotate(Vector3.forward * -this.angular_velocity);
            this.GetRotationDirection = RotationDirection.ANTICLOCKWISE;
        }

        if ((Input.GetKeyUp(this.anticlockwise_rotation_key) && this.GetRotationDirection == RotationDirection.ANTICLOCKWISE) || (Input.GetKeyUp(this.clockwise_rotation_key) && this.GetRotationDirection == RotationDirection.CLOCKWISE))
            this.GetRotationDirection = RotationDirection.NONE;
	}

    void LateUpdate()
    {
        switch(this.GetRotationDirection)
        {
            case RotationDirection.CLOCKWISE:
                this.gameObject.transform.Rotate(Vector3.forward * this.angular_velocity);
                break;
            case RotationDirection.ANTICLOCKWISE:
                this.gameObject.transform.Rotate(Vector3.forward * -this.angular_velocity);
                break;
        }
        if (this.IsAccelerating)
            this.rigid_body.AddForce(this.gameObject.transform.up.normalized * this.acceleration_force);
    }
}
