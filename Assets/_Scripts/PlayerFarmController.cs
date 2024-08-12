using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFarmController : MonoBehaviour
{
    public Tilemap tm_Ground;
    public Tilemap tm_Grass;
    public Tilemap tm_Flower;

    public TileBase tb_Ground;
    public TileBase tb_Grass;
    public TileBase tb_Flower;

    private RecyclableInventoryManager recyclableInventoryManager;

    public TileMapManager tileMapManager;

    // Start is called before the first frame update
    void Start()
    {
        recyclableInventoryManager = GameObject.Find("InventoryManager").GetComponent<RecyclableInventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleFarmAction();
    }

    public void HandleFarmAction()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("cellPos:" + cellPos);

            TileBase crrTileBase = tm_Grass.GetTile(cellPos);

            if (crrTileBase == tb_Grass)
            {
                Debug.Log("Dao");
                tm_Grass.SetTile(cellPos, null);
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Ground);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("cellPos:" + cellPos);

            TileBase crrTileBase = tm_Grass.GetTile(cellPos);

            if (crrTileBase == null)
            {
                Debug.Log("Trong hoa");
                tm_Flower.SetTile(cellPos, tb_Flower);
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Flower);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("cellPos:" + cellPos);
            TileBase crrTileBase = tm_Flower.GetTile(cellPos);

            if (crrTileBase != null)
            {
                tm_Grass.SetTile(cellPos, tb_Grass);
                tm_Flower.SetTile(cellPos, null);

                //Lay item va them vao tui do
                InvenItems itemFlower = new InvenItems();
                itemFlower.Name = "Hoa 1h";
                itemFlower.description = "Hoa nay trang tri rat dep";

                Debug.Log(itemFlower.ToString());

                recyclableInventoryManager.AddInventoryItem(itemFlower);
            }
        }
    }
}
