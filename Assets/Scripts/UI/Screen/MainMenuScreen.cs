using HaloKero.UI.Overlap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HaloKero.UI.Screen
{
    public class MainMenuScreen : BaseScreen
    {
        public override void Hide()
        {
            base.Hide();
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            UIManager.Instance?.ShowOverlap<MainMenuOverlap>(forceShowData: true);
        }
    }
}

