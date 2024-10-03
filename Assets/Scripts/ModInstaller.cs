using Cysharp.Threading.Tasks;
//using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace NewNamespace
{
    public class ModInstaller : MonoBehaviour
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private float2 _barSpeed;
        [SerializeField] private string _modName;
        [SerializeField] private Image _loadingbar;
        private float _currentSpeed;

        private void Start()
        {
            _openButton.onClick.AddListener(OpenMod);
        }

        private async void OnEnable()
        {
            //FileBrowser.RequestPermission();
            _currentSpeed = _barSpeed.x;
            _loadingbar.fillAmount = 0;
            StartCoroutine(InstallView());
            await Install();
        }

        private IEnumerator InstallView()
        {
            while(_loadingbar.fillAmount < 1)
            {
                yield return new WaitForSeconds(_currentSpeed);
                _loadingbar.fillAmount += .1f;
            }
            _openButton.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private async UniTask Install()
        {
            string shortName = Path.GetFileNameWithoutExtension(_modName);
            print(shortName);
            var loading = await Resources.LoadAsync(shortName).ToUniTask();
            List<byte> bytes = ((TextAsset)loading).bytes.ToList();
            var install = File.WriteAllBytesAsync(Path.Combine(Application.persistentDataPath, _modName), bytes.ToArray());
            await install;
            _currentSpeed = _barSpeed.y;
        }

        private void OpenMod()
        {
            //if(FileBrowser.CheckPermission() == FileBrowser.Permission.Granted) 
                AndroidContentOpenerWrapper.OpenContent(Path.Combine(Application.persistentDataPath, _modName));
            //else FileBrowser.RequestPermission();
        }
    }
}
