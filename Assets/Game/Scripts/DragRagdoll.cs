using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DragRagdoll : MonoBehaviour
{
    //Gizmos
    Vector3 lastHitPoint;
    bool seatFound = false;

    [SerializeField] Rigidbody selectedRb;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] internal Seat selectedSeat;
    [SerializeField] internal CharacterBehaviour character;

    [SerializeField] LayerMask groundMask;
    [SerializeField] internal Vector3 offset;

    private bool isDragging = false;
    [SerializeField] float followSpeed = 10f;
    [SerializeField] float sphereRadius = .2f;

    public void SetCharacter(CharacterBehaviour characterBehaviour, CharacterSO characterSO, UnityAction completeEvent)
    {
        character = characterBehaviour;
        character.characterSO = characterSO;
        character.completeEvent = completeEvent;
        selectedRb = character.head;

        character.OpenRagdoll();
        lineRenderer.positionCount = 2;
        isDragging = true;
        selectedRb.isKinematic = true;
        selectedRb.transform.DORotate(new Vector3(-30, 180, 0), .2f);
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && character)
        {
            character.ghostCloth.SetActive(false);
            StartCoroutine(AnimatorActivate());
            lineRenderer.positionCount = 0;
            isDragging = false;
            selectedRb.isKinematic = false;
        }
    }
    IEnumerator AnimatorActivate()
    {
        yield return new WaitForSeconds(0f);
        if (!selectedSeat)
            Destroy(character.gameObject);
        else
            StartCoroutine(character.BlendToAnimation(selectedSeat));
        selectedSeat = null;
        character = null;
    }
    void FixedUpdate()
    {
         if (isDragging && character)
         {
             Vector3 targetPosition = GetMouseWorldPosition();
             // Hips'i hareket ettir
             selectedRb.MovePosition(Vector3.Lerp(selectedRb.position, targetPosition, Time.fixedDeltaTime * followSpeed));

             // LineRenderer güncelle
             lineRenderer.SetPosition(0, selectedRb.position); // Çizginin baþlangýcý hips konumu
             lineRenderer.SetPosition(1, GetMouseRaycastHit()); // Çizginin ucu mouse pozisyonu
         }
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(selectedRb.position).z; // Derinlik korunsun
        Vector3 returnedPos = Camera.main.ScreenToWorldPoint(mousePos);
        returnedPos.y = offset.y;
        returnedPos.z = returnedPos.z + offset.z;
        returnedPos.x = returnedPos.x + offset.x;
        return returnedPos;
    }

    Vector3 GetMouseRaycastHit()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.y += offset.z * 150;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        selectedSeat = null;
        character.GhostBody(null);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            lastHitPoint = hit.point; // Son hit noktasýný kaydet
            seatFound = false; // Önce seat bulunmadý olarak ayarla

            // OverlapSphere ile en yakýn Seat'i bul
            Collider[] hits = Physics.OverlapSphere(hit.point, sphereRadius, groundMask);
            foreach (var col in hits)
            {
                if (col.TryGetComponent<Seat>(out Seat seat))
                {
                    character.GhostBody(seat);
                    selectedSeat = seat;
                    seatFound = true; // Seat bulundu
                    return seat.transform.position; // Seat'in pozisyonuna snap
                }
            }
            return hit.point; // Eðer Seat yoksa normal hit noktasýný döndür
        }
        return Vector3.zero; // Eðer çarpmazsa boþ bir deðer döndür
    }

    void OnDrawGizmos()
    {
        if (lastHitPoint != Vector3.zero)
        {
            Gizmos.color = seatFound ? Color.green : Color.red; // Eðer seat varsa yeþil, yoksa mavi
            Gizmos.DrawWireSphere(lastHitPoint, sphereRadius); // OverlapSphere çapýnda küre çiz
        }
    }

}