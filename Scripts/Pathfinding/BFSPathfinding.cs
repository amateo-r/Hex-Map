using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFSPathfinding : MonoBehaviour
{
	public MapGenerator			map;

	private List<HexTileData>	lst_frontier;
	private List<HexTileData>	lst_reached;
	private List<HexTileData>	lst_path;
	private	Unit				unit;
	private BFSPathNode [,]		paths;

	void	Start()
	{
		InitPaths();
	}

	///<summary> Busca el camino más eficiente al destino selecionado por el jugador. </summary>
	public void		Search(Vector3Int vi_start, Vector3Int vi_destiny)
	{
		HexTileData	current = map.Layout[vi_start.x, vi_start.y];
		HexTileData	start = map.Layout[vi_start.x, vi_start.y];
		HexTileData	destiny = map.Layout[vi_destiny.x, vi_destiny.y];

		lst_frontier = new List<HexTileData>();
		lst_reached = new List<HexTileData>();
		lst_frontier.Add(current);
		lst_reached.Add(current);
		paths[vi_start.x, vi_start.y] = new BFSPathNode(new Vector2Int(vi_start.x, vi_start.y), 0, -1, false);

		// int	j = 500;
		while (!lst_reached.Contains(destiny)) //  && j-- > 0
		{
			Debug.Log("Current [" + current.x + ", " + current.y +"]: " + current.biome + " " + current.terrain_type);
			for (int i = 0; i < current.nb.Length; i++)
			{
				if (current.nb[i] != new Vector3Int(-1, -1, -1))
					if (paths[current.nb[i].x, current.nb[i].y].searched)
						if (!map.Layout[current.nb[i].x, current.nb[i].y].terrain_type.Contains("Mountain"))
						{
							paths[current.nb[i].x, current.nb[i].y] = new BFSPathNode(new Vector2Int(current.nb[i].x, current.nb[i].y), paths[current.x, current.y].cost + 1, (i < 3)? i + 3 : i - 3, false);
							lst_frontier.Add(map.Layout[current.nb[i].x, current.nb[i].y]);
							lst_reached.Add(map.Layout[current.nb[i].x, current.nb[i].y]);
						}
			}
			lst_frontier.Remove(current);
			current = lst_frontier[0];
		}
		MakePath(destiny, start);
		PrintPath();
	}

	///<summary> Mueve la unidad selecionada al destino marcado por el jugador. </summary>
	public void		Move()
	{
		// Cosas de Move:
		//	0. Hay seis direcciones en las que el bicho debe moverse.
		//	1. Animaciones.
		//	2. Fluidez en el cambio de dirección del movimiento.
		//	3. Diferencia entre continental y oceanic.
		return;
	}

	///<summary> Reset all lst and paths to remake a path. </summary>
	public void		CleanSearch()
	{
		InitPaths();
		lst_frontier.Clear();
		lst_reached.Clear();
		lst_path.Clear();
		return;
	}

	///<summary> Inicialize all the objects of array paths to default settings. </summary>
	private void	InitPaths()
	{
		paths = new BFSPathNode [map.mapWidth, map.mapHeight];
		for (int y = 0; y < map.mapHeight; y++)
			for (int x = 0; x < map.mapWidth; x++)
				paths[x, y] = new BFSPathNode();
		return;
	}

	///<summary> Search and place the possible way from destiny to start. </summary>
	private void	MakePath(HexTileData destiny, HexTileData start)
	{
		HexTileData	current = destiny;

		lst_path = new List<HexTileData>();
		lst_path.Add(current);
		while (!lst_path.Contains(start))
		{
			if (paths[current.x, current.y].came_from > -1)
				current = map.Layout[current.nb[paths[current.x, current.y].came_from].x, current.nb[paths[current.x, current.y].came_from].y];
			lst_path.Add(current);
		}
		return;
	}

	///<summary> [DEV_PURPOSE] Print path in debug </summary>
	private void	PrintPath()
	{
		Debug.Log("La ruta que debe seguirse es: ");
		foreach (HexTileData item in lst_path)
			Debug.Log("Ruta: [" + item.x + ", " + item.y + "]: " + item.terrain_type + " " + item.biome);
		return;
	}
}
