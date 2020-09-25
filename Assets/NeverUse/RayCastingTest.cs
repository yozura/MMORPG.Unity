using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastingTest : MonoBehaviour
{
    // Local <-> World <-> (Viewport <-> Screen)(화면)
    void RayScreenPointToRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name} !");
            }
        }
    }

    void RayCastingUseLayerMask()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
            // int mask = 768; // (1 << 8) | (1 << 9);
            LayerMask mask = LayerMask.GetMask("Monster");
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 100.0f, mask))
            {
                Debug.Log($"Raycast Camera @ {hitinfo.collider.gameObject.name} !");
            }
        }
    }

    void RayCastingALLTest()
    {
        #region RayCasting
        // transform.forward;
        // transform.TransformDirection(Vector3.forward);
        Vector3 look = transform.localToWorldMatrix * Vector3.forward;
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}!");
        }
        #endregion RayCasting
    }

    void RayCastingScreenToWorld()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dir = mousePos - Camera.main.transform.position;
            dir = dir.normalized;

            Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name} !");
            }
        }
    }

    void FindScreenCoordinate()
    {
        Debug.Log(Input.mousePosition); // Screen 좌표
    }

    void FindViewPortCoordinate()
    {
        Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); // ViewPort 좌표
    }
}
