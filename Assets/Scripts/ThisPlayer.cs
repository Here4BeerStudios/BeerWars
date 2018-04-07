﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ThisPlayer : Player {
    // Access via ThisPlayer.self.XXX
    public static ThisPlayer self;
    
	void Awake () {
        // We check if we have a player.
        // If we already have a player - destroy the gameObject.
        // Just some insurance if we somehow mess up stuff ;D
        if (self == null) {
            DontDestroyOnLoad(gameObject);
            self = this;

            Load();
        }
        else if(self != this)
        {
            Destroy(gameObject);
        }
	}
	
	public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        // TODO Check if Sprite.name is correct variable to save.
        PlayerData data = new PlayerData { name = self.Name, sprite = self.Emblem.name };

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData pd = (PlayerData)bf.Deserialize(file);

            file.Close();

            self.Name = pd.name;
            self.Emblem = Resources.Load(pd.sprite, typeof(Sprite)) as Sprite;
        }
    }
}

[Serializable]
class PlayerData
{
    public string name;
    public string sprite;
}
