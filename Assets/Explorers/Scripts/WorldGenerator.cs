using UnityEngine;
using System.Collections;
using Explorers;
using System.Collections.Generic;

public class TilesByBiome {
  //private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();

  //private static Texture2D GetOrLoad(string name) {
  //  var path = string.Format("Textures/Tiles/{0}", name);
  //  if (cache.ContainsKey(path)) {
  //    return cache[path];
  //  }

  //  var texture = Resources.Load<Texture2D>(path);
  //  cache[path] = texture;
  //  return texture;
  //}

  private static Dictionary<string, Sprite[]> tiles = new Dictionary<string, Sprite[]>();

  public static void Load() {
    tiles["snow0"] = Resources.LoadAll<Sprite>("Sprites/Tiles/cold_terrain_sheet_hex0");
    tiles["snow1"] = Resources.LoadAll<Sprite>("Sprites/Tiles/cold_terrain_sheet_hex1");
    tiles["desert0"] = Resources.LoadAll<Sprite>("Sprites/Tiles/painted_terrain_hexes_desert_256x384_sheet0");
    tiles["desert1"] = Resources.LoadAll<Sprite>("Sprites/Tiles/painted_terrain_hexes_desert_256x384_sheet0");
    tiles["grass"] = Resources.LoadAll<Sprite>("Sprites/Tiles/terrain_hextiles_painted_basic_256x384");

    foreach (var key in tiles.Keys) {
      for (int i = 0; i < tiles[key].Length; i++) {
        tiles[key][i] = Sprite.Create(tiles[key][i].texture, tiles[key][i].rect, new Vector2(0.5f, 0.33334f));
      }
    }
  }

  public static Sprite Get(Tile tile) {
    switch (tile.Biome) {
      case Biome.Desert:
        return tiles["desert1"][0];
      case Biome.Grass:
        return tiles["grass"][0];
      case Biome.Snow:
        return tiles["snow1"][8];
      default:
        throw new System.Exception("Biome not supported: " + tile.Biome);
    }
  }
}

public class WorldGenerator {
  //public Terrain terrain;
  public WorldHexGrid map;

  public WorldGenerator(WorldHexGrid map) {
    this.map = map;
  }

  void InstantiateTiles() {
    TilesByBiome.Load();
    for (int i = 0; i < map.grid.Length; i++) {
      GameObject go = Object.Instantiate(Prefabs.Instance.Tile);
      go.name = "T" + i;
      go.transform.position = map.grid[i].position;
      go.transform.parent = map.transform.parent;
      go.GetComponent<SpriteRenderer>().sprite = TilesByBiome.Get((Tile)map.grid[i]);
    }
  }

  // Use this for initialization
  public void GenerateWorld() {
    FastNoise myNoise = new FastNoise(); // Create a FastNoise object
    myNoise.SetNoiseType(FastNoise.NoiseType.PerlinFractal); // Set the desired noise type
    myNoise.SetSeed(Random.Range(0, int.MaxValue));

    //float[,] heightMap = new float[map.mapHorizontalSize, map.mapVerticalSize]; // 2D heightmap to create terrain

    for (int x = 0; x < map.mapHorizontalSize; x++) {
      for (int y = 0; y < map.mapVerticalSize; y++) {
        var noise = (myNoise.GetNoise(x, y) + 1) / 2;
        var tile = map.NodeAt<Tile>(x, y);
        if (noise < 0.2f) {
          tile.Biome = Biome.Desert;
        } else if (noise < 0.4f) {
          tile.Biome = Biome.Grass;
        } else {
          tile.Biome = Biome.Snow;
        }
        //map.grid[]
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

    InstantiateTiles();

  }

  void GenerateBiomes() {

  }

  // Update is called once per frame
  void Update() {

  }
}
