using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OfflineProgess : MonoBehaviour {
	PlayerPrefs playerPrefs;
	private const string offlineProgressKey = "offlineProgress";
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey(offlineProgressKey))
		{
			long storedTime = Convert.ToInt64(PlayerPrefs.GetString(offlineProgressKey));
			DateTime lastTime = DateTime.FromBinary(storedTime);
			TimeSpan offlineDuration = DateTime.UtcNow - lastTime;
			Debug.Log("Offline duration: " + offlineDuration.TotalSeconds + " seconds");
		}
		if (PlayerPrefs.HasKey("value"))
        {
            float storedValue = PlayerPrefs.GetFloat("value");
            GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number = storedValue;
            Debug.Log("Restored value: " + storedValue);
        }
		else Debug.Log("no offline progess detected");
	}
	
	void OnApplicationQuit()
    {
        long currentTime = DateTime.UtcNow.ToBinary();
        PlayerPrefs.SetString(offlineProgressKey, currentTime.ToString());
		PlayerPrefs.SetFloat("value", GameObject.Find("_0Gamemanager").GetComponent<Gamemanager>().Number);
		
        PlayerPrefs.Save();
    }
}
