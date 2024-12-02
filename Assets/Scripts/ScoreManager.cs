using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI fillScore;
    public TextMeshProUGUI fillKill;

    private int currentScore;
    private int currentKill;
    private FirebaseFirestore db;

    private void Awake()
    {
        Instance = this;

        // Kh?i t?o Firebase Firestore
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
            }
            else
            {
                Debug.LogError("Firebase kh�ng s?n s�ng: " + task.Result);
            }
        });
    }

    private void Start()
    {
        // ??t gi� tr? ban ??u
        FileReadWrite.Instance.UpdateKill(0);
        FileReadWrite.Instance.UpdateScore(0);
        FillInf();
    }

    public void UpdateScore(int score)
    {
        currentScore += score;
        FileReadWrite.Instance.UpdateScore(currentScore);
        FillInf();

        // L?u ?i?m l�n Firestore
        SaveScoreToFirestore("player123");  // Thay "player123" b?ng ID ng??i ch?i th?c t?
    }

    public void UpdateKill()
    {
        currentKill++;
        FileReadWrite.Instance.UpdateKill(currentKill);
        FillInf();

        // L?u s? l??ng gi?t l�n Firestore
        SaveKillToFirestore("player123");  // Thay "player123" b?ng ID ng??i ch?i th?c t?
    }

    private void FillInf()
    {
        fillScore.text = "Score: " + currentScore;
        fillKill.text = "Kill: " + currentKill;
    }

    // Ph??ng th?c l?u ?i?m l�n Firestore
    private void SaveScoreToFirestore(string playerId)
    {
        DocumentReference playerRef = db.Collection("players").Document(playerId);
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "score", currentScore },
            { "kill", currentKill }
        };

        playerRef.SetAsync(playerData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("?i?m v� s? l??ng gi?t ?� ???c l?u l�n Firestore");
            }
            else
            {
                Debug.LogError("L?i khi l?u d? li?u l�n Firestore: " + task.Exception);
            }
        });
    }

    // Ph??ng th?c l?u s? l??ng gi?t l�n Firestore
    private void SaveKillToFirestore(string playerId)
    {
        DocumentReference playerRef = db.Collection("players").Document(playerId);
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
            { "kill", currentKill }
        };

        playerRef.SetAsync(playerData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("S? l??ng gi?t ?� ???c l?u l�n Firestore");
            }
            else
            {
                Debug.LogError("L?i khi l?u d? li?u s? l??ng gi?t l�n Firestore: " + task.Exception);
            }
        });
    }
}
