﻿using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshProUGUI fillScore;
    public TextMeshProUGUI fillKill;

    private int currentScore;
    private int currentKill;
    private FirebaseFirestore db;
    private bool isFirebaseReady = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Debug.LogError("ScoreManager.Instance đã tồn tại, phá hủy đối tượng mới!");
            Destroy(gameObject);
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                isFirebaseReady = true;
                Debug.Log("Firebase đã sẵn sàng!");
            }
            else
            {
                Debug.LogError("Lỗi Firebase: " + task.Result);
            }
        });
    }
    private IEnumerator WaitForFirebaseAndLoadData(string playerId)
    {
        while (!isFirebaseReady)
        {
            yield return null; 
        }
        LoadPlayerData(playerId);
    }

    private void Start()
    {
        StartCoroutine(WaitForFirebaseAndLoadData("player123")); 
    }
    public void UpdateScore(int score)
    {
        currentScore += score;
        FileReadWrite.Instance.UpdateScore(currentScore);
        FillInf();
        SaveScoreToFirestore("player123");  
    }

    public void UpdateKill()//
    {
        currentKill++;
        FileReadWrite.Instance.UpdateKill(currentKill);
        FillInf();

        // Lưu số lượng giết lên Firestore
        SaveKillToFirestore("player123");  // Thay "player123" bằng ID người chơi thực tế
    }

    private void FillInf()
    {
        fillScore.text = "Score: " + currentScore;
        fillKill.text = "Kill: " + currentKill;
    }

    // Phương thức lưu điểm lên Firestore
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
                Debug.Log("Điểm và số lượng giết đã được lưu lên Firestore");
            }
            else
            {
                Debug.LogError("Lỗi khi lưu dữ liệu lên Firestore: " + task.Exception);
            }
        });
    }

    // Phương thức lấy dữ liệu từ Firestore
    private void LoadPlayerData(string playerId)
    {
        DocumentReference playerRef = db.Collection("players").Document(playerId);
        playerRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                Debug.Log("Dữ liệu người chơi đã được tải");
                Dictionary<string, object> playerData = task.Result.ToDictionary();

                // Gán giá trị từ Firestore
                if (playerData.ContainsKey("score"))
                {
                    currentScore = System.Convert.ToInt32(playerData["score"]);
                }

                if (playerData.ContainsKey("kill"))
                {
                    currentKill = System.Convert.ToInt32(playerData["kill"]);
                }

                FillInf(); // Cập nhật giao diện
            }
            else
            {
                Debug.LogWarning("Dữ liệu người chơi không tồn tại hoặc lỗi xảy ra: " + task.Exception);
                currentScore = 0;
                currentKill = 0;
                FillInf();
            }
        });
    }

    // Phương thức lưu số lượng giết lên Firestore
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
                Debug.Log("Số lượng giết đã được lưu lên Firestore");
            }
            else
            {
                Debug.LogError("Lỗi khi lưu dữ liệu số lượng giết lên Firestore: " + task.Exception);
            }
        });
    }
}
