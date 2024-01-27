using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNotify : BaseUIElement
{
    public override void Hide()
    {
        base.Hide();
    }

    public override void Init()
    {
        base.Init();
        uiType = UIType.Notify;
    }

    public override void Show(object data)
    {
        base.Show(data);
    }
}
