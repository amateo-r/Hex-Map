using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
	public GameObject	path_system;

	bool				move_on = false;

    public void	MoveArmy()
	{
		move_on = !move_on;
		if (move_on)
			path_system.SetActive(true);
		else
			path_system.SetActive(false);
		return;
	}
}
