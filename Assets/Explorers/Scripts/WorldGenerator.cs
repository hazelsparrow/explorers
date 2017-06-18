using UnityEngine;
using System.Collections;
using Explorers;
using System.Collections.Generic;

public class TilesByBiome {
  private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();

  private static Texture2D GetOrLoad(string name) {
    if (cache.ContainsKey(name)) {
      return cache[name];
    }

    var texture = Resources.Load<Texture2D>(name);
    cache[name] = texture;
    return texture;
  }

  public static Texture2D Get(Tile tile) {

  }
}

public class WorldGenerator : MonoBehaviour {
  //public Terrain terrain;
  public WorldHexGrid map;

  void InstantiateTiles() {

  }

  // Use this for initialization
  void Start() {
    map = GameObject.Find("HexGrid").GetComponent<WorldHexGrid>();

    FastNoise myNoise = new FastNoise(); // Create a FastNoise object
    myNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal); // Set the desired noise type
    myNoise.SetSeed(Random.Range(0, int.MaxValue));

    //float[,] heightMap = new float[map.mapHorizontalSize, map.mapVerticalSize]; // 2D heightmap to create terrain

    for (int x = 0; x < map.mapHorizontalSize; x++) {
      for (int y = 0; y < map.mapVerticalSize; y++) {
        map.grid.
        //heightMap[x, y] = (myNoise.GetNoise(x, y) + 1) / 2;
        //if (heightMap[x, y] < 0.1f) {
        //  heightMap[x, y] = 0;
        //} else if (heightMap[x, y] < 0.3f) {
        //  heightMap[x, y] = 0.2f;
        //} else {
        //  heightMap[x, y] = 0.4f;
        //}
      }
    }

    terrain.terrainData.SetHeights(0, 0, heightMap);
  }

  void GenerateBiomes() {

  }

  // Update is called once per frame
  void Update() {

  }
}
