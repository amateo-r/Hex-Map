using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexTest : MonoBehaviour
{

	public	Tilemap 	hexMap;
	public	GameObject	modelData;

    // Start is called before the first frame update
    void Start()
    {
		Tile ground = new Tile();
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
        hexMap.SetTransformMatrix(new Vector3Int(0, 0, 0), matrix);


		ground.gameObject = modelData;
		for (int x = 0; x < 300; x++)
			for (int y = 0; y < 300; y++)
				hexMap.SetTile(new Vector3Int(x, y, 0), ground);
    }
}
