using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace NewNamespace
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private GameObject _nextScreen;
        [SerializeField] private TextMeshProUGUI _loadingText;

    	private void Start()
    	{
            _loadingText.DOFade(0, .5f).SetLoops(-1, LoopType.Yoyo);
            StartCoroutine(Load());
    	}

        private IEnumerator Load()
        {
            yield return new WaitForSeconds(7);
            gameObject.SetActive(false);
            _nextScreen.SetActive(true);
        }
    }
}
