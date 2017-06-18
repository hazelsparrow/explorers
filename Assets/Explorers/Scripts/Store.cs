using UnityEngine;
using System.Collections;

public class MoveCost {
  public float Get(Biome type) {
    switch (type) {
      case Biome.Desert:
        return 20;
      case Biome.Dirt:
        return 10;
      case Biome.Forest:
        return 20;
      case Biome.Hills:
        return 20;
      case Biome.Marsh:
        return 25;
      case Biome.Mountain:
        return 45;
      case Biome.Plains:
        return 10;
      case Biome.Ocean:
      default:
        return 0;
    }
  }
}

public class WorldGenerationSettings {
  public int Width { get; set; }
  public int Height { get; set; }
}

public class Store {
  public static MoveCost MoveCost = new MoveCost();
}
