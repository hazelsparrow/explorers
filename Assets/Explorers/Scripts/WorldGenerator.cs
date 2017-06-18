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
    tiles["desert1"] = Resources.LoadAll<Sprite>("Sprites/Tiles/painted_terrain_hexes_desert_256x384_sheet1");
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
        return tiles["grass"][28];
      case Biome.Snow:
        return tiles["snow0"][32];
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
    GenerateBiomes();
    InstantiateTiles();
  }

  FastNoise CreateBiomeNoise() {
    var noise = new FastNoise();
    noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal); 
    noise.SetSeed(Random.Range(0, int.MaxValue));
    return noise;
  }

  float HorizontalDistance(int i, int target, int maxDistance) {
    return 1 - Mathf.Pow(Mathf.Abs(target - i) / Mathf.Abs(target - maxDistance), 2);
  }

  float NormalizedNoise(float noise) {
    return (noise + 1f) / 2f;
  }

  void GenerateBiomes() {
    FastNoise snowNoise = CreateBiomeNoise();
    FastNoise grassNoise = CreateBiomeNoise();
    FastNoise desertNoise = CreateBiomeNoise();
    var width = map.mapHorizontalSize;
    var height = map.mapVerticalSize;
    var n = (int)(0.5 * width);
    var g = 20;

    for (int i = 0; i < width; i++) {
      for (int j = 0; j < height; j++) {
        var snow = NormalizedNoise(snowNoise.GetNoise(i * g, j * g)) * HorizontalDistance(i, 0, n);
        var grass = NormalizedNoise(grassNoise.GetNoise(i * g, j * g)) * HorizontalDistance(i, n, n / 3);
        var desert = NormalizedNoise(desertNoise.GetNoise(i * g, j * g)) * HorizontalDistance(i, 2 * n, n);
        var max = Mathf.Max(snow, grass, desert);

        var tile = map.NodeAt<Tile>(i, j);
        Debug.Log(string.Format("i = {0}; snow = {1}; grass = {2}; desert = {3}", i, snow, grass, desert));
        if (max == desert) {
          tile.Biome = Biome.Desert;
        } else if (max == grass) {
          tile.Biome = Biome.Grass;
        } else {
          tile.Biome = Biome.Snow;
        }
      }
    }

  }

  // Update is called once per frame
  void Update() {

  }
}
