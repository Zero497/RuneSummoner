using System.IO;
using UnityEngine;

public static class SaveHandler
{
    public static string currentVersion;

    public static string lastPathSaveLoc;

    public static string settingsSavePath;

    private static string savePath;

    private static bool loadImmediately;

    private static Coroutine timeTracker;

    private static void Awake()
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+lastPathSaveLoc);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+settingsSavePath);
        try
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath+"/"+lastPathSaveLoc + "/lastPath");
            string line = streamReader.ReadLine();
            line = line.Trim();
            streamReader.Close();
            setSavePath(line);
        }
        catch (FileNotFoundException)
        { }
    }

    public static bool checkPath()
    {
        return checkPath(savePath);
    }
    
    public static bool checkPath(string path)
    {
        if (path == null || path.Equals("")) return false;
        string meta = readMetaFile(path);
        return !meta.Equals("err");
    }

    public static void setSavePath(string path = "")
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
                Debug.Log("Last used path not found");
                return;
            }
        }
        savePath = path;
        string tup = readMetaFile(path);
        if (tup.Equals("err"))
        {
            Directory.CreateDirectory(Application.persistentDataPath+"/"+path);
            writeMetaFile(path, currentVersion);
        }
        else
        {
            if(!tup.Equals(currentVersion))
                VersionConversion(tup);
        }
        File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc+"/lastPath", path);
    }

    private static void writeMetaFile(string path, string version)
    {
        File.WriteAllText(Application.persistentDataPath+"/"+path+"/meta", version+"\n");
    }

    private static string readMetaFile(string path)
    {
        string ret = "";
        try
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/" + path + "/meta");
            string line = streamReader.ReadLine();
            ret = line.Trim();
            streamReader.Close();
            File.WriteAllText(Application.persistentDataPath + "/" + lastPathSaveLoc + "/lastPath", path);
            return ret;
        }
        catch (FileNotFoundException)
        {
            ret = "err";
            return ret;
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
        setSavePath(path);
    }

    public static void VersionConversion(string fileVersion)
    {
        Debug.Log("Version mismatch");
        //TODO
    }

    public static void Load()
    {
        //TODO
    }

    public static void Save()
    {
        if (savePath != null && !savePath.Equals(""))
        {
            writeMetaFile(savePath, currentVersion);
            File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc + "/lastPath", savePath);
            //TODO
        }
    }
}
