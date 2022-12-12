using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableObject : XRGrabInteractable
{
    public bool userOffset;

    private Vector3 initialAttachLocalPosition;
    private Quaternion initialAttachLocalRotation;

    private void Start()
    {
        if (attachTransform == null)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPosition = attachTransform.localPosition;
        initialAttachLocalRotation = attachTransform.localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (userOffset)
        {
            if (args.interactorObject is XRDirectInteractor)
            {
                attachTransform.position = args.interactorObject.transform.position;
                attachTransform.rotation = args.interactorObject.transform.rotation;
            }
            else
            {
                attachTransform.localPosition = initialAttachLocalPosition;
                attachTransform.localRotation = initialAttachLocalRotation;
            }
        }
        base.OnSelectEntered(args);
    }
}
