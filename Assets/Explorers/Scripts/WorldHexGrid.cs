using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapNavKit;

namespace Explorers {

  public class WorldHexGrid : MapNavHexa {
    public GameObject player;
    private TileFactory factory;

    private List<GameObject> tiles = new List<GameObject>();

    public void Start() {
      factory = GameObject.Find("Engine").GetComponent<TileFactory>();
      CreateGrid<Tile>();
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
          var tile = (Tile)grid[idx];
          factory.ConfigureRandomTile(tile);
          GameObject go = Instantiate(tile.Sprite);
          go.name = "T" + idx.ToString();
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
          GameObject go = parent.Find("T" + idx.ToString()).gameObject;
          go.transform.position = grid[idx].position;
        }
      }


      // player
      player.transform.position = NodeAt<MapNavNode>(0, 0).position;
      var currentTile = NodeAt<Tile>(0, 0);
      ExploreAroundTile(currentTile);
      player.GetComponent<Unit>().tile = currentTile;
      GameObject.Find("Engine").GetComponent<HexFog>().InitFog();
    }

    public void ExploreAroundTile(Tile tile) {
      foreach (var node in NodesAround<Tile>(tile, 1, null)) {
        node.Explored = true;
        Debug.Log("explored: " + node);
      }
      tile.Explored = true;
    }

    protected virtual float OnNodeCostCallback(MapNavNode fromNode, MapNavNode toNode) {
      var tile = (Tile)toNode;
      return tile.MoveCost;
    }

    protected void OnUnitMoveComplete() {
    }

    protected void Update() {
      if (Input.GetMouseButtonDown(0)) {
        var result = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        var go = result.collider.gameObject;
        var index = int.Parse(go.name.Remove(0, 1));
        //player.transform.position = grid[index].position;

        var tile = (Tile)grid[index];
        Debug.Log(tile.Type);

        var unit = player.GetComponent<Unit>();

        List<MapNavNode> path = Path<MapNavNode>(unit.tile, tile, OnNodeCostCallback);
        Debug.Log(path.Count);
        if (path != null) {
          //unitMoving = true; // need to wait while unit is moving
          //ClearMoveMarkers();
          unit.Move(path, OnUnitMoveComplete);
        }
      }


    }

    // ------------------------------------------------------------------------------------------------------------
  }
}
