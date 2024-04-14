using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapInteractor : MonoBehaviour
{
    private Tilemap map;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")) {
            Vector3Int gridPosition = map.WorldToCell(player.position);
            TileBase currentTile = map.GetTile(gridPosition);
            Debug.Log(currentTile);
        }
    }
}
