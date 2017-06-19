using UnityEngine;
using System.Collections;

public class Go {
  private static GameObject engine = null;
  public static GameObject Engine {
    get {
      if (engine == null) {
        engine = GameObject.Find("Engine");
      }
      return engine;
    }
  }
}
