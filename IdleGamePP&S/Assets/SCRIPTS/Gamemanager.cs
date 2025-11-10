using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public Text NumberText, NumberAs;
    public float Number = 10;
    public GameObject[] Upgrades = new GameObject[8];
    public GameObject[] Generators = new GameObject[8];
    public float CPS, generation;
    public GameObject[] menus = new GameObject[8];
    private bool isGenerating = false; // Track if generation is active
    public float growthrate = 1.15f;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < Generators.Length; i++)
        {
            Generators[i].GetComponent<GUmanager>().cost = Mathf.Pow(8, (i * ((i!= 0 && i!= 1) ? 1.3f : 1f )) + 1) * Mathf.Pow(growthrate, Generators[i].GetComponent<GUmanager>().amountowned) ; // Cost increases exponentially
            Generators[i].GetComponent<GUmanager>().purchasemultiplier = 1.6f + (i * 0.35f); // Incremental multiplier
            Generators[i].GetComponent<GUmanager>().change = 0.02f * Mathf.Pow(8, (i * ((i != 0 && i != 1) ? 1.3f : 1f)) + 1);//this is kinda messed up   // Change increases with index
            Generators[i].GetComponent<GUmanager>().multiplier = 1f;
            Generators[i].GetComponent<GUmanager>().speed = 1f;
            Generators[i].GetComponent<GUmanager>().amountowned = 0;
            Generators[i].GetComponent<GUmanager>().description = Generators[i].GetComponent<GUmanager>().change.ToString("F1") + "/s"; // Description matches change
        }
    }

    // Update is called once per frame
    void Update()
    {
        NumberText.text = Number.ToString("F1");
        
        float totalGeneration = 0;
        for (int i = 0; i < Generators.Length; i++)
        {
            totalGeneration += (Generators[i].GetComponent<GUmanager>().change * Generators[i].GetComponent<GUmanager>().amountowned);
            Generators[i].GetComponent<GUmanager>().description = Generators[i].GetComponent<GUmanager>().change.ToString("F1") + "/s";
        }
        generation = totalGeneration;
        NumberAs.text = (totalGeneration*10).ToString("F1") + "/s";
    }

    public void AddNumber(float add)
    {
        NumberText.text = Number.ToString();
    }

    public void SubNumber(float sub)
    {
        Number -= sub;
        NumberText.text = Number.ToString();
    }

    public void BuyUpgrade(int index)
    {
        if (index >= 0 && index < Generators.Length)
        {
            if (Number >= Generators[index].GetComponent<GUmanager>().cost)
            {
                Number -= Generators[index].GetComponent<GUmanager>().cost;
                Generators[index].GetComponent<GUmanager>().cost *= Generators[index].GetComponent<GUmanager>().purchasemultiplier;
                Generators[index].GetComponent<GUmanager>().amountowned++;
                StartGenerating(); // Start generating when upgrade is bought
            }
        }
    }

    private void StartGenerating()
    {
        isGenerating = true; // Set generating to true
        StartCoroutine(ConstantTime());
    }

    IEnumerator ConstantTime()
    {
        while (isGenerating)
        {
            Number += generation; // Add generation to Number
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OpenGen()
    {
        menus[0].SetActive(true);
        menus[1].SetActive(false);
    }

    public void OpenUpg()
    {
        menus[1].SetActive(true);
        menus[0].SetActive(false);
    }
}

