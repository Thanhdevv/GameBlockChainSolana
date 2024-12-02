using UnityEngine;
using UnityEngine.UI;

public class WalletLink : MonoBehaviour
{
    // G?n nút b?m vào Unity
    public Button connectButton;

    void Start()
    {
        connectButton.onClick.AddListener(OnConnectButtonClick);
    }

    // Khi nh?n nút, m? liên k?t ??n ví Phantom
    void OnConnectButtonClick()
    {
        // URL Deep Link ??n ví Phantom
        string phantomUrl = "https://phantom.app/"; // ho?c deep link ??n ví Phantom

        // M? liên k?t ví Phantom trên trình duy?t
        Application.OpenURL(phantomUrl);
    }
}
