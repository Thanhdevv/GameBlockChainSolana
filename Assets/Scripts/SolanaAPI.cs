using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SolanaAPI : MonoBehaviour
{
    private string rpcUrl = "https://api.testnet.solana.com"; // Endpoint Testnet

    // Hàm g?i token
    public void SendTransaction(string senderPrivateKey, string recipientPublicKey, ulong amount)
    {
        string transactionPayload = $@"
        {{
            ""jsonrpc"": ""2.0"",
            ""id"": 1,
            ""method"": ""sendTransaction"",
            ""params"": [
                ""{senderPrivateKey}"",
                ""{recipientPublicKey}"",
                {amount}
            ]
        }}";

        StartCoroutine(PostRequest(rpcUrl, transactionPayload));
    }

    // G?i yêu c?u POST
    IEnumerator PostRequest(string url, string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Giao d?ch thành công: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("L?i giao d?ch: " + request.error);
        }
    }
}
