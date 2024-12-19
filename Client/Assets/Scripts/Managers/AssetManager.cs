using System;
using System.Collections.Generic;
using Best.HTTP.JSON;
using UnityEngine;

public class AssetManager
{
    [Serializable]
    public class Assets
    {
        public Stages stages;
        public Items items;
        public ItemUnlocks itemUnlocks;
    }

    [Serializable]
    public class Stages
    {
        public string name;
        public string version;
        public StageData[] data;
    }

    [Serializable]
    public class StageData
    {
        public int id;
        public int score;
    }

    [Serializable]
    public class Items
    {
        public string name;
        public string version;
        public ItemData[] data;
    }

    [Serializable]
    public class ItemData
    {
        public int id;
        public int score;
    }

    [Serializable]
    public class ItemUnlocks
    {
        public string name;
        public string version;
        public ItemUnlockData[] data;
    }

    [Serializable]
    public class ItemUnlockData
    {
        public int id;
        public int stage_id;
        public int item_id;
    }

    public Assets assets;
    public StageData[] stages
    {
        get { return assets.stages.data; }
    }
    public ItemData[] items
    {
        get { return assets.items.data; }
    }
    public ItemUnlockData[] itemUnlocks
    {
        get { return assets.itemUnlocks.data; }
    }

    public int CurrentItemScore
    {
        get
        {
            try
            {
                int currentStageId = GameManager.Instance.CurrentStageId;
                var unlock = Array.Find(itemUnlocks, e => e.stage_id == currentStageId);
                var item = Array.Find(items, e => e.id == unlock.item_id);
                return item.score;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                return 0;
            }
        }
    }

    public int FirstStageId
    {
        get
        {
            return stages[0].id;
        }
    }

    public int NextStageId
    {
        get
        {
            return stages[GameManager.Instance.CurrentStage + 1].id;
        }
    }

    public void LoadAsset(string payload)
    {
        try
        {
            assets = JsonUtility.FromJson<Assets>(payload);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


}
