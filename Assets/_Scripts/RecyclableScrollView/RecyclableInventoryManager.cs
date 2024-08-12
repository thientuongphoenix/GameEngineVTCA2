using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclableInventoryManager : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;
    [SerializeField]
    private int _dataLength;

    public GameObject inventoryGameObject;

    //Dummy data List
    private List<InvenItems> _invenItems = new List<InvenItems>();
    //Recyclable scroll rect's data source must be assigned in Awake.
    private void Awake()
    {
        //InitData();
        _recyclableScrollRect.DataSource = this;
    }
    #region DATA-SOURCE
    /// <summary>
    /// Data source method. return the list length.
    /// </summary>
    public int GetItemCount()
    {
        return _invenItems.Count;
    }
    /// <summary>
    /// Called for a cell every time it is recycled
    /// Implement this method to do the necessary cell configuration.
    /// </summary>
    public void SetCell(ICell cell, int index)
    {
        //Casting to the implemented Cell
        var item = cell as CellItemData;
        item.ConfigureCell(_invenItems[index], index);
    }
    #endregion

    private void Start()
    {
        List<InvenItems> lstItem = new List<InvenItems>();
        for(int i = 0; i < 50; i++)
        {
            InvenItems invenItem = new InvenItems();
            invenItem.Name = "Name_" + i.ToString();
            invenItem.description = "Des_" + i.ToString();

            lstItem.Add(invenItem);
        }
        SetLstItem(lstItem);
        _recyclableScrollRect.ReloadData();
    }

    public void SetLstItem(List<InvenItems> lst)
    {
        _invenItems = lst;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            InvenItems invenItemDemo = new InvenItems("Ca","Ca");
            _invenItems.Add(invenItemDemo);
            _recyclableScrollRect.ReloadData();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //inventoryGameObject.SetActive(!inventoryGameObject.activeSelf);
            Vector3 currPosInven = inventoryGameObject.GetComponent<RectTransform>().anchoredPosition;
            inventoryGameObject.GetComponent<RectTransform>().anchoredPosition = currPosInven.y == 1000 ? Vector3.zero : new Vector3(0, 1000, 0);
        }
    }

    public void AddInventoryItem(InvenItems item)
    {
        _invenItems.Add(item);
        _recyclableScrollRect.ReloadData();
    }
}