using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileData
{
    public string			biome;
	public string			terrain_type;
	public string			fertility;
	public bool				continental; // water : false or ground : true
	public bool				forest;
	public Resource			resource;
	public int				x;
	public int				y;
	public int				river_direction;
	public	Vector3Int []	nb;

	public			HexTileData (string terrain_type, string biome, bool continental, int x, int y)
	{
		this.terrain_type = terrain_type;
		this.biome = biome;
		this.fertility = "fertile";
		this.continental = continental;
		this.forest = false;
		this.resource = null;
		this.x = x;
		this.y = y;
		this.river_direction = -1;
		this.nb = new Vector3Int [6];
	}

	/// <summary> Guarda los vecinos de la tile correspondiente. </summary>
	public void		SetNeightbours(Vector3Int position, int row, int mapWidth, int mapHeight)
	{
		int				r = 0;
		int				c = 0;

		nb[0] = (row % 2 == 0) ? new Vector3Int (-1, 1, 0) : Vector3Int.up;		// up_left
		nb[1] = (row % 2 == 0) ? Vector3Int.up : new Vector3Int (1, 1, 0);		// up_right
		nb[2] = Vector3Int.right;												// right
		nb[3] = (row % 2 == 0) ? Vector3Int.down : new Vector3Int (1, -1, 0);	// down_right
		nb[4] = (row % 2 == 0) ? new Vector3Int (-1, -1, 0) : Vector3Int.down;	// down_left
		nb[5] = Vector3Int.left;												// left
		for (int i = 0; i < 6; i++)
		{
			r = position.x + nb[i].x;
			c = position.y + nb[i].y;
			if (r >= 0 && c >= 0 && r < mapWidth && c < mapHeight)
				nb[i] = position + nb[i];
			else
				nb[i] = new Vector3Int (-1, -1, -1);
		}
		return;
	}

	/// <summary> Copia una clase </summary>
	public void	CopyClass (HexTileData copy)
	{
		this.x = copy.x;
		this.y = copy.y;
		this.biome = copy.biome;
		this.continental = copy.continental;
		this.nb = copy.nb;
	}

	/// <summary> Añade una dirección al río con un número entre 0-6, repsectivamente: up_left, up_right, right, down_right, down_left, left. </summary>
	public void	SetRiverDirection(int direction)
	{

	}
}
