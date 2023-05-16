using RPGMaker.Codebase.Runtime.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowSwitchValueText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    void Start()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshProUGUI.text = "switch 0001:" + DataManager.Self().GetRuntimeSaveDataModel().switches.data[0].ToString();
    }
}
