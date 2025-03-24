using UnityEngine;

public class GhostClothingSync
{
    GameObject originalClothes;
    GameObject ghostClothes;
    public GhostClothingSync(GameObject originalClothes, GameObject ghostClothes)
    {
        this.originalClothes = originalClothes;
        this.ghostClothes = ghostClothes;
        SyncClothes();
    }
    public void SyncClothes()
    {
        if (originalClothes == null || ghostClothes == null)
        {
            Debug.LogError("Kýyafetlerin atanmýþ olduðundan emin olun!");
            return;
        }

        SyncGroup(originalClothes.transform, ghostClothes.transform);
    }

    private void SyncGroup(Transform originalGroup, Transform ghostGroup)
    {
        for (int i = 0; i < originalGroup.childCount; i++)
        {
            Transform originalChild = originalGroup.GetChild(i);
            Transform ghostChild = ghostGroup.Find(originalChild.name);

            if (ghostChild != null)
            {
                ghostChild.gameObject.SetActive(originalChild.gameObject.activeSelf);
                SyncGroup(originalChild, ghostChild); // Alt objeleri de kontrol et
            }
        }
    }
}