using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    private void OnEnable()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.SetText(GameManager.Instance.playerScore.ToString());
    }
}
