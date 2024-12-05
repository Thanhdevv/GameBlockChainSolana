using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
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
    private bool isFirebaseReady = false;
    private void Awake()
    {
        Instance = this;

        // Khởi tạo Firebase Firestore
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                isFirebaseReady = true;
            }
            else
            {
                Debug.LogError("Firebase không sẵn sàng: " + task.Result);
            }
        });
    }

    private void Start()
    {
        // Đợi Firebase sẵn sàng trước khi gọi LoadPlayerData
        if (isFirebaseReady)
        {
            LoadPlayerData("player123"); // Thay "player123" bằng ID người chơi thực tế
        }
        else
        {
            Debug.LogError("Firebase chưa sẵn sàng khi Start được gọi.");
        }
    }
    public void UpdateScore(int score)
    {
        currentScore += score;
        FileReadWrite.Instance.UpdateScore(currentScore);
        FillInf();

        // Lưu điểm lên Firestore
        SaveScoreToFirestore("player123");  // Thay "player123" bằng ID người chơi thực tế
    }

    public void UpdateKill()
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
