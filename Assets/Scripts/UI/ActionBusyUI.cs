using UnityEngine;

namespace RS{
    public class ActionBusyUI : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameObject.SetActive(false);

            UnitActionSystem.instance.ON_ACTION_RUNNING += (sender, args) => { gameObject.SetActive(true); };
            UnitActionSystem.instance.ON_ACTION_CLEAR += (sender, args) => { gameObject.SetActive(false); };
        }
    }
}
