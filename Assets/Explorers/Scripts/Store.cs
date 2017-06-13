using UnityEngine;
using System.Collections;

public class MoveCost {
  public float Get(TileType type) {
    switch (type) {
      case TileType.Desert:
        return 20;
      case TileType.Dirt:
        return 10;
      case TileType.Forest:
        return 20;
      case TileType.Hills:
        return 20;
      case TileType.Marsh:
        return 25;
      case TileType.Mountain:
        return 45;
      case TileType.Plains:
        return 10;
      case TileType.Ocean:
      default:
        return 0;
    }
  }
}

public class Store {
  public static MoveCost MoveCost = new MoveCost();
}
