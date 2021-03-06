using UnityEngine;
using UnityEditor;

namespace SamDriver.Decal
{
  public static class StaticDecalMenuItem
  {
    [MenuItem("GameObject/3D Object/Driven Decal")]     
    static void CreateDecal()
    {
      GameObject decalObject = new GameObject("Decal");
      decalObject.AddComponent<MeshFilter>();
      var meshRenderer = decalObject.AddComponent<MeshRenderer>();
      var decal = decalObject.AddComponent<DecalMesh>();

      // do NOT set to be used in static batching - Unity breaks something with UVs
      GameObjectUtility.SetStaticEditorFlags(decalObject,
        StaticEditorFlags.ContributeGI |
        StaticEditorFlags.OccludeeStatic |
        StaticEditorFlags.ReflectionProbeStatic
      );

      // decals shouldn't cast shadows
      meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

      // set as child of selected object, if possible
      if (Selection.activeTransform != null)
      {
        decalObject.transform.SetParent(Selection.activeTransform, false);
      }

      decal.DecalAsset = FetchSampleDecalAsset();

      // generate initial quad mesh
      decal.GenerateSimpleQuadMesh();

      // select the created item (mimicking behaviour of adding a built-in object)
      Selection.activeObject = decalObject;
    }

    static DecalAsset FetchSampleDecalAsset()
    {
      return (DecalAsset)AssetDatabase.LoadAssetAtPath(
        "Assets/DrivenDecals/Sample Decals/Simple/Grid.asset",
        typeof(DecalAsset)
      );
    }
  }
}
