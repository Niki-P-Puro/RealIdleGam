using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUmanager : MonoBehaviour
{


    
    public new string name { get; set; }
    public string description { get; set; }
    public float change { get; set;}
    public float cost { get; set; }
    public float multiplier { get; set; }
    public float speed { get; set; }
    public float purchasemultiplier { get; set; } 
    public GUmanager(string name, string description, float change, float cost, float multiplier, float speed, float purchasemultiplier)
    {
        this.name = name;
        this.description = description;
        this.change = change;
        this.cost = cost;
        this.multiplier = multiplier;
        this.speed = speed;
        this.purchasemultiplier = purchasemultiplier;
    }

}    