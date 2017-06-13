using UnityEngine;
using System.Collections;

public class UnitFactory : MonoBehaviour {
  public GameObject Face1 = null;
  public GameObject Face2 = null;
  public GameObject Face3 = null;

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public GameObject CreateRandomPlayer() {
    var r = Random.Range(0, 3);
    GameObject go = null;
    switch (r) {
      case 0:
        go = Face1;
        break;
      case 1:
        go = Face2;
        break;
      case 2:
        go = Face3;
        break;
    }
    var player = Instantiate<GameObject>(go);
    player.AddComponent<Unit>();
    return player;
  }
}
