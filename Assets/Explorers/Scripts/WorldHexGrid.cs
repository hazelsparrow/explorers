using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapNavKit;

namespace Explorers {

  public class WorldHexGrid : MapNavHexa {
    public GameObject hex1;
    public GameObject hex2;
    public GameObject hex3;
    public GameObject hex4;
    public GameObject hex5;
    public GameObject hex6;
    public GameObject hex7;

    private List<GameObject> tiles = new List<GameObject>();

    public void Start() {
      tiles.Add(hex1);
      tiles.Add(hex2);
      tiles.Add(hex3);
      tiles.Add(hex4);
      tiles.Add(hex5);
      tiles.Add(hex6);
      tiles.Add(hex7);

      CreateGrid();
    }

    public override void OnGridChanged(bool created) {
      // The parent object that will hold all the instantiated tile objects
      Transform parent = gameObject.transform;

      // Remove existing tiles and place new ones if map was (re)created
      // since the number of tiles might be different now
      if (created) {
        for (int i = parent.childCount - 1; i >= 0; i--) {
          if (Application.isPlaying) {  // was called at runtime
            Object.Destroy(parent.GetChild(i).gameObject);
          } else {  // was called from editor
            Object.DestroyImmediate(parent.GetChild(i).gameObject);
          }
        }

        // Place tiles according to the generated grid
        for (int idx = 0; idx < grid.Length; idx++) {
          // make sure it is a valid node before placing tile here
          if (false == grid[idx].isValid) continue;

          // create a new tile
          GameObject go = (GameObject)Instantiate(tiles[Random.Range(0, 7)]);
          go.name = "Tile " + idx.ToString();
          go.transform.position = grid[idx].position;
          go.transform.parent = parent;
        }

      }

      // else, simply update the position of existing tiles
      else {
        for (int idx = 0; idx < grid.Length; idx++) {
          // make sure it is a valid node before processing it
          if (false == grid[idx].isValid) continue;

          // Since I gave the tiles proper names I can easily find them by name
          GameObject go = parent.Find("Tile " + idx.ToString()).gameObject;
          go.transform.position = grid[idx].position;
        }
      }
    }

    protected void Update() {
    }

    // ------------------------------------------------------------------------------------------------------------
  }
}
