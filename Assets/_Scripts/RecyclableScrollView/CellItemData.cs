using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class CellItemData : MonoBehaviour, ICell
{
    //UI
    public Text nameLabel;
    public Text desLabel;
    //Model
    private InvenItems _contactInfo;
    private int _cellIndex;
    //This is called from the SetCell method in DataSource
    public void ConfigureCell(InvenItems invenItems, int cellIndex)
    {
        _cellIndex = cellIndex;
        _contactInfo = invenItems;
        nameLabel.text = invenItems.Name;
        desLabel.text = invenItems.description;
    }

}
