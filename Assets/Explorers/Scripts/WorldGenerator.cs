using UnityEngine;
using System.Collections;
using Explorers;
using System.Collections.Generic;

public class TilesByBiome {
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

  private static Sprite RandomSpriteFromPool(string tileSheetName, params int[] pool) {
    var r = Random.Range(0, pool.Length);
    return tiles[tileSheetName][pool[r]];
  }

  private static Sprite RandomSprite(string tileSheetName, int startIndex) {
    return RandomSpriteFromPool(tileSheetName, startIndex, startIndex + 1, startIndex + 2, startIndex + 3);
  }

  public static Sprite Get(Tile tile) {
    var r4 = Random.Range(0, 4);
    switch (tile.Biome) {
      case Biome.Desert:
        switch (tile.Feature) {
          case Feature.GrassOcean:
            return RandomSprite("grass", 36);
          case Feature.DesertCactiForest:
            return RandomSprite("desert0", 4);
          case Feature.DesertForest:
            return RandomSprite("desert1", 8);
          case Feature.DesertHills:
            return RandomSprite("desert1", 24);
          case Feature.DesertMountain:
            return RandomSprite("desert1", 32);
          default:
          return RandomSprite("desert1", 4);
        }
      case Biome.Grass:
        switch (tile.Feature) {
          case Feature.GrassOcean:
            return RandomSprite("grass", 36);
          case Feature.GrassForest:
            return RandomSprite("grass", 12);
          case Feature.GrassHills:
            return RandomSprite("grass", 16);
          case Feature.GrassMountain:
            return RandomSprite("grass", 24);
          default:
            return RandomSprite("grass", 28);
        }
      case Biome.Snow:
        switch (tile.Feature) {
          case Feature.SnowIcebergs:
            return RandomSprite("snow0", 24);
          case Feature.SnowForest:
            return RandomSpriteFromPool("snow0", 6, 7, 8, 9, 10, 11, 12, 13, 14);
          case Feature.SnowHills:
            return RandomSpriteFromPool("snow0", 14, 15, 16, 17, 18, 19, 20, 21);
          case Feature.SnowMountain:
            return RandomSpriteFromPool("snow0", 0, 1, 22, 23);
          default:
            return RandomSprite("snow0", 32);
        }
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
    GenerateFeatures();
    InstantiateTiles();
  }

  FastNoise CreateBiomeNoise() {
    var noise = new FastNoise();
    noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
    return noise;
  }

  float HorizontalDistance(int i, int target, int maxDistance) {
    return 1 - Mathf.Pow(Mathf.Abs(target - i) / Mathf.Abs(target - maxDistance), 2);
  }

  float NormalizedNoise(float noise) {
    return (noise + 1f) / 2f;
  }

  void GenerateFeatures() {
    FastNoise forestNoise = new FastNoise(FastNoise.NoiseType.SimplexFractal);
    FastNoise oceanNoise = new FastNoise(FastNoise.NoiseType.SimplexFractal);
    FastNoise heightNoise = new FastNoise(FastNoise.NoiseType.PerlinFractal);
    var STEP = 20;

    for (int i = 0; i < map.mapHorizontalSize; i++) {
      for (int j = 0; j < map.mapVerticalSize; j++) {
        var tile = map.NodeAt<Tile>(i, j);
        var biome = tile.Biome;
        var feature = Feature.None;

        var forest = NormalizedNoise(forestNoise.GetNoise(i * STEP, j * STEP));
        var ocean = NormalizedNoise(oceanNoise.GetNoise(i * STEP, j * STEP)) * (1 - HorizontalDistance(j, map.mapVerticalSize / 2, map.mapVerticalSize / 8) + 1 - HorizontalDistance(i, map.mapHorizontalSize / 2, map.mapHorizontalSize / 8) + 1);
        var height = NormalizedNoise(heightNoise.GetNoise(i * STEP, j * STEP));

        if (height < 0.5f) {
          if (ocean > 0.5f) {
            switch (biome) {
              case Biome.Snow:
                feature = Feature.SnowIcebergs;
                break;
              default:
                feature = Feature.GrassOcean;
                break;
            }
          } else {
            if (forest > 0.5f) {
              switch (biome) {
                case Biome.Desert:
                  feature = forest > 0.7f ? Feature.DesertCactiForest : Feature.DesertForest;
                  break;
                case Biome.Grass:
                  feature = Feature.GrassForest;
                  break;
                case Biome.Snow:
                  feature = Feature.SnowForest;
                  break;
              }
            }
          }
        } else { // height > 0.5
          switch (biome) {
            case Biome.Desert:
              feature = height > 0.7f ? Feature.DesertMountain : Feature.DesertHills;
              break;
            case Biome.Grass:
              feature = height > 0.7f ? Feature.GrassMountain : Feature.GrassHills;
              break;
            case Biome.Snow:
              feature = height > 0.7f ? Feature.SnowMountain : Feature.SnowHills;
              break;
          }
        }

        tile.Feature = feature;
      }
    }
  }

  void GenerateBiomes() {
    FastNoise snowNoise = CreateBiomeNoise();
    FastNoise grassNoise = CreateBiomeNoise();
    FastNoise desertNoise = CreateBiomeNoise();
    var n = (int)(0.5 * map.mapHorizontalSize);
    var STEP = 20;

    for (int i = 0; i < map.mapHorizontalSize; i++) {
      for (int j = 0; j < map.mapVerticalSize; j++) {
        var snow = NormalizedNoise(snowNoise.GetNoise(i * STEP, j * STEP)) * HorizontalDistance(i, 0, n);
        var grass = NormalizedNoise(grassNoise.GetNoise(i * STEP, j * STEP)) * HorizontalDistance(i, n, n / 3);
        var desert = NormalizedNoise(desertNoise.GetNoise(i * STEP, j * STEP)) * HorizontalDistance(i, 2 * n, n);
        var max = Mathf.Max(snow, grass, desert);

        var tile = map.NodeAt<Tile>(i, j);
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
