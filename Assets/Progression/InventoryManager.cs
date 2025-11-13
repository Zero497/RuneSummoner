using UnityEngine;

public static class InventoryManager 
{
   private static ActionPriorityWrapper<string> saveCallback;
    
   private static ActionPriorityWrapper<string> loadCallback;
    
   private static ActionPriorityWrapper<string> NGCallback;
    
   private static ActionPriorityWrapper<string, string> VCCallback;
    
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
   }
    
   private static void Save(string path)
   {
      //TODO
   }

   private static void Load(string path)
   {
      //TODO
   }

   private static void VersionConversion(string fileVersion, string path)
   {
      //TODO
   }

   private static void NewGame(string path)
   {
      //TODO
   }
}
