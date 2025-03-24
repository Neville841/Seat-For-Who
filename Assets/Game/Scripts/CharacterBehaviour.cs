using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class CharacterBehaviour : MonoBehaviour
{
    internal UnityAction completeEvent;
    [SerializeField] internal GameObject ghostCloth, realCloth;

    [SerializeField] internal Animator animatedChar, ragdollChar;
    [SerializeField] internal Rigidbody head;
    [SerializeField] internal CharacterSO characterSO;

    [SerializeField] List<Transform> ragdollBones;
    [SerializeField] Transform hips;

    [SerializeField] private float blendDuration = 0.5f;
    [ContextMenu("GhostClothSet")]
    public void GhostClothSet()
    {
        GhostClothingSync ghostCloth = new GhostClothingSync(realCloth, this.ghostCloth);
    }
    public void OpenRagdoll()
    { // Karakterin tüm kemiklerini al ve listeye ekle
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            ragdollBones.Add(rb.transform);
        }
        ragdollChar.enabled = false;
        foreach (Transform bone in ragdollBones)
        {
            bone.GetComponent<Rigidbody>().useGravity = true;
            bone.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    public void GhostBody(Seat seat)
    {
        if (seat)
        {
            ghostCloth.SetActive(true);
            animatedChar.transform.position = seat.characterPos.position;
        }
        else
        {
            ghostCloth.SetActive(false);
        }
    }
    public IEnumerator BlendToAnimation(Seat seat)
    {
        if (seat.seatType != characterSO.SeatType)
        {
            Destroy(gameObject);
            yield break;
        }
        animatedChar.transform.localPosition = Vector3.zero;
        float elapsedTime = 0f;
        transform.position = hips.transform.position;
        hips.localPosition = Vector3.zero;
        transform.DOMove(seat.characterPos.position, .5f);
        while (elapsedTime < .75f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / blendDuration / 10;
            foreach (var bone in ragdollBones)
            {
                Transform animBone = GetMatchingAnimatedBone(bone);
                if (animBone == null) continue;
                bone.position = Vector3.Lerp(bone.position, animBone.position, t);
                bone.rotation = Quaternion.Slerp(bone.rotation, animBone.rotation, t);
                if (Vector3.Distance(bone.position, animBone.position) <= 0.2f)
                {
                    bone.GetComponent<Rigidbody>().useGravity = false;
                    bone.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            yield return null;
        }
        seat.SetCharacter(this);
        ragdollChar.enabled = true;
        ragdollChar.Play("Sit");
        completeEvent.Invoke();
    }

    private Transform GetMatchingAnimatedBone(Transform ragdollBone)
    {
        Transform animatedBone = FindChildRecursive(animatedChar.transform, ragdollBone.name);

        if (animatedBone == null)
        {
            Debug.LogWarning($"Kemik eþleþmedi: {ragdollBone.name}");
        }
        else
        {
            Debug.Log($"Eþleþen Kemik: {ragdollBone.name} -> {animatedBone.name}");
        }

        return animatedBone;
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindChildRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
