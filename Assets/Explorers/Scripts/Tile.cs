using UnityEngine;
using System.Collections;
using MapNavKit;

public enum TileType {
  Desert = 0,
  Dirt = 1,
  Forest = 2,
  Hills = 3,
  Marsh = 4,
  Mountain = 5,
  Ocean = 6,
  Plains = 7,
  Fog = -1
}

public class Tile : MapNavNode {
  public TileType Type = TileType.Fog;
  public GameObject Sprite = null;
  public Unit Unit = null;
  public bool Explored = false;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public float MoveCost {
    get {
      switch (Type) {
        case TileType.Ocean:
        case TileType.Mountain:
          return 0f;
        default:
          return 1f;
      }
    }
  }
}
