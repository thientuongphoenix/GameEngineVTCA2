using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenItems
{
    public string Name { get; set; }
    public string description { get; set; }

    public InvenItems()
    {

    }

    public InvenItems(string name, string description)
    {
        this.Name = name;
        this.description = description;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
