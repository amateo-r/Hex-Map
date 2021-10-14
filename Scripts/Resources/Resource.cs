using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New resource", menuName = "Resource")]
public class Resource : ScriptableObject
{
	public new string		name;
	public int				turn_amount;			//	Número de turnos hasta que produzca 1.
	public e_factory		tile_factory;			//	Tipo de tile que produce el recurso.
	public e_biomes			farmable_biomes;		//	Biomas donde se puede producir el recurso.
	public e_seasons		farmable_seasons;		//	Estaciones durante las que el recurso puede producirse.
	public bool				abundant;				//	Establece cuántas unidades del recurso se pueden generan en un mapa. Para true: [30, 40), para false: [5, 10).
	public List<Resource>	resource_cost;			//	[REVISAR]: Consiste básicamente en una lista de los recursos que un recurso requiere para producirse.
	public List<Resource>	resource_production;	//	[REVISAR]: Consiste en la lista de recursos que un recurso puede producir en una casilla de producción. TODO recurso puede producirse a sí mismo.
	public GameObject		gb;

	public enum e_factory
	{
		none,
		farm,
		plantation,
		mine,
		factory
	}

	[System.Flags]
	public enum e_biomes
	{
		none = 0x00,
		everything = 0x01,
		hot_desert = 0x02,
		arid_steppe = 0x04,
		savanna = 0x08,
		continental = 0x10,
		tropical_rainforest = 0x20,
		subpolar = 0x40,
		polar = 0x80
	}

	[System.Flags]
	public enum e_seasons
	{
		none = 0x00,
		spring = 0x01,
		summer = 0x02,
		autumn = 0x04,
		winter = 0x08
	}
}
