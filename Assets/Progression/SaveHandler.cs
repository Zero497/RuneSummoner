using System.IO;
using UnityEngine;

public static class SaveHandler
{
    public static EventPriorityWrapper<string> save;

    public static EventPriorityWrapper<string> load;

    public static EventPriorityWrapper<string> newGame;

    //version, path
    public static EventPriorityWrapper<string, string> versionConversion;
    
    private static string currentVersion = "0.1.0";
    
    private static string lastPathSaveLoc = "lastPath";

    private static string settingsSavePath = "settings";

    private static string savePath;

    private static bool loadImmediately;

    private static Coroutine timeTracker;

    private static string saveSlotName = "slot";

    public static void Init()
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+lastPathSaveLoc);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+settingsSavePath);
        save = new EventPriorityWrapper<string>();
        load = new EventPriorityWrapper<string>();
        newGame = new EventPriorityWrapper<string>();
        versionConversion = new EventPriorityWrapper<string, string>();
    }

    public static bool checkPath()
    {
        return checkPath(savePath);
    }
    
    public static bool checkPath(string path)
    {
        if (path == null || path.Equals("")) return false;
        (string,string) meta = readMetaFile(path);
        return !meta.Item1.Equals("err");
    }

    public static bool NewGameNewSlot()
    {
        string path = saveSlotName + "1";
        int slotNum = 1;
        while (slotNum < 10000)
        {
            if (!checkPath(path))
            {
                NewGame(path);
                return true;
            }
            slotNum++;
            path = saveSlotName + slotNum;
        }
        Debug.Log("Max saves exceed!");
        return false;
 
    }

    public static bool setSavePath(string path = "")
    {
        if (checkPath())
        {
            Save();
        }
        if (path == "")
        {
            try
            {
                StreamReader streamReader =
                    new StreamReader(Application.persistentDataPath + "/" + lastPathSaveLoc + "/lastPath");
                string line = streamReader.ReadLine();
                line = line.Trim();
                streamReader.Close();
                path = line;
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Last used path not found. Creating new game in next unused slot");
                return NewGameNewSlot();
            }
        }
        savePath = path;
        (string,string) tup = readMetaFile(path);
        if (tup.Item1.Equals("err"))
        {
            Debug.Log("ERR save meta file not found! Creating new game in next unused slot");
            NewGameNewSlot();
            return false;
        }
        if(!tup.Item1.Equals(currentVersion))
            VersionConversion(tup.Item1, path);
        
        File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc+"/lastPath", path);
        return true;
    }

    private static void writeMetaFile(string path, string version)
    {
        File.WriteAllText(Application.persistentDataPath+"/"+path+"/meta", version+"\n"+PlayTimer.timer.time+"\n");
    }

    public static (string, string) readMetaFile()
    {
        return readMetaFile(savePath);
    }

    public static (string,string) readMetaFile(string path)
    {
        string ret = "";
        string ret2 = "";
        try
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/" + path + "/meta");
            string line = streamReader.ReadLine();
            if (line != null)
            {
                ret = line.Trim();
            }
            else
            {
                ret = "";
            }

            line = streamReader.ReadLine();
            if (line != null)
            {
                ret2 = line.Trim();
            }
            else
            {
                ret2 = "0";
            }

            streamReader.Close();
            File.WriteAllText(Application.persistentDataPath + "/" + lastPathSaveLoc + "/lastPath", path);
            return (ret, ret2);
        }
        catch (FileNotFoundException)
        {
            ret = "err";
            return (ret, "0");
        }
        catch (DirectoryNotFoundException)
        {
            ret = "err";
            return (ret, "0");
        }
    }
    
    public static string getSavePath()
    {
        return Application.persistentDataPath+"/"+savePath;
    }

    public static void NewGame(string path)
    {
        try
        {
            Directory.Delete(Application.persistentDataPath+"/"+path, true);
        }
        catch(DirectoryNotFoundException) {}
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path);
        writeMetaFile(path, currentVersion);
        setSavePath(path);
        newGame.Invoke(path);
    }

    public static void VersionConversion(string fileVersion, string path)
    {
        Debug.Log("Version mismatch");
        versionConversion.Invoke(fileVersion, path);
        writeMetaFile(path, currentVersion);
    }

    public static void Load()
    {
        load.Invoke(savePath);
    }

    public static void Save()
    {
        if (savePath != null && !savePath.Equals(""))
        {
            writeMetaFile(savePath, currentVersion);
            File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc + "/lastPath", savePath);
            save.Invoke(savePath);
        }
    }
}
