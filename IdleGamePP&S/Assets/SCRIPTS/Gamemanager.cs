using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;  
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public List<int> value = new List<int>();
    public Text NumberText, NumberAs;
    public float Number = 10;
    public GameObject[] Upgrades = new GameObject[8];
    public GameObject[] Generators = new GameObject[8];
    public GameObject[] Achievements = new GameObject[16];
    public float CPS, generation;
    public GameObject[] menus = new GameObject[8];
    private bool isGenerating = false; // Track if generation is active
    public float growthrate = 1.15f;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < Generators.Length; i++)
        {
            Generators[i].GetComponent<GUmanager>().cost = Mathf.Pow(8, (i * ((i != 0 && i != 1) ? 1.3f : 1f)) + 1); // Cost increases exponentially
            Generators[i].GetComponent<GUmanager>().purchasemultiplier = 1.6f + ((i+1) * (growthrate)); // Incremental multiplier
            Generators[i].GetComponent<GUmanager>().change = 0.02f * Mathf.Pow(6, (i * 1.3f) + 1);//this is kinda messed up   // Change increases with index
            Generators[i].GetComponent<GUmanager>().multiplier = 1f;
            Generators[i].GetComponent<GUmanager>().speed = 1f;
            Generators[i].GetComponent<GUmanager>().amountowned = 0;
        }
        for (int i = 0; i < Upgrades.Length; i++)
        {
            Upgrades[i].GetComponent<GUmanager>().cost = Mathf.Pow(12, (i * 1.1f) + 1); // Cost increases exponentially
            Upgrades[i].GetComponent<GUmanager>().purchasemultiplier = 6f + ((i+1) * (growthrate)); // Incremental multiplier
            Upgrades[i].GetComponent<GUmanager>().change = 0;
            Upgrades[i].GetComponent<GUmanager>().multiplier = 1.4f;
            Upgrades[i].GetComponent<GUmanager>().speed = 1f;
            Upgrades[i].GetComponent<GUmanager>().amountowned = 0;
            Upgrades[i].GetComponent<GUmanager>().description = "2x production";
        }        
        //for (int i = 0; i < Achievements.Length; i++)
        //    {
        //     Achievements[i] = GameObject.Find("AchievementHolder").transform.GetChild(i).gameObject;
        //    }
        //achivements will provide flat multipliers 
    }

    // Update is called once per frame
    void Update()
    {
        if (Number < Mathf.Pow(10,4))
        NumberText.text = Number.ToString("F1");
        else
        NumberText.text = Number.ToString("E1");
        float totalGeneration = 0;
        for (int i = 0; i < Generators.Length; i++)
        {
            totalGeneration += (Generators[i].GetComponent<GUmanager>().change * Generators[i].GetComponent<GUmanager>().amountowned);
            Generators[i].GetComponent<GUmanager>().description = Generators[i].GetComponent<GUmanager>().change.ToString("F1") + "/s";
        }
        generation = totalGeneration;
        
        NumberAs.text = (totalGeneration*10).ToString("F1") + "/s";
    }

    public void SubNumber(float sub)
    {
        Number -= sub;
        NumberText.text = Number.ToString();
    }

    public void BuyGenerator(int index)
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
    public void BuyUpgrade(int index)
    {
        if (index >= 0 && index < Upgrades.Length)
        {
            if (Number >= Upgrades[index].GetComponent<GUmanager>().cost)
            {
                Number -= Upgrades[index].GetComponent<GUmanager>().cost;
                Upgrades[index].GetComponent<GUmanager>().cost *= Upgrades[index].GetComponent<GUmanager>().purchasemultiplier;
                Upgrades[index].GetComponent<GUmanager>().amountowned++;
                Generators[index].GetComponent<GUmanager>().change *= Upgrades[index].GetComponent<GUmanager>().multiplier;
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
        menus[2].SetActive(false);
    }

    public void OpenUpg()
    {
        menus[1].SetActive(true);
        menus[0].SetActive(false);
        menus[2].SetActive(false);
    }
    public void OpenAch()
    {
        menus[2].SetActive(true);
        menus[0].SetActive(false);
        menus[1].SetActive(false);
    }
}

