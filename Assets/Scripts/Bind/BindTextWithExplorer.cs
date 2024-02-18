using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BindTextWithExplorer : MonoBehaviour
{
    [SerializeField] TMP_Text explorerText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnExplorerBunnyChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnExplorerBunnyChanged -= Refresh;
    }

    private void Refresh()
    {
        explorerText.SetText($"x{GameManager.Instance.ExplorerBunny}");
    }
}
