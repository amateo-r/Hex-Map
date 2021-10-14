using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform	cam_transform;

	public float		normal_speed;
	public float		fast_speed;
	public float		move_speed;
	public float		move_time;
	public float		rotation_amount;
	public Vector2		pos_limit;
	public Vector2		zoom_limit;
	public Vector3		zoom_amount;

	public Vector3		new_position;
	public Quaternion	new_rotation;
	public Vector3		new_zoom;

    // Start is called before the first frame update
    void		Start()
    {
        new_position = transform.position;
		new_rotation = transform.rotation;
		new_zoom = cam_transform.localPosition;
    }

    // Update is called once per frame
    void		Update()
    {
        HandlerKeyMovement();
		HandlerMouseMovement();
    }

	/// <summary> Collect all action inputs where camera is moved by keyboard. </summary>
	public void	HandlerKeyMovement()
	{
		if (Input.GetKey(KeyCode.LeftShift))
			move_speed = fast_speed;
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			new_position += (transform.forward * move_speed);
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			new_position += (transform.forward * -move_speed);
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			new_position += (transform.right * move_speed);
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			new_position += (transform.right * -move_speed);
		if (Input.GetKey(KeyCode.Q))
			new_rotation *= Quaternion.Euler(Vector3.up * rotation_amount);
		if (Input.GetKey(KeyCode.E))
			new_rotation *= Quaternion.Euler(Vector3.up * -rotation_amount);
		if (Input.GetKey(KeyCode.R))
			new_zoom += zoom_amount;
		if (Input.GetKey(KeyCode.F))
			new_zoom -= zoom_amount;
		
		new_position.x = Mathf.Clamp(new_position.x, 0, pos_limit.x);
		new_position.z = Mathf.Clamp(new_position.z, 30, pos_limit.y);
		new_zoom.y = Mathf.Clamp(new_zoom.y, -5, zoom_limit.x);
		new_zoom.z = Mathf.Clamp(new_zoom.z, zoom_limit.y, -35);
		transform.position = Vector3.Lerp(transform.position, new_position, Time.deltaTime * move_time);
		transform.rotation = Quaternion.Lerp(transform.rotation, new_rotation, Time.deltaTime * move_time);
		cam_transform.localPosition = Vector3.Lerp(cam_transform.localPosition, new_zoom, Time.deltaTime * move_time);
		return;
	}

	/// <summary> Collect all action inputs where camera is moved by mouse. </summary>
	public void	HandlerMouseMovement()
	{
		if (Input.mouseScrollDelta.y != 0)
			new_zoom += Input.mouseScrollDelta.y * (zoom_amount * 3);
	}
}
