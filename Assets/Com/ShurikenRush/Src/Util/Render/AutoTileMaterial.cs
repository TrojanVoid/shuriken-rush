#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoTileMaterial : MonoBehaviour
{
    public Vector2 tilingPerUnit = new Vector2(1, 1);

    private Renderer rend;
    private Material instanceMaterial;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        CreateMaterialInstance();
        UpdateTiling();
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }

#if UNITY_EDITOR
    void EditorUpdate()
    {
        UpdateTiling();
    }
#endif

    void OnValidate()
    {
        rend = GetComponent<Renderer>();
        CreateMaterialInstance();
        UpdateTiling();
    }

    void CreateMaterialInstance()
    {
        if (rend == null) return;
        if (instanceMaterial == null)
        {
            instanceMaterial = new Material(rend.sharedMaterial);
            instanceMaterial.name = rend.sharedMaterial.name + "_Instance";
            rend.sharedMaterial = instanceMaterial;
        }
    }

    void UpdateTiling()
    {
        if (instanceMaterial == null || rend == null) return;

        Vector3 scale = transform.localScale;
        Vector2 newTiling = new Vector2(scale.x * tilingPerUnit.x, scale.z * tilingPerUnit.y);

        instanceMaterial.SetTextureScale("_BaseMap", newTiling);
        if (instanceMaterial.HasProperty("_BumpMap"))
            instanceMaterial.SetTextureScale("_BumpMap", newTiling);
        if (instanceMaterial.HasProperty("_MetallicGlossMap"))
            instanceMaterial.SetTextureScale("_MetallicGlossMap", newTiling);

#if UNITY_EDITOR
        // Force Scene view to repaint so changes appear immediately
        SceneView.RepaintAll();
#endif
    }
}