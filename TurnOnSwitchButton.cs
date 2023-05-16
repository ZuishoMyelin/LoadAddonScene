using RPGMaker.Codebase.Runtime.Common;
using UnityEngine;

public class TurnOnSwitchButton : MonoBehaviour
{
    public void OnClick() {
        DataManager.Self().GetRuntimeSaveDataModel().switches.data[0] = true;
    }
}
