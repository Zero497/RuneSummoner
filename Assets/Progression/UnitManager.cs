using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public static class UnitManager
{
    private static ActionPriorityWrapper<string> saveCallback;
    
    private static ActionPriorityWrapper<string> loadCallback;
    
    private static ActionPriorityWrapper<string> NGCallback;
    
    private static ActionPriorityWrapper<string, string> VCCallback;

    public static List<UnitSimple> playerUnits;
    
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
        playerUnits = new List<UnitSimple>();
    }
    
    private static void Save(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/units");
        XmlSerializer ser = new XmlSerializer(typeof(UnitSimple));
        foreach (UnitSimple unit in playerUnits)
        {
            TextWriter writer = new StreamWriter(Application.persistentDataPath+"/"+path+"/units/"+unit.id);
            ser.Serialize(writer, unit);
            writer.Close();
        }
    }

    private static void Load(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/units");
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + path + "/units");
        foreach (string file in files)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(UnitSimple));
            using FileStream myFileStream = new FileStream(file, FileMode.Open);
            UnitSimple unit = (UnitSimple)mySerializer.Deserialize(myFileStream);
            playerUnits.Add(unit);
        }
        playerUnits.Sort();
    }

    private static void ModUnit(UnitSimple mod)
    {
        if (!IDExists(mod.id))
        {
            AddUnit(mod);
            return;
        }
        XmlSerializer ser = new XmlSerializer(typeof(UnitSimple));
        TextWriter writer = new StreamWriter(SaveHandler.getSavePath()+"/units/"+mod.id);
        ser.Serialize(writer, mod);
        writer.Close();
    }

    public static string GetValidUnitID(string name)
    {
        int idNum = 1;
        foreach (UnitSimple unit in playerUnits)
        {
            if (unit.name.Equals(name))
            {
                idNum++;
            }
        }
        return name + idNum;
    }

    public static bool IDExists(string id)
    {
        foreach (UnitSimple unit in playerUnits)
        {
            if (unit.id.Equals(id))
                return true;
        }
        return false;
    }

    private static void AddUnit(UnitSimple add)
    {
        if (IDExists(add.id)) return;
        playerUnits.Add(add);
        XmlSerializer ser = new XmlSerializer(typeof(UnitSimple));
        TextWriter writer = new StreamWriter(SaveHandler.getSavePath()+"/units/"+add.id);
        ser.Serialize(writer, add);
        writer.Close();
    }

    private static void VersionConversion(string fileVersion, string path)
    {
        //TODO
    }

    private static void NewGame(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/units");
        UnitSimple player = new UnitSimple("Player", "player", 1, new StatGrades());
        AddUnit(player);
        UnitSimple golem = new UnitSimple("Golem", "golem1", 1, new StatGrades());
        golem.availableUpgradePoints = 1;
        AddUnit(golem);
        UnitSimple ferret = new UnitSimple("DireFerret", "direferret1", 1, new StatGrades());
        ferret.availableUpgradePoints = 1;
        AddUnit(ferret);
    }
}
