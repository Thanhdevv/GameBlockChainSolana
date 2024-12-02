using UnityEngine;
using UnityEngine.UI;

public class WalletLink : MonoBehaviour
{
    // G?n n�t b?m v�o Unity
    public Button connectButton;

    void Start()
    {
        connectButton.onClick.AddListener(OnConnectButtonClick);
    }

    // Khi nh?n n�t, m? li�n k?t ??n v� Phantom
    void OnConnectButtonClick()
    {
        // URL Deep Link ??n v� Phantom
        string phantomUrl = "https://phantom.app/"; // ho?c deep link ??n v� Phantom

        // M? li�n k?t v� Phantom tr�n tr�nh duy?t
        Application.OpenURL(phantomUrl);
    }
}
