using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] internal Animator animatedChar, ragdollChar;
    [SerializeField] List<Transform> ragdollBones;
    [SerializeField] private float blendDuration = 0.5f;
    [SerializeField] Transform hips;
    [SerializeField] internal Rigidbody head;
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
    public IEnumerator BlendToAnimation(GameObject seat)
    {
        animatedChar.SetBool("Sit", true);
        ragdollChar.SetBool("Sit", true);
        float elapsedTime = 0f;
        transform.position = hips.transform.position;
        hips.localPosition = Vector3.zero;
        Vector3 seatPos = seat.transform.position; seatPos.y = 0.28f; seatPos.z -= 0.34f;
        transform.DOMove(seatPos, .5f);
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
        ragdollChar.enabled = true;
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
