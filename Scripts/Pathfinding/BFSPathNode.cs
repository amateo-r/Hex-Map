using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSPathNode : MonoBehaviour
{
    public Vector2Int	position;
	public int			cost;
	public int			came_from;	// Direction between 0-6. -1 if is start node.
	public bool			searched;	// Stablish if a node can be used for search. It is used mainly for not research nodes searched.

	public BFSPathNode()
	{
		searched = true;
	}

	public	BFSPathNode(Vector2Int position, int cost, int came_from, bool searched = true)
	{
		this.position = position;
		this.cost = cost;
		this.came_from = came_from;
		this.searched = searched;
	}
}
