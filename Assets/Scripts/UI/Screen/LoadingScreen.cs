using HaloKero.UI.Overlap;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace HaloKero.UI.Screen
{
    public class LoadingScreen : BaseScreen
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TMP_Text _loadingText;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnGameLoading, UpdateProgress);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.OnGameLoading, UpdateProgress);
        }


        private void UpdateProgress(object data)
       {
            float prog = (float)data;
            if (prog >= 1)
            {
                return;
            }
            else
            {
                _loadingSlider.value = prog;
                _loadingText.text = (Mathf.Floor(prog * 100f)).ToString() + '%';
            }
       }
    }
}

