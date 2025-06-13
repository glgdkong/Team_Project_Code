using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class MapData
    {
        [SerializeField] private string mapName;
        [SerializeField] private GameObject mapObject;
        public GameObject MapObject { get { if (mapObject != null) { return mapObject; } else { return null; } } }
    }

    [SerializeField] private MapData[] mapDatas;

    private void Start()
    {
        MapLoad();
    }
    private void MapLoad()
    {
        int index = 0;
        switch (CharacterLists.battleIndex)
        {
            case 0:
                index = 0;
                break;
            case 1:
                index = 1;
                break;
            case 2:
                index = 2;
                break;
            default:
                break;
        }
        if (mapDatas[index].MapObject != null)
        {
            foreach (MapData mapData in mapDatas)
            {
                mapData.MapObject.SetActive(false);
            }
            mapDatas[index].MapObject.SetActive(true);
        }
    }
}
