using UnityEngine;
using System.Threading.Tasks; // For async/await operations

public class TokenToSolanaConverter : MonoBehaviour
{
    public int userTokenBalance = 5000; // Example user token balance
    public string adminWalletAddress = "YourAdminWalletPublicKey"; // Replace with your admin wallet address
    public float conversionRate = 1000f; // 1000 tokens = 1 Solana
    public string phantomWalletUrl = "https://phantom.app/"; // Phantom wallet URL for browser interaction

    // Method to handle the button click for conversion
    public async void ConvertTokensToSolana()
    {
        // Check if the user has enough tokens
        if (userTokenBalance >= conversionRate)
        {
            // Calculate Solana to transfer
            float solanaToTransfer = userTokenBalance / conversionRate;

            // Log the calculated transfer amount
            Debug.Log($"Attempting to transfer {solanaToTransfer} SOL from admin wallet.");

            // Deduct the tokens from user balance
            userTokenBalance -= (int)conversionRate;

            // Call the function to transfer Solana
            bool success = await TransferSolanaToUser(solanaToTransfer);

            if (success)
            {
                Debug.Log($"Successfully transferred {solanaToTransfer} SOL to the user.");
            }
            else
            {
                Debug.LogError("Failed to transfer Solana. Reverting token deduction.");
                userTokenBalance += (int)conversionRate; // Refund tokens if the transfer fails
            }
        }
        else
        {
            Debug.LogError("Not enough tokens to convert to Solana.");
        }
    }

    // Mockup of a method that handles Solana transfer
    private async Task<bool> TransferSolanaToUser(float solanaAmount)
    {
        // Connect to the Phantom wallet and initiate transfer
        try
        {
            // Call the Phantom wallet (JavaScript interaction)
            Application.ExternalEval(@$"
                if (window.solana && window.solana.isPhantom) {{
                    window.solana.connect().then(() => {{
                        const transaction = new web3.Transaction();
                        transaction.add(
                            web3.SystemProgram.transfer({{
                                fromPubkey: '{adminWalletAddress}',
                                toPubkey: window.solana.publicKey.toBase58(),
                                lamports: web3.LAMPORTS_PER_SOL * {solanaAmount}
                            }})
                        );
                        console.log('Transaction initiated');
                    }});
                }} else {{
                    console.error('Phantom wallet not installed.');
                }}
            ");

            // Simulate asynchronous wait for a successful transfer
            await Task.Delay(2000); // Simulate network delay
            return true; // Simulate success
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error during Solana transfer: {ex.Message}");
            return false; // Return failure
        }
    }
}
