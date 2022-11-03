using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Json;

public class SavesManager
{
    public static Dictionary<string, float> configdic;

    public static void GetGameConfig()
    {
        string configpath = @"D:/Config.txt";
        configdic = new Dictionary<string, float>();
        if (!File.Exists(configpath))
        {
            CreateDefaultConfig(configpath);
        }
        Stream reader = File.OpenRead(configpath);
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<string, float>));
        configdic = (Dictionary<string, float>)ser.ReadObject(reader);
    }

    private static void CreateDefaultConfig(string _configpath)
    {
        configdic = new Dictionary<string, float> {
            { "UGUI", 1},
        };

        File.Create(_configpath).Dispose();
        Stream writer = File.OpenWrite(_configpath);
        var json = JsonUtility.ToJson(configdic);
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<string, float>));
        ser.WriteObject(writer,configdic);
        writer.Close();
    }

}
