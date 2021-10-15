using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject []		bottom_rooms;
	public GameObject []		top_rooms;
	public GameObject []		left_rooms;
	public GameObject []		right_rooms;
	public GameObject			closedRoom;
	public List<GameObject>		rooms;
	public float				wait_time;
	public GameObject			boss;

	private bool				spawned_boss;

	void Update()
	{
		if (wait_time <= 0 && !spawned_boss)
			for (int i = 0; i < rooms.Count; i++)
			{
				if (i == rooms.Count - 1)
				{
					Instantiate(boss, rooms[i].transform.position + new Vector3(0, 0, -1), Quaternion.identity);
					spawned_boss = true;
				}
			}
		else if (!spawned_boss)
			wait_time -= Time.deltaTime;
	}
}
