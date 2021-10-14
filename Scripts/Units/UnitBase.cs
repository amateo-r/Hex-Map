using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://odininspector.com/blog/scriptable-objects-tutorial

public abstract class UnitBase : ScriptableObject
{
	public new string	name;
	public int			pt_health;
	public int			pt_move;
	public int			pt_damage;

    protected void	Move()
	{
		// move unit;
		return;
	}
}
