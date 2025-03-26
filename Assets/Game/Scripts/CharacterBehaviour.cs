using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using Zenject;

public class CharacterBehaviour : MonoBehaviour
{
    [Inject] PoolingSystem poolingSystem;
    internal UnityAction completeEvent;
    [SerializeField] internal GameObject ghostCloth, realCloth;

    [SerializeField] internal Animator animatedChar, ragdollChar;
    [SerializeField] internal Rigidbody head;
    [SerializeField] internal CharacterSO characterSO;

    [SerializeField] List<Transform> ragdollBones;
    [SerializeField] internal Transform hips;

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
    bool blending;
    float elapsedTime = 0f;
    private void FixedUpdate()
    {
        if (blending)
        {
            elapsedTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / (blendDuration * 7f));
            foreach (var bone in ragdollBones)
            {
                Transform animBone = GetMatchingAnimatedBone(bone);
                if (animBone == null) continue;
                bone.position = Vector3.Lerp(bone.position, animBone.position, t);
                bone.rotation = Quaternion.Slerp(bone.rotation, animBone.rotation, t);
                if (Vector3.Distance(bone.position, animBone.position) <= 0.02f)
                {
                    bone.GetComponent<Rigidbody>().useGravity = false;
                    bone.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
    public IEnumerator BlendToAnimation(Seat seat)
    {
        animatedChar.transform.localPosition = Vector3.zero;
        transform.position = hips.transform.position;
        hips.localPosition = Vector3.zero;
        elapsedTime = 0f;
        blending = true;
        transform.DOMove(seat.characterPos.position, 1f).OnComplete(() =>
        {
            blending = false;
            ragdollChar.enabled = true;
            ragdollChar.Play("Sit");
            if (seat.seatType != characterSO.SeatType)
            {
                VfxSpawn("Angry");
                StartCoroutine(DestroyDelay());
            }
            else if (characterSO.SeatType != SeatType.None)
            {
                VfxSpawn("Happy");
                seat.SetCharacter(this);
                completeEvent.Invoke();
            }
            else if (seat.CheckSeats(this))
            {
                VfxSpawn("Happy");
                seat.SetCharacter(this);
                completeEvent.Invoke();
            }
            else
            {
                Debug.Log("sandalye yok");
                VfxSpawn("Angry");
                StartCoroutine(DestroyDelay());
            }
        });
        while (elapsedTime < 5f)
        {

            yield return null;
        }

    }
    IEnumerator DestroyDelay()
    {
        EventManager.OnWrongSeat();
        yield return new WaitForSeconds(1f);
        VfxSpawn("Poof");
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
    void VfxSpawn(string name)
    {
        Vector3 pos = transform.position;
        pos.y += 2f;
        poolingSystem.InstantiateAPS(name, pos);
    }
    private Transform GetMatchingAnimatedBone(Transform ragdollBone)
    {
        Transform animatedBone = FindChildRecursive(animatedChar.transform, ragdollBone.name);

        /* if (animatedBone == null)
         {
             Debug.LogWarning($"Kemik eþleþmedi: {ragdollBone.name}");
         }
         else
         {
             Debug.Log($"Eþleþen Kemik: {ragdollBone.name} -> {animatedBone.name}");
         }
        */
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
