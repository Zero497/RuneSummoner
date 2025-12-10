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

    public static UnitSimple player;

    public static List<UnitSimple> playerParty;

    private static float baseSummonCap = 300;

    public static float partyCost => _partyCost;

    private static float _partyCost;
    
    public static float summonCap => GetSummonCap();
    
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

    private static float GetSummonCap()
    {
        if (player == null)
            return 0;
        return baseSummonCap * (0.9f + 0.1f * player.level);
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

    private static void GetPlayer(string path)
    {
        XmlSerializer mySerializer = new XmlSerializer(typeof(UnitSimple));
        using FileStream myFileStream = new FileStream(Application.persistentDataPath + "/" + path + "/units/player", FileMode.Open);
        UnitSimple unit = (UnitSimple)mySerializer.Deserialize(myFileStream);
        player = unit;
        unit.InitAbilities();
        AddToParty(unit, false);
    }

    private static void Load(string path)
    {
        playerUnits = new List<UnitSimple>();
        playerParty = new List<UnitSimple>();
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/units");
        GetPlayer(path);
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + path + "/units");
        foreach (string file in files)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(UnitSimple));
            using FileStream myFileStream = new FileStream(file, FileMode.Open);
            UnitSimple unit = (UnitSimple)mySerializer.Deserialize(myFileStream);
            if (unit.id.Equals("player"))
            {
                continue;
            }
            playerUnits.Add(unit);
            if (unit.inParty)
            {
                AddToParty(unit, false);
            }
            unit.InitAbilities();
        }
        playerUnits.Sort();
    }

    public static void ModUnit(UnitSimple mod)
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
        if (id.Equals("player") && player != null) return true;
        foreach (UnitSimple unit in playerUnits)
        {
            if (unit.id.Equals(id))
                return true;
        }
        return false;
    }

    public static bool AddToParty(UnitSimple unit, bool save = true)
    {
        UnitData data = unit.GetMyUnitData();
        if (partyCost + data.summonCost <= summonCap && (!unit.inParty || !save))
        {
            unit.inParty = true;
            if(save) {ModUnit(unit);}
            _partyCost += data.summonCost;
            playerParty.Add(unit);
            playerUnits.Sort();
            return true;
        }
        return false;
    }

    public static bool RemoveFromParty(UnitSimple unit)
    {
        if (playerParty.Contains(unit))
        {
            playerParty.Remove(unit);
            unit.inParty = false;
            ModUnit(unit);
            _partyCost -= unit.GetMyUnitData().summonCost;
            playerUnits.Sort();
            return true;
        }
        return false;
    }

    public static void AddUnit(UnitSimple add)
    {
        if (IDExists(add.id)) return;
        playerUnits.Add(add);
        add.InitAbilities();
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
        player.inParty = true;
        AddUnit(player);
        UnitSimple golem = new UnitSimple("Golem", "golem1", 1, new StatGrades());
        golem.availableUpgradePoints = 1;
        golem.inParty = true;
        AddUnit(golem);
        UnitSimple ferret = new UnitSimple("Dire Ferret", "direferret1", 1, new StatGrades());
        ferret.availableUpgradePoints = 1;
        ferret.inParty = true;
        AddUnit(ferret);
    }
}
