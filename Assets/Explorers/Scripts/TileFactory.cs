using UnityEngine;
using System.Collections;

public class TileFactory : MonoBehaviour {
  public GameObject Desert = null;
  public GameObject Dirt = null;
  public GameObject Forest = null;
  public GameObject Hills = null;
  public GameObject Marsh = null;
  public GameObject Mountain = null;
  public GameObject Ocean = null;
  public GameObject Plains = null;
  public GameObject Fog = null;

  // Use this for initialization
  void Start() {
    
  }

  // Update is called once per frame
  void Update() {

  }

  public void ConfigureRandomTile(Tile tile) {
    var r = Random.Range(0, 8);
    tile.Biome = (Biome)r;

    ConfigureTile(tile);
  }

  private void ConfigureTile(Tile tile) {
    switch (tile.Biome) {
      case Biome.Desert:
        tile.Sprite = Desert;
        break;
      case Biome.Dirt:
        tile.Sprite = Dirt;
        break;
      case Biome.Forest:
        tile.Sprite = Forest;
        break;
      case Biome.Hills:
        tile.Sprite = Hills;
        break;
      case Biome.Marsh:
        tile.Sprite = Marsh;
        break;
      case Biome.Mountain:
        tile.Sprite = Mountain;
        break;
      case Biome.Ocean:
        tile.Sprite = Ocean;
        break;
      case Biome.Plains:
        tile.Sprite = Plains;
        break;
      default:
        throw new System.Exception("Boo");
    }

    tile.Explored = false;
  }
}
