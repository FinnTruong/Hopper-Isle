using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utility
{
    public static bool IsPointerOverUIElement(Vector3 offset = default)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(offset));
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaycastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults(Vector3 offset = default)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition + offset;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


    public static bool IsPointerOverTile(out Tile tile)
    {
        tile = null;
        RaycastHit2D tileHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if(tileHit)
        {
            var targetTile = tileHit.collider.gameObject.GetComponent<Tile>();
            tile = targetTile;
            return true;
        }

        return false;
    }
}
