using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BindTextWithWorker : MonoBehaviour
{
    [SerializeField] TMP_Text workerText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnSettlerBunnyChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSettlerBunnyChanged -= Refresh;
    }

    private void Refresh()
    {
        workerText.SetText($"x{GameManager.Instance.SettlerBunny}");
    }
}
