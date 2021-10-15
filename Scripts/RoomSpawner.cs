// Dar el Tag de Room a las salas.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta clase va en todos los spawn points
// Hay que añadir un spawnpoint en el medio de la sala inicial llamado Destroyer sin este script

public class RoomSpawner : MonoBehaviour
{
	public int	openingDirection; // En los puntos de spawn de una sala es siempre lo contrario. Es decir punto de spawn de top tiene un 1 porque necesita una sala con bottom.
	// bottom->1
	// top->2
	// left->3
	// right->4

	private RoomTemplates	templates;
	private int				rand;
	private bool			spawned = false;

	public float wait_time = 4f;

    void Start()
    {
		Destroy(gameObject, wait_time);
        templates = GameObject.FindGameObjectWithTag("Room Template").GetComponent<RoomTemplates>();
		Invoke("Spawn", 0.1f); // Forma de que no se creen varios spawn points. Retraso de tiempo
    }

	void Spawn()
	{
		if (spawned == false)
		{
			if (openingDirection == 1)
			{
				// Bottom door
				rand = Random.Range(0, templates.bottom_rooms.Length);
				Instantiate(templates.bottom_rooms[rand], transform.position, templates.bottom_rooms[rand].transform.rotation); // Quaternion.identity
			}
			else if (openingDirection == 3)
			{
				// Top door
				rand = Random.Range(0, templates.top_rooms.Length);
				Instantiate(templates.top_rooms[rand], transform.position, templates.top_rooms[rand].transform.rotation);
			}
			else if (openingDirection == 2)
			{
				// Left door
				rand = Random.Range(0, templates.left_rooms.Length);
				Instantiate(templates.left_rooms[rand], transform.position, templates.left_rooms[rand].transform.rotation);
			}
			else if (openingDirection == 4)
			{
				// Right door
				rand = Random.Range(0, templates.right_rooms.Length);
				Instantiate(templates.right_rooms[rand], transform.position, templates.right_rooms[rand].transform.rotation);
			}
			spawned = true;
		}	
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag("Spawn point"))
		{
			if (other.GetComponent<RoomSpawner>())
			{
				if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
				{
					// Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
					Destroy(gameObject);
				}
			}
			spawned = true;
		}
	}
}
