using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleGenerator : MonoBehaviour
{
    public static Battle launchBattle;
    
    public string id;

    public List<Vector2Int> mapSizes;

    public List<int> encounterLevels;

    public List<EnemyGenData> validEnemies;

    private int curLevelSelection = 0;

    private Battle.Difficulty curDiffSelection = 0;
    
    public List<DataTile> tiles;

    public List<MapGenerator.GenerationType> genTypes;

    public List<float> spawnFrequency;

    public GameObject launchPanel;

    public Button rightButtonL;

    public Button leftButtonL;

    public Button rightButtonD;

    public Button leftButtonD;

    public TextMeshProUGUI levelText;

    public List<Sprite> difficultyIcons;

    public TextMeshProUGUI diffText;

    public Button launchButton;

    public Image launchButtonImage;

    public TextMeshProUGUI descText;

    public Button closeButton;

    private List<List<Battle>> encounters;

    private ActionPriorityWrapper<string> load;

    private ActionPriorityWrapper<string> save;

    public void OpenLaunchPanel()
    {
        if (launchPanel.activeSelf)
        {
            rightButtonL.onClick.RemoveAllListeners();
            leftButtonL.onClick.RemoveAllListeners();
            rightButtonD.onClick.RemoveAllListeners();
            leftButtonD.onClick.RemoveAllListeners();
            launchButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }
        else
        {
            rightButtonL.onClick.AddListener(ChangeLevelSelectionR);
            leftButtonL.onClick.AddListener(ChangeLevelSelectionL);
            rightButtonD.onClick.AddListener(ChangeDiffSelectionR);
            leftButtonD.onClick.AddListener(ChangeDiffSelectionL);
            launchButton.onClick.AddListener(Launch);
            closeButton.onClick.AddListener(OpenLaunchPanel);
        }
        launchPanel.SetActive(!launchPanel.activeSelf);
        UpdateText();
    }

    public void Launch()
    {
        launchBattle = GetSelectedBattle();
        encounters = new List<List<Battle>>();
        Fill();
        SceneManager.LoadScene("CombatScene");
    }
    
    public void ChangeDiffSelectionL()
    {
        curDiffSelection -= 1;
        if ((int)curDiffSelection < 0)
        {
            curDiffSelection += difficultyIcons.Count;
        }
        UpdateText();
    }

    public void ChangeDiffSelectionR()
    {
        curDiffSelection += 1;
        if ((int)curDiffSelection < difficultyIcons.Count)
        {
            curDiffSelection -= difficultyIcons.Count;
        }
        UpdateText();
    }

    public void ChangeLevelSelectionL()
    {
        curLevelSelection -= 1;
        if (curLevelSelection < 0)
        {
            curLevelSelection += encounterLevels.Count;
        }
        UpdateText();
    }

    public void ChangeLevelSelectionR()
    {
        curLevelSelection += 1;
        if (curLevelSelection < encounterLevels.Count)
        {
            curLevelSelection -= encounterLevels.Count;
        }
        UpdateText();
    }

    private Battle GetSelectedBattle()
    {
        return encounters[curLevelSelection][(int)curDiffSelection];
    }

    private void UpdateText()
    {
        levelText.text = "<style=\"C6\">Level " + encounterLevels[curLevelSelection] + "</style>";
        diffText.text = "<style=" + curDiffSelection + ">" + curDiffSelection.ToString().FirstCharacterToUpper() +
                        "</style>";
        launchButtonImage.sprite = difficultyIcons[(int)curDiffSelection];
        Battle cur = GetSelectedBattle();
        descText.text = "Expected Units:\n";
        int unitsPerCol = 1 + Mathf.FloorToInt(cur.enemies.Count/10.0f);
        int curUnitInd = 0;
        while (curUnitInd < cur.enemies.Count)
        {
            for (int i = 0; i < unitsPerCol; i++)
            {
                UnitSimple unit = cur.enemies[curUnitInd + i];
                int rand = Random.Range(0, 100);
                if(rand < 140-20*(int)curDiffSelection)
                    descText.text += UnitData.GradeToColor(unit.GetMyUnitData().myGrade) + unit.name;
                else
                {
                    descText.text += "?";
                }
                descText.text += " Level " + unit.level + " ";
                if (rand < 140 - 20 * (int)curDiffSelection)
                    descText.text += "</color>";
                curUnitInd++;
            }
            descText.text += "\n";
        }
    }

    private void Awake()
    {
        load = new ActionPriorityWrapper<string>();
        load.action = Load;
        SaveHandler.load.Subscribe(load);
        save = new ActionPriorityWrapper<string>();
        save.action = Save;
        SaveHandler.save.Subscribe(save);
        Load(SaveHandler.getPartialPath());
    }

    private void Fill()
    {
        for (int i = 0; i < encounterLevels.Count; i++)
        {
            if(i == encounterLevels.Count)
                encounters.Add(new List<Battle>());
            for (int j = 0; j < 4; j++)
            {
                if (j == encounters.Count)
                {
                    encounters[i].Add(GenerateBattle(encounterLevels[i], (Battle.Difficulty)j));
                }
            }
        }
        Save(SaveHandler.getPartialPath());
    }

    private Battle GenerateBattle(int level, Battle.Difficulty difficulty)
    {
        Battle ret = new Battle();
        ret.zoneID = id;
        ret.diff = difficulty;
        ret.level = level;
        ret.mapSize = mapSizes[Random.Range(0, mapSizes.Count)];
        ret.tiles = tiles;
        ret.genTypes = genTypes;
        ret.spawnFrequency = spawnFrequency;
        float points = GetPoints(ret);
        float min = MinPointCost(level - 3);
        List<(EnemyGenData, float)> randList = GetRandList(level - 3);
        float randMax = randList[^1].Item2;
        int eCount = 0;
        ret.enemies = new List<UnitSimple>();
        while (points > min)
        {
            float rand = Random.Range(0, randMax);
            int ind;
            for (ind = 0; ind < randList.Count; ind++)
            {
                if (rand <= randList[ind].Item2)
                    break;
            }

            EnemyGenData data = randList[ind].Item1;
            int unitLevel = Random.Range(Mathf.Max(level - 3, data.minLevel), level + 3);
            UnitData ud = Resources.Load<UnitData>("UnitData/" + data.eName);
            float cost = GetCost(ud.summonCost, unitLevel);
            if (cost > points)
                continue;
            points -= cost;
            UnitSimple unit = new UnitSimple(data.eName, "Enemy" + data.eName + eCount, unitLevel,
                StatGrades.RandomStatGrades());
            ret.enemies.Add(unit);
            for (int i = 0; i < unitLevel && i < data.upgradeStrings.Count; i++)
            {
                unit.acquiredUpgrades.Add(data.upgradeStrings[i]);
            }
            unit.InitAbilities();
            eCount++;
        }
        return ret;
    }

    private List<(EnemyGenData, float)> GetRandList(int levelMin)
    {
        List<(EnemyGenData, float)> ret = new List<(EnemyGenData, float)>();
        float val = 0;
        foreach (EnemyGenData data in validEnemies)
        {
            if (data.minLevel >= levelMin)
            {
                val += data.frequency;
                ret.Add((data, val));
            }
        }
        return ret;
    }

    private float GetCost(float baseCost, int level)
    {
        return baseCost * (1 + 0.1f * (level - 1));
    }

    private float MinPointCost(int levelMin)
    {
        float min = float.PositiveInfinity;
        foreach (EnemyGenData data in validEnemies)
        {
            if (data.minLevel >= levelMin)
            {
                UnitData ud = Resources.Load<UnitData>("UnitData/" + data.eName);
                float cost = GetCost(ud.summonCost, levelMin);
                min = Mathf.Min(min, cost);
            }
        }
        return min;
    }

    private float GetPoints(Battle battle)
    {
        float points = 0;
        points += battle.mapSize.x * battle.mapSize.y * 0.5f;
        points *= 1 + (0.1f * battle.level) + Mathf.Max(0.05f * (battle.level - 10), 0);
        float diffMult = 1;
        switch (battle.diff)
        {
            case Battle.Difficulty.standard:
                diffMult = 1.3f;
                break;
            case Battle.Difficulty.heroic:
                diffMult = 1.6f;
                break;
            case Battle.Difficulty.mythic:
                diffMult = 2.0f;
                break;
        }
        points *= diffMult;
        return points;
    }

    private void Load(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/encounters/"+id);
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + path + "/encounters/"+id);
        encounters = new List<List<Battle>>();
        for(int i = 0; i<encounterLevels.Count; i++)
            encounters.Add(new List<Battle>());
        foreach (string file in files)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Battle));
            using FileStream myFileStream = new FileStream(file, FileMode.Open);
            Battle battle = (Battle)mySerializer.Deserialize(myFileStream);
            encounters[encounterLevels.IndexOf(battle.level)].Add(battle);
        }
        for(int i = 0; i<encounterLevels.Count; i++)
            encounters[i].Sort();
        Fill();
    }

    private void Save(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath+"/"+path+"/encounters/"+id);
        XmlSerializer ser = new XmlSerializer(typeof(Battle));
        foreach (List<Battle> battles in encounters)
        {
            foreach (Battle battle in battles)
            {
                TextWriter writer = new StreamWriter(Application.persistentDataPath+"/"+path+"/encounters/"+id+"/"+battle.zoneID+battle.level.ToString()+battle.diff.ToString());
                ser.Serialize(writer, battle);
                writer.Close();
            }
        }
    }
}
