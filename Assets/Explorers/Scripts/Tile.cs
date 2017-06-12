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

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
