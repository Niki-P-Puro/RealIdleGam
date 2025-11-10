using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUmanager : MonoBehaviour
{


    
    public new string name { get; set; }
    public string description { get; set; }
    public float change { get; set;}
    public float cost { get; set; }
    public float multiplier { get; set; }
    public float speed { get; set; }
    public float purchasemultiplier { get; set; } 
    public float amountowned { get; set; }
    public GUmanager(string name, string description, float change, float cost, float multiplier, float speed, float purchasemultiplier, float amountowned)
    {
        this.name = name;
        this.description = description;
        this.change = change;
        this.cost = cost;
        this.multiplier = multiplier;
        this.speed = speed;
        this.purchasemultiplier = purchasemultiplier;
        this.amountowned = amountowned;
    }
    public void Update()
    {
        Transform child1 = transform.Find("Cost");
        child1.GetComponent<Text>().text = "Cost: " + cost.ToString("F1");
        Transform child2 = transform.Find("Upgrade");
        child2.GetComponent<Text>().text = description;
    }

}    