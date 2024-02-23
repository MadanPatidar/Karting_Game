using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public StoreScript storeScript;
    
    void Start()
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
        storeScript.SetStoreUIUpdateCall(ColorUpdate);
    }

    void ColorUpdate(bool bValues)
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
    }
}