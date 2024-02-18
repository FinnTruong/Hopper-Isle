using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BindTextWithAdventurer : MonoBehaviour
{
    [SerializeField] TMP_Text adventurerText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnAdventurerBunnyChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAdventurerBunnyChanged -= Refresh;
    }

    private void Refresh()
    {
        adventurerText.SetText($"x{GameManager.Instance.AdventurerBunny}");
    }
}
