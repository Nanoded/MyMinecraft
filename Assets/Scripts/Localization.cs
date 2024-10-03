using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NewNamespace
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private List<LocalizationText> _localizationTexts;
        private TextMeshProUGUI _textMesh;

        private void Start()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
            LocalizationText localizationText = _localizationTexts.Where(x => x.Language == Application.systemLanguage).FirstOrDefault();
            if (localizationText == null)
            {
                localizationText = _localizationTexts.Where(x => x.Language == SystemLanguage.English).First();
            }
            _textMesh.text = localizationText.Text;
        }
    }

    [Serializable]
    public class  LocalizationText
    {
        [SerializeField] private SystemLanguage _language;
        [SerializeField, TextArea] private string _text;

        public SystemLanguage Language => _language;
        public string Text => _text;
    }
}
