using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<UnitBase>	lst_units;
	public int				index;

	void	Start()
	{
		if (lst_units[index] is Rider)
		{
			Rider unit = (Rider)lst_units[index];
			// unit.Charge();
		}
	}
}
