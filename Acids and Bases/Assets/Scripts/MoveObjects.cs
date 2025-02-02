using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    public string draggingTag; 
    public Camera mainCamera; 

    private Vector3 dis; 
    private float posX; 
    private float posY; 
    private bool touched = false; 
    private bool dragging = false; 
    private Transform toDrag; 
    private Rigidbody toDragRigidbody; 
    private Vector3 previousPosition; 


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.touchCount != 1)
        {
            dragging = false;
            touched = false;
            if (toDragRigidbody) 
            {
                SetFreeProperties(toDragRigidbody);
            }
            return; 
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position; 

        if(touch.phase == TouchPhase.Began)
        {
            RaycastHit hit; 
            Ray ray = mainCamera.ScreenPointToRay(pos); 

            if(Physics.Raycast(ray,out hit) && hit.collider.tag == draggingTag)
            {
                toDrag = hit.transform; 
                previousPosition = toDrag.position; 
                toDragRigidbody = toDrag.GetComponent<Rigidbody>();
                dis = mainCamera.WorldToScreenPoint(previousPosition); 
                posX = Input.GetTouch(0).position.x - dis.x; 
                posY = Input.GetTouch(0).position.y - dis.y;

                SetDraggingProperties(toDragRigidbody);

                touched = true; 

            }

        }
        if (touched && touch.phase == TouchPhase.Moved)
        {
            dragging = true; 
            float posXNow = Input.GetTouch(0).position.x - posX; 
            float posYNow = Input.GetTouch(0).position.y - posY; 
            Vector3 curPos = new Vector3(posXNow, posYNow, dis.z); 

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(curPos) - previousPosition;
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0.0f); 

            toDragRigidbody.velocity = worldPosition/ (Time.deltaTime*10);
            previousPosition = toDrag.position; 

        }   

        if(dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging =false; 
            touched = false; 
            previousPosition = new Vector3(0.0f, 0.0f, 0.0f); 
            SetFreeProperties(toDragRigidbody);
        }
    }
    private void SetDraggingProperties(Rigidbody rb)
    {
        rb.useGravity = false; 
        rb.drag = 20; 
    }
    private void SetFreeProperties(Rigidbody rb) 
    {
        rb.useGravity = true; 
        rb.drag = 5; 
    }
}
