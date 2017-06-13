using UnityEngine;
using System.Collections;
using Explorers;
using System.Collections.Generic;

public class HexFog : MonoBehaviour {
  GameObject Fog = null;
  WorldHexGrid Map = null;
  Dictionary<Tile, GameObject> fogByTiles = new Dictionary<Tile, GameObject>();

  // Use this for initialization
  public void InitFog() {
    Fog = GameObject.Find("Engine").GetComponent<TileFactory>().Fog;
    Map = GameObject.Find("HexGrid").GetComponent<WorldHexGrid>();
    for (int i = 0; i < Map.grid.Length; i++) {
      Tile tile = (Tile)Map.grid[i];
      if (!tile.Explored && tile.isValid) {
        fogByTiles[tile] = Instantiate(Fog);
        fogByTiles[tile].transform.parent = GameObject.Find("Fog").transform;
        fogByTiles[tile].transform.position = tile.position;
      }
    }
  }

  // Update is called once per frame
  void Update() {
    var list = new List<Tile>();

    foreach (var item in fogByTiles) {
      if (item.Key.Explored) {
        Destroy(item.Value);
        list.Add(item.Key);
      }
    }
    foreach (var key in list) {
      fogByTiles.Remove(key);
    }
  }
}
