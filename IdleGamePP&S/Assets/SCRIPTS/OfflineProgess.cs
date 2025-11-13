using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OfflineProgess : MonoBehaviour {
	PlayerPrefs playerPrefs;
	private const string offlineProgressKey = "offlineProgress";
    // Use this for initialization
    public void Start()
    {
        {
            StartCoroutine(DelayStart());
        }
    }   
            IEnumerator DelayStart () {
        yield return new WaitForSeconds(1f); // Wait for 1 second to ensure Gamemanager is initialized
		if (PlayerPrefs.HasKey(offlineProgressKey))
		{
			long storedTime = Convert.ToInt64(PlayerPrefs.GetString(offlineProgressKey));
			DateTime lastTime = DateTime.FromBinary(storedTime);
			TimeSpan offlineDuration = DateTime.UtcNow - lastTime;
			Debug.Log("Offline duration: " + offlineDuration.TotalSeconds + " seconds");
			if (offlineDuration.TotalSeconds > 0)
            {
                float offlineEarnings = GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().generation * (float)offlineDuration.TotalSeconds;
                GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number += offlineEarnings;
                Debug.Log("Added offline earnings: " + offlineEarnings);
            }
		}
		if (PlayerPrefs.HasKey("value"))
        {
            float storedValue = PlayerPrefs.GetFloat("value");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number = storedValue;
            Debug.Log("Restored value: " + storedValue);
        }
		else Debug.Log("no offline progess detected");
		
		if (PlayerPrefs.HasKey("generators"))
        {
            string generators = PlayerPrefs.GetString("generators");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGenUmanager();
            Debug.Log("Restored generators: " + generators);
        }
		if (PlayerPrefs.HasKey("upgrades"))
        {
            string serializedUpgrades = PlayerPrefs.GetString("upgrades");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().LoadGUPGmanager();
            Debug.Log("Restored upgrades: " + serializedUpgrades);
        }
        GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().StartGenerating();
	}
	
	void OnApplicationQuit()
    {
        long currentTime = DateTime.UtcNow.ToBinary();
        PlayerPrefs.SetString(offlineProgressKey, currentTime.ToString());
		PlayerPrefs.SetFloat("value", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number);
		PlayerPrefs.SetString("generators", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGenUmanager());
        PlayerPrefs.SetString("upgrades", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().SaveGUPGmanager());
		PlayerPrefs.SetFloat("generation", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().generation);
        PlayerPrefs.Save();
    }
}
