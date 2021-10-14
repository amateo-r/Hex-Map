using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldResourceManager : MonoBehaviour
{
    public List<Resource>			lst_natural_resources;
	
	public t_resources_biomes []	dic_nr_biome; // Natural resources by biome.

	[System.Serializable]
	public struct t_resources_biomes
	{
		public string			biome;
		public List<Resource>	resources;
	}
}
