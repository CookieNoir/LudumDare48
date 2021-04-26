using UnityEngine;

public class IslandFactory : MonoBehaviour
{
    public string[] possibleNames;
    public GameObject[] tradingIslandMapPrefabs;
    public GameObject[] resourceIslandMapPrefabs;
    public SurfaceIslandPattern[] tradingIslandPatterns;
    public SurfaceIslandPattern[] resourceIslandPatterns;

    public OceanIsland CreateIsland(int day, int gearScore, bool isTrading, Vector3 position)
    {
        GameObject islandObject;
        if (isTrading)
        {
            islandObject = Instantiate(tradingIslandMapPrefabs[Random.Range(0, tradingIslandMapPrefabs.Length)], position, Quaternion.identity);
        }
        else
        {
            islandObject = Instantiate(resourceIslandMapPrefabs[Random.Range(0, resourceIslandMapPrefabs.Length)], position, Quaternion.identity);
        }
        islandObject.name = possibleNames[Random.Range(0, possibleNames.Length)];
        OceanIsland island = islandObject.AddComponent<OceanIsland>();
        island.islandType = isTrading ? IslandTypes.Trading : IslandTypes.Resource;
        int value = day * gearScore;
        if (value > 100)
        {
            island.value = 2;
        }
        else if (value > 20)
        {
            island.value = 1;
        }
        else
        {
            island.value = 0;
        }
        if (isTrading)
        {
            island.surfaceIsland = tradingIslandPatterns[Random.Range(0, tradingIslandPatterns.Length)].CreateIsland();
        }
        else
        {
            island.surfaceIsland = resourceIslandPatterns[Random.Range(0, resourceIslandPatterns.Length)].CreateIsland();
        }
        return island;
    }
}