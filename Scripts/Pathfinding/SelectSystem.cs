using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSystem : MonoBehaviour
{
	public Camera			cam;
	public MapGenerator		map;
	public Toggle			move_on;
	public BFSPathfinding	path;

    private Ray				ray;
	private Unit			unit;
	private Vector3Int		vi_start;
	private Vector3Int		vi_destiny;
	private bool			unit_select;

    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hitUnitInfo) && unit_select == false)
				if (hitUnitInfo.collider.gameObject.transform.GetComponent<Unit>())
				{
					Debug.Log("Funciona para unit.");
					unit = hitUnitInfo.collider.gameObject.transform.GetComponent<Unit>();
					unit_select = true;
				}
		}
		if(move_on.isOn && unit_select)
			if (Input.GetMouseButtonDown(1))
			{
				ray = cam.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
					if (!hitInfo.collider.gameObject.GetComponent<Unit>())
					{
						vi_start = map.hexMap.WorldToCell(unit.transform.position);
						vi_destiny = map.hexMap.WorldToCell(hitInfo.transform.position);
						Debug.Log("Coordenadas de unit: " + vi_start.ToString());
						Debug.Log("Coordenadas de destino: " + vi_destiny.ToString());
						path.Search(vi_start, vi_destiny);
						//path.Move();
						path.CleanSearch();
					}
						
				}
			}
    }
}
