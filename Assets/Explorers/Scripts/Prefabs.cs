using UnityEngine;
using System.Collections;

public class Prefabs : MonoBehaviour {
  public GameObject Tile = null;

  // Use this for initialization
  void Start() {
    if (instance != null) {
      throw new System.Exception("Only one prefabs library is allowed.");
    }
    instance = this;
  }

  // Update is called once per frame
  void Update() {

  }

  private static Prefabs instance = null;
  public static Prefabs Instance {
    get {
      return instance;
    }
  }
}
