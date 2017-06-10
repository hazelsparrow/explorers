using UnityEngine;
using System.Collections;
using UnityEditor;
using MapNavKit;
using Explorers;

[CustomEditor(typeof(WorldHexGrid))]
public class WorldHexGridInspector : MapNavBaseInspector {
  public override void OnInspectorGUI() {
    base.OnInspectorGUI();
  }

  protected override void OnSceneGUI() {
    base.OnSceneGUI();
  }
}
