using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FungusPackedConfig
{
    public string name;
    public FungusConfig config;
    public FungusStats stats;
}
[CreateAssetMenu(fileName = "New Available Fungi Config", menuName = "Config/Available Fungi Config")]
public class AvailableFungiConfig : ScriptableObject
{
    public List<FungusPackedConfig> fungusPackedConfigList;

    private Dictionary<string, FungusPackedConfig> fungusConfigDictionary;

    public FungusPackedConfig GetFungusPackedConfigByName(string name)
    {
        if (fungusConfigDictionary == null)
        {
            fungusConfigDictionary = new Dictionary<string, FungusPackedConfig>();
            foreach (FungusPackedConfig config in fungusPackedConfigList)
            {
                fungusConfigDictionary.Add(config.name, config);
            }
        }

        if (fungusConfigDictionary.ContainsKey(name))
            return fungusConfigDictionary[name];
        return null;
    }
}
