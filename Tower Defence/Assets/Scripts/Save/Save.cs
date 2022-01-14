using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{

    public Dictionary<KeyValuePair<float, float>, string> towerPositions = new Dictionary<KeyValuePair<float,float>, string>();
    public Dictionary<KeyValuePair<float, float>, string> spawnerPositions = new Dictionary<KeyValuePair<float, float>, string>();

    public List<string> monsterNames = new List<string>();
    public List<float> monsterPositionX = new List<float>();
    public List<float> monsterPositionY = new List<float>();
    public List<float> monsterPositionZ = new List<float>();

    public List<int> numTowers = new List<int>();
    public List<int> numSpawners = new List<int>();

    public Dictionary<KeyValuePair<float, float>, bool> riverTiles = new Dictionary<KeyValuePair<float, float>, bool>();
    public List<KeyValuePair<float, float>> obstacles = new List<KeyValuePair<float, float>>();
    public List<KeyValuePair<float, float>> notSelectable = new List<KeyValuePair<float, float>>();

    public List<bool> upgradesAvailable = new List<bool>();
    public List<bool> upgradesPressed = new List<bool>();

    public List<int> upgradeValues;

    public double score;
    public int gems;
}
