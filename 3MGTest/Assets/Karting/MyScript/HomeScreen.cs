using UnityEngine;

public class HomeScreen : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public StoreScript storeScript;
    
    void Start()
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
        storeScript.SetStoreUIUpdateCall(ColorUpdate);

#if UNITY_EDITOR
      //  LocalStorage.MyPlayerPrefs.DeleteAllKeys();//TODO
#endif
    }

    void ColorUpdate(bool bValues)
    {
        skinnedMeshRenderer.material.color = Util.GetColorByCode(LocalStorage.KartColor);
    }
}