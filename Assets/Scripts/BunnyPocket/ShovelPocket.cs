using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShovelPocket : Pocket
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GameManager.Instance.IsDraggingShovel = true;
    }
    public override void OnRelease()
    {
        GameManager.Instance.IsDraggingShovel = false;
        if (Utility.IsPointerOverTile(out Tile tile))
        {
            tile.RemoveSettler();
        }
    }
}
