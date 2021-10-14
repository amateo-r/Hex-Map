using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Weapon")]
public class Weapon : Item
{
	public int	damage;
	public int	pt_range;		// Es para determinar si es a distancia o el área de tiles sobre las que una unidad puede alcanzar para realizar un ataque. 0 es Melee.
    public bool	one_hand;
	// Tipo de daño;
}
