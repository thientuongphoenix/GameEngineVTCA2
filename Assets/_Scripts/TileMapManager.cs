using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapManager : MonoBehaviour
{
    public Tilemap tm_Ground;
    public Tilemap tm_Grass;
    public Tilemap tm_Flower;

    public TileBase tb_Flower;

    private Map map;

    private FirebaseDatabaseManager DatabaseManager;
    private FirebaseUser user;

    private DatabaseReference reference;

    public Button btnWriteMapToFirebase;
    public Button btnReadMapFromFirebase;

    private void Start()
    {
        map = new Map();

        DatabaseManager = GameObject.Find("DatabaseManager").GetComponent<FirebaseDatabaseManager>();
        user = FirebaseAuth.DefaultInstance.CurrentUser;

        //WriteAllTileMapToFirebase();

        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        LoadMapForUser();
        btnWriteMapToFirebase.onClick.AddListener(WriteAllTileMapToFirebase);
        btnReadMapFromFirebase.onClick.AddListener(LoadMapForUser);
    }

    public void WriteAllTileMapToFirebase()
    {
        List<TilemapDetail> tilemaps = new List<TilemapDetail>();

        for(int x = tm_Ground.cellBounds.min.x; x < tm_Ground.cellBounds.max.x; x++)
        {
            for(int y = tm_Ground.cellBounds.min.y; y < tm_Ground.cellBounds.max.y; y++)
            {
                //TilemapDetail tm_Detail = new TilemapDetail(x,y,TilemapState.Grass);
                //tilemaps.Add(tm_Detail);
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                TilemapState state = TilemapState.Ground; // Default state

                if (tm_Flower.GetTile(cellPos) == tb_Flower) // Check if the tile is flower
                {
                    state = TilemapState.Flower;
                }
                else if (tm_Grass.GetTile(cellPos) != null)
                {
                    state = TilemapState.Grass;
                }

                TilemapDetail tm_Detail = new TilemapDetail(x, y, state);
                tilemaps.Add(tm_Detail);
            }
        }

        map = new Map(tilemaps);
        //lstTilemapDetail.ToString();
        Debug.Log(map.ToString());

        DatabaseManager.WriteDatabase(user.UserId + "/Map", map.ToString());
    }

    public void LoadMapForUser()
    {
        reference.Child("Users").Child(user.UserId + "/Map").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) return;
            else if (task.IsFaulted) return;
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value.ToString());
                map = JsonConvert.DeserializeObject<Map>(snapshot.Value.ToString());
                Debug.Log("Load map: " + map.ToString());
                MapToUI(map);
            }
        });
        //Debug.Log("Load lstTilemapDetail: " + lstTilemapDetail.ToString());
    }

    public void TilemapDetailToTileBase(TilemapDetail tilemapDetail)
    {
        Vector3Int cellPos = new Vector3Int(tilemapDetail.x, tilemapDetail.y, 0);

        if(tilemapDetail.tilemapState == TilemapState.Ground)
        {
            tm_Grass.SetTile(cellPos, null);

            tm_Flower.SetTile(cellPos, null);
        }
        else if(tilemapDetail.tilemapState == TilemapState.Grass)
        {
            tm_Flower.SetTile(cellPos, null);
            //if(cellPos == new Vector3Int(1,1,0))
            //{
            //    tm_Grass.SetTile(cellPos, null);
            //    tm_Flower.SetTile(cellPos, tb_Flower);
            //}
            //else
            //{
            //    tm_Flower.SetTile(cellPos, null);
            //}
        }
        else if (tilemapDetail.tilemapState == TilemapState.Flower)
        {
            tm_Grass.SetTile(cellPos, null);
            tm_Flower.SetTile(cellPos, tb_Flower);
        }
    }

    public void MapToUI(Map map)
    {
        Debug.Log("Load map to UI");
        for(int i = 0; i < map.GetLength(); i++)
        {
            TilemapDetailToTileBase(map.lstTilemapDetail[i]);
        }
    }

    public void  SetStateForTilemapDetail(int x, int y, TilemapState state)
    {
        for(int i = 0; i < map.GetLength(); i++)
        {
            if (map.lstTilemapDetail[i].x == x && map.lstTilemapDetail[i].y == y)
            {
                map.lstTilemapDetail[i].tilemapState = state;
                DatabaseManager.WriteDatabase(user.UserId + "/Map", map.ToString());
                Debug.Log("Save to Firebase successful");
            }
        }
    }
}
