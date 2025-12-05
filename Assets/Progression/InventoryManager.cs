using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class InventoryManager 
{
   private static ActionPriorityWrapper<string> saveCallback;
    
   private static ActionPriorityWrapper<string> loadCallback;
    
   private static ActionPriorityWrapper<string> NGCallback;
    
   private static ActionPriorityWrapper<string, string> VCCallback;

   private static Dictionary<string, int> summoningShards;
    
   public static void Init()
   {
      saveCallback = new ActionPriorityWrapper<string>();
      saveCallback.action = Save;
      SaveHandler.save.Subscribe(saveCallback);
      loadCallback = new ActionPriorityWrapper<string>();
      loadCallback.action = Load;
      SaveHandler.load.Subscribe(loadCallback);
      NGCallback = new ActionPriorityWrapper<string>();
      NGCallback.action = NewGame;
      SaveHandler.newGame.Subscribe(NGCallback);
      VCCallback = new ActionPriorityWrapper<string, string>();
      VCCallback.action = VersionConversion;
      SaveHandler.versionConversion.Subscribe(VCCallback);
      summoningShards = new Dictionary<string, int>();
   }
    
   private static void Save(string path)
   {
      Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/inventory");
      Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/inventory/shards");
      XmlSerializer ser = new XmlSerializer(typeof((string, int)));
      foreach (string key in summoningShards.Keys)
      {
         TextWriter writer = new StreamWriter(Application.persistentDataPath+"/"+path+"/inventory/shards/"+key);
         ser.Serialize(writer, (key, summoningShards[key]));
         writer.Close();
      }
   }

   private static void Load(string path)
   {
      Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/inventory");
      Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/inventory/shards");
      string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + path + "/inventory/shards");
      foreach (string file in files)
      {
         XmlSerializer mySerializer = new XmlSerializer(typeof((string,int)));
         using FileStream myFileStream = new FileStream(file, FileMode.Open);
         (string, int) shard = ((string,int))mySerializer.Deserialize(myFileStream);
         summoningShards[shard.Item1] = shard.Item2;
      }
      
   }

   private static void VersionConversion(string fileVersion, string path)
   {
      //TODO
   }

   private static void NewGame(string path)
   {
   }

   public static bool ChangeSummonShards(string unitName, int amt)
   {
      if (summoningShards.ContainsKey(unitName))
      {
         if (summoningShards[unitName] + amt < 0) return false;
         summoningShards[unitName] += amt;
      }
      else
      {
         if (amt < 0) return false;
         summoningShards.Add(unitName, amt);
      }
      return true;
   }

   public static int GetSummonShards(string unitName)
   {
      if (summoningShards.ContainsKey(unitName)) return summoningShards[unitName];
      return 0;
   }

   public static List<(string,int)> GetAllShards()
   {
      List<(string,int)> ret = new List<(string,int)>();
      foreach (string key in summoningShards.Keys)
      {
         ret.Add((key, summoningShards[key]));
      }
      return ret;
   }
}
