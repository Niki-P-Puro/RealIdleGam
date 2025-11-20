using UnityEngine;
using UnityEngine.UI;

public class GUmanager : MonoBehaviour
{


    // getter setter + properties
    public new string name { get; set; }
    public string description { get; set; }
    public float change { get; set;}
    public float cost { get; set; }
    public float multiplier { get; set; }
    public float speed { get; set; }
    public float purchasemultiplier { get; set; } 
    public float amountowned { get; set; }
    public GUmanager(string name,string description, float change, float cost, float multiplier, float speed, float purchasemultiplier, float amountowned)
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
    public void Update() // updates the text fields of the GUI element
    {
        
        Transform child1 = transform.Find("Cost");
        if (cost < Mathf.Pow(10, 6))
            child1.GetComponent<Text>().text = "Cost: " + cost.ToString("F2");
        else
            child1.GetComponent<Text>().text = "Cost: " + cost.ToString("E1");
                                                                                
        Transform child2 = transform.Find("Upgrade");
        if (this.description != null)
        {
            child2.GetComponent<Text>().text = this.description;
            return;
        }
        else
        {
            if (change < Mathf.Pow(10, 6))
                child2.GetComponent<Text>().text = (10 * change).ToString("F2") + "/s";
            else
                child2.GetComponent<Text>().text = (10 * change).ToString("E1") + "/s";

        }
    }

}    