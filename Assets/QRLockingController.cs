using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QRTracking;
using UnityEngine.XR.ARFoundation;

public class QRLockingController : MonoBehaviour
{
    [SerializeField]
    private SpatialGraphNodeTracker nodeTracker;
    
    [SerializeField]
    private QRCode qrCode;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private bool _toggled = false;
    private ARAnchor _anchor = null;

    public void ToggleCode()
    {
        _toggled = !_toggled;
        nodeTracker.enabled = !_toggled;
        qrCode.enabled = !_toggled;
        meshRenderer.material.color = _toggled ? Color.green:Color.blue;
        if (_toggled)
        {
            _anchor = gameObject.AddComponent<ARAnchor>();
        }
        else
        {
            if (_anchor != null)
            {
                Component.Destroy(_anchor);
                _anchor = null;
            }
        }
    }
}
