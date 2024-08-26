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

    public List<TileBase> lstTb_Pumpkin;

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
                //tm_Flower.SetTile(cellPos, tb_Flower);
                StartCoroutine(GrowPlant(cellPos, tm_Flower, lstTb_Pumpkin));
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Pumpkin);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3Int cellPos = tm_Ground.WorldToCell(transform.position);
            Debug.Log("cellPos:" + cellPos);
            TileBase crrTileBase = tm_Flower.GetTile(cellPos);

            if (crrTileBase == lstTb_Pumpkin[4])
            {
                tm_Grass.SetTile(cellPos, tb_Grass);
                tm_Flower.SetTile(cellPos, null);

                //Lay item va them vao tui do
                InvenItems itemPumpkin = new InvenItems();
                itemPumpkin.Name = "Bi do";
                itemPumpkin.description = "Bi do an rat ngon!";

                Debug.Log(itemPumpkin.ToString());

                recyclableInventoryManager.AddInventoryItem(itemPumpkin);
                tileMapManager.SetStateForTilemapDetail(cellPos.x, cellPos.y, TilemapState.Pumpkin);
            }
        }
    }

    public IEnumerator GrowPlant(Vector3Int cellPos, Tilemap tilemap, List<TileBase> lstTilebase)
    {
        int crrStage = 0;

        while(crrStage < lstTilebase.Count)
        {
            tilemap.SetTile(cellPos, lstTilebase[crrStage]);
            yield return new WaitForSeconds(5);
            crrStage++;
        }
    }
}
