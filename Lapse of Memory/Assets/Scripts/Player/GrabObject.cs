using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private GameObject grabbedObject;

    [SerializeField] private float pickupRange;
    [SerializeField] private float moveForce = 250;
    [SerializeField] private Transform holdParent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null) 
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }

        if(grabbedObject != null)
        {
            MoveObject();
           
        }
    }

    private void MoveObject()
    {
        if(Vector3.Distance(grabbedObject.transform.position, holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (holdParent.position - grabbedObject.transform.position);
            grabbedObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    private void PickupObject(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            Rigidbody objectRigidbody = pickObject.GetComponent<Rigidbody>();
            objectRigidbody.useGravity = false;
            objectRigidbody.drag = 10;
            objectRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;

            objectRigidbody.transform.parent = holdParent;
            grabbedObject = pickObject;
        }
    }

    private void DropObject()
    {
        Rigidbody heldRigidbody = grabbedObject.GetComponent<Rigidbody>();
        heldRigidbody.useGravity = true;
        heldRigidbody.drag = 1;
        heldRigidbody.constraints = RigidbodyConstraints.None;

        grabbedObject.transform.parent = null;
        grabbedObject = null;
    }
}
