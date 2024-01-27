using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Tools
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Config/Localization", fileName = "New Localization Config")]
    public class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private List<LocalizedLanguage> _localizedLanguages;

        public List<LocalizedLanguage> LocalizedLanguages { get => _localizedLanguages; }
    }
}
