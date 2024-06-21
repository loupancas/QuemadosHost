using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
  public static void SetRenderLayerInChildren(Transform transform, int layerNumber)
  {
        foreach (Transform trans in transform.GetComponentInChildren<Transform>(true))
       trans.gameObject.layer = layerNumber;
    }
}
