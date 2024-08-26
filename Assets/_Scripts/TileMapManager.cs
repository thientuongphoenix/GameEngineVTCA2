using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
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
    public List<TileBase> lstTb_Pumpkin;

    //private Map map;

    private FirebaseDatabaseManager DatabaseManager;
    //private FirebaseUser user;

    private DatabaseReference reference;

    public Button btnWriteMapToFirebase;
    public Button btnReadMapFromFirebase;

    public PlayerFarmController playerFarmController;

    private void Start()
    {
        //map = new Map();

        DatabaseManager = GameObject.Find("DatabaseManager").GetComponent<FirebaseDatabaseManager>();
        //user = FirebaseAuth.DefaultInstance.CurrentUser;

        if(LoadDataManager.userInGame.MapInGame.lstTilemapDetail != null)
        {
            LoadMapForUser();
        }
        else
        {
            WriteAllTileMapToFirebase();
        }

        FirebaseApp app = FirebaseApp.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

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

                //Vector3Int cellPos = new Vector3Int(x, y, 0);

                //TilemapState state = TilemapState.Ground; // Default state

                //if (tm_Flower.GetTile(cellPos) == tb_Flower) // Check if the tile is flower
                //{
                //    state = TilemapState.Flower;
                //}
                //else if (tm_Grass.GetTile(cellPos) != null)
                //{
                //    state = TilemapState.Grass;
                //}

                //TilemapDetail tm_Detail = new TilemapDetail(x, y, TilemapState.Grass, DateTime.Now);
                //tilemaps.Add(tm_Detail);

                TilemapDetail tm_detail = new TilemapDetail(x, y, TilemapState.Grass, DateTime.Now);
                tilemaps.Add(tm_detail);
            }
        }

        //map = new Map(tilemaps);
        //lstTilemapDetail.ToString();
        //Debug.Log(map.ToString());

        LoadDataManager.userInGame.MapInGame = new Map(tilemaps);

        DatabaseManager.WriteDatabase("Users/" + LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());
    }

    public void LoadMapForUser()
    {
        MapToUI(LoadDataManager.userInGame.MapInGame);
        //reference.Child("Users").Child(user.UserId + "/Map").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCanceled) return;
        //    else if (task.IsFaulted) return;
        //    else if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;
        //        Debug.Log(snapshot.Value.ToString());
        //        map = JsonConvert.DeserializeObject<Map>(snapshot.Value.ToString());
        //        Debug.Log("Load map: " + map.ToString());
        //        MapToUI(map);
        //    }
        //});
        ////Debug.Log("Load lstTilemapDetail: " + lstTilemapDetail.ToString());
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
        else if (tilemapDetail.tilemapState == TilemapState.Pumpkin)
        {
            double elapsedTime = DateTime.Now.Subtract(tilemapDetail.growTime).TotalSeconds;
            tm_Grass.SetTile(cellPos, null);

            if (elapsedTime > 20)
            {
                tm_Flower.SetTile(cellPos, lstTb_Pumpkin[4]);
            }
            else if (elapsedTime > 15)
            {
                tm_Flower.SetTile(cellPos, lstTb_Pumpkin[3]);
                playerFarmController.StartCoroutine(playerFarmController.GrowPlant(cellPos, tm_Flower, lstTb_Pumpkin.GetRange(3, 2)));
            }
            else if (elapsedTime > 10)
            {
                tm_Flower.SetTile(cellPos, lstTb_Pumpkin[2]);
                playerFarmController.StartCoroutine(playerFarmController.GrowPlant(cellPos, tm_Flower, lstTb_Pumpkin.GetRange(2, 3)));
            }
            else if (elapsedTime > 5)
            {
                tm_Flower.SetTile(cellPos, lstTb_Pumpkin[1]);
                playerFarmController.StartCoroutine(playerFarmController.GrowPlant(cellPos, tm_Flower, lstTb_Pumpkin.GetRange(1, 4)));
            }
            else
            {
                tm_Flower.SetTile(cellPos, lstTb_Pumpkin[0]);
                playerFarmController.StartCoroutine(playerFarmController.GrowPlant(cellPos, tm_Flower, lstTb_Pumpkin.GetRange(0, 5)));
            }
        }
    }

    public void MapToUI(Map map)
    {
        Debug.Log("Load map to UI");
        for(int i = 0; i < map.GetLength(); i++)
        {
            Debug.Log("I" +  i);
            TilemapDetailToTileBase(map.lstTilemapDetail[i]);
        }
    }

    public void  SetStateForTilemapDetail(int x, int y, TilemapState state)
    {
        for(int i = 0; i < LoadDataManager.userInGame.MapInGame.GetLength(); i++)
        {
            if (LoadDataManager.userInGame.MapInGame.lstTilemapDetail[i].x == x && LoadDataManager.userInGame.MapInGame.lstTilemapDetail[i].y == y)
            {
                LoadDataManager.userInGame.MapInGame.lstTilemapDetail[i].tilemapState = state;
                LoadDataManager.userInGame.MapInGame.lstTilemapDetail[i].growTime = DateTime.Now;
                DatabaseManager.WriteDatabase("Users/" + LoadDataManager.firebaseUser.UserId, LoadDataManager.userInGame.ToString());
                Debug.Log("Save to Firebase successful");
            }
        }
    }
}
