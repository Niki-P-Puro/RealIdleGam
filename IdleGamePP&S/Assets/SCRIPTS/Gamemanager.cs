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

    // Use this for initialization
    void Start()
    {// we gotta find a better way to do this
        Generators[0].GetComponent<GUmanager>().cost = 10;
        Generators[0].GetComponent<GUmanager>().purchasemultiplier = 1.1f;
        Generators[0].GetComponent<GUmanager>().change = 0.1f;
        Generators[0].GetComponent<GUmanager>().multiplier = 1f;
        Generators[0].GetComponent<GUmanager>().speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        NumberText.text = Number.ToString("F1");
        generation = isGenerating ? Generators[0].GetComponent<GUmanager>().change : 0;
        NumberAs.text = (generation * 10).ToString("F1") + "/s";
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
        if (index == 0)
        {
            if (Number >= Generators[0].GetComponent<GUmanager>().cost)
            {
                Number -= Generators[0].GetComponent<GUmanager>().cost;
                Generators[0].GetComponent<GUmanager>().cost *= Generators[0].GetComponent<GUmanager>().purchasemultiplier;
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

