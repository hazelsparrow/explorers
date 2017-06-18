using UnityEngine;
using System.Collections;
using MapNavKit;

public enum Biome {
  Snow = 0,
  Grass = 1, 
  Desert = 2
}

// 1**: snow features
// 2**: temperate features
// 3**: desert features
public enum Feature {
  None = -1,

  SnowDefault = 100,
  SnowForest = 101,
  SnowHills = 102,
  SnowMountain = 103,
  SnowDirt = 104,
  SnowField = 105,
  SnowIcebergs = 106,

  GrassDefault = 200,
  GrassForest = 201,
  GrassHills = 202,
  GrassMountain = 203,
  GrassDirt = 204,
  GrassMarsh = 205,
  GrassColdPlains = 206,
  GrassOcean = 207,

  DesertDefault = 300,
  DesertForest = 301,
  DesertHills = 302,
  DesertMountain = 303,
  DesertDirt = 304,
  DesertGrass = 305,
  DesertMesa = 306,
  DesertMesaLarge = 307,
  DesertCrater = 308,
  DesertCactiForest = 309
}

public enum Place {
  None = -1,
  ForestClearing = 1,
  Cave = 2,
  Oasis = 3,
  Ruins = 4,
  Forester = 5,
  Farm = 6,
  Henge = 7,
  Inn = 8,
  Pyramids = 9,
  SmallVillage = 10,
  LargeVillage = 11,
  Town = 12,
  City = 13,
  Temple = 14,
  Mine = 15,
  Graveyard = 16,
  Smithy = 17,
  MountainFortress = 18
}

public class Tile : MapNavNode {
  public Biome Biome = Biome.Grass;
  public Feature Feature = Feature.None;
  public Place Place = Place.None;
  public Unit Unit = null;
  public bool Explored = false;

  // Use this for initialization
  void Start() {
  }

  // Update is called once per frame
  void Update() {

  }
}
