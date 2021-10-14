using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Unit/Rider")]
public class Rider : UnitBase
{
    public int	speed;

	public void	Charge()
	{
		Debug.Log ("Ha funcionado para el jinete " + name);
	}
}
