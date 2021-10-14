using System.Collections.Generic;
using System.Linq;
using sys = System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int				mapWidth;
    public int				mapHeight;
    public float			noiseScale;

    public int				octaves;
    [Range (0,1)]
    public float			persistance; // multiplayer
    public float			lacuranity; // refinement

    [Range (-100000, 100000)]
    public int				seed;
    public Vector2			offset;

    [SerializeField]
	public Tilemap			hexMap;
    public TerrainType []	regions; // Serían las tiles
	public BiomeType []		biomes;
	public float			terrain_level;
	public GameObject		gb_river;

	public WorldResourceManager	wr_manager;

	private	HexTileData [,]	layout;
	public HexTileData [,]	Layout { get { return layout; }}
	private Vector3Int		vi_null = new Vector3Int (-1, -1, -1);
	private List<BiomeType>	lst_biomes_gen = new List<BiomeType>();

    void			Start ()
	{
        GenerateMap();
    }

    public void		GenerateMap()
	{
        float [,]			noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, RandomSeed(), noiseScale, octaves, persistance, lacuranity, offset);
		bool				continental = false;
		Tile				ground = null;
        Matrix4x4			matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
		List<HexTileData>	lst_mountains = new List<HexTileData>();
		List<HexTileData>	lst_groundtiles = new List<HexTileData>();
		string				terrain_type_tmp = "";

		layout = new HexTileData [mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
		{
            for (int x = 0; x < mapWidth; x++)
			{
				continental = false;
				terrain_type_tmp = "water";
				if (noiseMap[x, y] > terrain_level)
				{
					for (int j = 0; j < regions.Length; j++)
						if (noiseMap[x, y] < regions[j].height)
						{
							continental = true;
							terrain_type_tmp = regions[j].name;
							break;
						}
				}
				else
					terrain_type_tmp = regions[0].name;
				layout[x, y] = new HexTileData (terrain_type_tmp, "water", continental, x, y);
				layout[x, y].SetNeightbours(new Vector3Int (x, y, 0), y, mapWidth, mapHeight);
				if (layout[x, y].continental)
					lst_groundtiles.Add(layout[x, y]);
				if (layout[x, y].terrain_type == regions[regions.Length - 1].name)
				{
					lst_mountains.Add(layout[x, y]);
					lst_groundtiles.Remove(layout[x, y]);
				}
            }
        }
		SetBiome(layout);
		SetRivers(noiseMap, lst_mountains, lst_groundtiles);
		hexMap.SetTransformMatrix(new Vector3Int(0, 0, 0), matrix);
		for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
			{
				ground = null;
				if (layout[x, y].continental)
				{
					ground = new Tile();
					if (layout[x, y].terrain_type == regions[regions.Length - 1].name)
						ground.gameObject = regions[regions.Length - 1].tile;
					else if (layout[x, y].terrain_type == "river")
						ground.gameObject = gb_river;
					else
					{
						for (int j = 0; j < biomes.Length; j++)
							if (layout[x, y].biome == biomes[j].name)
								ground.gameObject = biomes[j].tile;
					}
					hexMap.SetTile(new Vector3Int (x, y, 0), ground);
				}
			}
		SetForest(lst_groundtiles);
		SetResources(lst_groundtiles);
        return;
    }

	/// <summary> Genera un valor númerico aleatorio que modifica el patrón de ruido del mapa </summary>
    private int		RandomSeed()
	{
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const int maxSeed = 6;

        string string_r = "";
        int i = 0;
        System.Random prng;

        if (seed == 0)
		{
            for ( i = 0; i < 15; i++)
                string_r += chars[Random.Range(0, chars.Length)];

            string temp_r = "" + string_r.GetHashCode();
            i = 0;
            if (temp_r.IndexOf("-") == 0)
                i = 1;
            seed = int.Parse(temp_r.Substring(0, maxSeed + i));

            prng = new System.Random(seed);
            return prng.Next(-100000, 100000);
        }
        else
		{
            prng = new System.Random(seed);
            return prng.Next(-100000, 100000);
        }
    }

	/// <summary> Adjunta tiles a una región para crear un bioma. </summary>
	private void	SetBiome(HexTileData [,] layout)
	{
		int				number_of_climates = 0;
		int				rand_coord = 0;
		float			dist = 0;
		float			cdist = 0;
		float			xdiff = 0;
		float			ydiff = 0;
		float			distorsion = 0;
		string			nearest_biome = "";
		float [,]		noise_frontier = Noise.GenerateNoiseMap (mapWidth, mapHeight, RandomSeed(), 10, 1, 5, 5, offset);

		for (int i = 0; i < biomes.Length; i++)
		{
			biomes[i].x = -1;
			biomes[i].y = -1;
		}

		number_of_climates = Random.Range(3, biomes.Length + 1);
		for (int i = 0; i < number_of_climates; i++)
		{
			rand_coord = Random.Range(0, biomes.Length);
			while (biomes[rand_coord].x != -1)
				rand_coord = Random.Range(0, biomes.Length);
			biomes[rand_coord].x = Random.Range(0, mapWidth);
			biomes[rand_coord].y = Random.Range(0, mapHeight);
			layout[biomes[rand_coord].x, biomes[rand_coord].y].biome = biomes[rand_coord].name;
			lst_biomes_gen.Add(biomes[rand_coord]);
		}

		for (int y = 0; y < mapHeight; y++)
			for (int x = 0; x < mapWidth; x++)
			{
				distorsion = noise_frontier[x, y];
				nearest_biome = ".";
				dist = int.MaxValue;
				for (int z = 0; z < biomes.Length; z++)
				{
					if (biomes[z].x != -1 && biomes[z].y != -1)
					{
						xdiff = biomes[z].x - (x + distorsion * 5);
						ydiff = biomes[z].y - (y + distorsion * 5);
						cdist = xdiff * xdiff + ydiff * ydiff;
						if (cdist < dist)
						{
							nearest_biome = biomes[z].name;
							dist = cdist;
						}
						distorsion += (noise_frontier[x, y]) / 5;
					}
				}
				layout[x, y].biome = nearest_biome;
				if (layout[x, y].biome == "polar" && layout[x, y].biome == "hot desert" && layout[x, y].continental)
					layout[x, y].fertility = "sterile";
			}
		return;
	}

	/// <summary> Añade ríos empezando por una montaña en lista lst_mountains. </summary>
	private void	SetRivers(float [, ] noiseMap, List<HexTileData> lst_mountains, List<HexTileData> lst_groundtiles)
	{
		int					index = 0;
		int					num_rivers = 20; //Random.Range (0, 20);
		int					min_x = 0;
		int					min_y = 0;
		int					attemps = 0;
		int					count = 0;
		int					dir = 0;
		float				min_height = 0;
		float				min_h_control = 0;
		HexTileData			current = null;
		List<HexTileData>	lst_river = new List<HexTileData>();
		List<int>			lst_river_dirs = new List<int>();

		for (int i = 0; i < num_rivers; i++)
		{
			index = Random.Range(0, lst_mountains.Count);
			current = lst_mountains[index];
			attemps = 1000;
			lst_river.Clear();
			lst_river_dirs.Clear();
			while (current.continental && attemps > 0)
			{
				min_height = noiseMap[current.x, current.y];
				for (int j = 0; j < current.nb.Length; j++)
				{
					if (current.nb[j] != new Vector3Int (-1, -1, -1))
						if (min_height >= noiseMap[current.nb[j].x, current.nb[j].y] && layout[current.nb[j].x, current.nb[j].y].terrain_type != "river")
						{
							min_height = noiseMap[current.nb[j].x, current.nb[j].y];
							min_x = current.nb[j].x;
							min_y = current.nb[j].y;
							dir = j;
						}
				}
				if (min_h_control == min_height)
				{
					int	j = 0;
					for (j = 0; j < current.nb.Length; j++)
					{
						if (current.nb[j] != new Vector3Int (-1, -1, -1))
						{
							if (layout[current.nb[j].x, current.nb[j].y].terrain_type != "river")
							{
								min_height = noiseMap[current.nb[j].x, current.nb[j].y];
								min_x = current.nb[j].x;
								min_y = current.nb[j].y;
								dir = j;
							}
						}
					}
				}
				min_h_control = min_height;
				lst_river.Add(layout[min_x, min_y]);
				lst_river_dirs.Add(dir);
				current = layout[min_x, min_y];
				attemps--;
			}
			if (!lst_river.Last().continental)
			{
				for (int j = 0; j < lst_river.Count; j++)
				{
					layout[lst_river[j].x, lst_river[j].y].terrain_type = "river";
					layout[lst_river[j].x, lst_river[j].y].river_direction = lst_river_dirs[j];
					layout[lst_river[j].x, lst_river[j].y].fertility = "very fertile";
					for (int k = 0; k < lst_river[j].nb.Length; k++)
					{
						if (lst_river[j].nb[k] != vi_null && layout[lst_river[j].nb[k].x, lst_river[j].nb[k].y].biome != "polar")
							layout[lst_river[j].nb[k].x, lst_river[j].nb[k].y].fertility = "very fertile";
					}
					lst_mountains.Remove(layout[lst_river[j].x, lst_river[j].y]);
					lst_groundtiles.Add(layout[lst_river[j].x, lst_river[j].y]);
				}
				count++;
			}
			else
				i--;
		}
		Debug.Log("Número de ríos: " + count);
		return;
	}

	/// <summary> Añade bosques en función de la humedad de un bioma. </summary>
	private void	SetForest(List<HexTileData> lst_groundtiles)
	{
		float [,]	noise_humidity = Noise.GenerateNoiseMap (mapWidth, mapHeight, RandomSeed(), 10, 1, 5, 5, offset);
		int			i = 0;
		Vector3		cell_position = new Vector3(0, 0.4f, 0);

		foreach (HexTileData item in lst_groundtiles)
		{
			cell_position = new Vector3(0, 0.4f, 0);
			for (i = 0; i < biomes.Length; i++)
			{
				if (noise_humidity[item.x, item.y] < biomes[i].humidity && item.biome == biomes[i].name)
				{
					item.forest = true;
					cell_position += hexMap.CellToWorld(new Vector3Int(item.x, item.y, 0));
					Instantiate(biomes[i].tree_type, cell_position, Quaternion.identity);
					break;
				}
			}
		}
		return;
	}

	/// <summary> Añade aleatoriamente recursos al mapa en función de sus características. </summary>
	private void	SetResources(List<HexTileData>	lst_groundtiles)
	{
		var		test_0 = wr_manager.lst_natural_resources[1].farmable_seasons;
		string	test_1 = wr_manager.lst_natural_resources[1].farmable_biomes.ToString();
		
		int				res_amount = lst_groundtiles.Count * 3 / 7; // (3/7) == 42% of ground landmass.
		int				i = 0;
		int				j = 0;
		int				k = 0;
		int				res_abundance = 0;
		string			biome_name = "";
		Resource		current = null;
		Vector3			cell_position = new Vector3(0, 0.4f, 0);
		List<HexTileData>	lst_freetiles = new List<HexTileData>();

		for (i = 0; i < res_amount; i++)
		{
			j = Random.Range (0, lst_groundtiles.Count);
			while (lst_freetiles.Contains(lst_groundtiles[j]))
				j = Random.Range (0, lst_groundtiles.Count);
			lst_freetiles.Add(lst_groundtiles[j]);
		}
		for (j = 0; j < res_amount; j++)
		{
			biome_name = "";
			if (Random.Range (0, 3) > 1)
			{
				i = 0;
				biome_name = "all";
			}
			else
			{
				i = -1;
				biome_name = lst_biomes_gen[Random.Range (0, lst_biomes_gen.Count)].name;
				while (wr_manager.dic_nr_biome[++i].biome != biome_name);
			}
			if (wr_manager.dic_nr_biome[i].resources.Count > 0)
			{
				current = wr_manager.dic_nr_biome[i].resources[Random.Range (0, wr_manager.dic_nr_biome[i].resources.Count)];
				res_abundance = (current.abundant) ? Random.Range (30, 41) : Random.Range (5, 11);
				k = 0;
				while (k < res_abundance)
				{
					cell_position = new Vector3(0, 0.4f, 0);
					if ((lst_freetiles[k].biome == biome_name || biome_name == "all") && lst_freetiles[k].continental)
					{
						cell_position += hexMap.CellToWorld(new Vector3Int(lst_freetiles[k].x, lst_freetiles[k].y, 0));
						layout[lst_freetiles[k].x, lst_freetiles[k].y].resource = current;
						Instantiate(current.gb, cell_position, Quaternion.identity);
					}
					k++;
				}
				lst_freetiles.RemoveRange(0, k);
				res_amount -= res_abundance;
			}
		}
		return;
	}
}

[System.Serializable]
public struct	TerrainType
{
    public string		name;
    public float		height;
	public GameObject	tile;
}

[System.Serializable]
public struct	BiomeType
{
    public string		name;
    public GameObject	tile;
	public int			x;
	public int			y;
	public float		humidity;
	public GameObject	tree_type;
}
