using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
    public enum eANCHOR 
    {
        NONE = 0,
        TOP ,
        BOTTOM ,
        LEFT ,
        RIGHT ,
        CENTER ,

    };

    [SerializeField]eANCHOR m_Anchor = eANCHOR.NONE;

    // Update is called once per frame
    void Update()
    {
        if (m_Anchor == eANCHOR.NONE)
        {
            return;
        }

        Vector3 anchorPos = Vector3.zero;
        Camera viewCam = Camera.main;
        float anchorX = viewCam.aspect * viewCam.orthographicSize;
        float anchorY = viewCam.orthographicSize;
        switch(m_Anchor)
        {
            case eANCHOR.LEFT:      anchorPos.x = -anchorX;  break;
            case eANCHOR.RIGHT:     anchorPos.x =  anchorX;  break;
            case eANCHOR.TOP:       anchorPos.y =  anchorY;  break;
            case eANCHOR.BOTTOM:    anchorPos.y = -anchorY;  break;
        }

        transform.position = anchorPos;
    }
}

