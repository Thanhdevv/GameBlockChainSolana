using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileReadWrite : MonoBehaviour
{

    public static FileReadWrite Instance;
    public Player player;


    private string nameFile = "playerData.json";

    private void Awake()
    {
/*        string filePath = Path.Combine(Application.persistentDataPath, nameFile);
        File.Delete(filePath);*/
        Instance = this;
        ReadFile();
    }

    public void WriteFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, nameFile);
        string jsonData = JsonUtility.ToJson(player);

        File.WriteAllText(filePath, jsonData);
        ReadFile();
    }
    public void ReadFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, nameFile);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);

            player = JsonUtility.FromJson<Player>(jsonData);
            Debug.Log(" - Player Id: " + player.id + "\n - Player Level: " + player.level + "\n - Player Current Exp: " + player.currentExp + "\n - Player Kill: " + player.kill + "\n - Player Score: " + player.score + "\n - Player Skin: " + player.selectPlayer + "\n - Player Weapon: " + player.idItemWeapon + " - " + player.isCave);
        }
        else
        {
            player = CreateNewPlayer();
            WriteFile();
        }
    }



    public void UpdateLevel(int level)
    {
        player.level = level;
        WriteFile();
    }
    public void UpdateCurrentExp(int curExp)
    {
        player.currentExp = curExp;
        WriteFile();
    }
    public void UpdateKill(int kill)
    {
        player.kill = kill;
        WriteFile();
    }
    public void UpdateScore(int score)
    {
        player.score = score;
        WriteFile();
    }
    public void UpdateItemWeapon(int id)
    {
        player.idItemWeapon = id;
        WriteFile();
    }
    public void UpdateIsCave()
    {
        player.isCave = true;
        WriteFile();
    }
    public void UpdateSelectPlayer(int index)
    {
        player.selectPlayer = index;
        WriteFile();
    }
    public Player CreateNewPlayer()
    {
        Player newPlayer = new Player();
        newPlayer.id = Guid.NewGuid().ToString();
        newPlayer.level = 1;
        newPlayer.currentExp = 0;
        newPlayer.kill = 0;
        newPlayer.score = 0;
        newPlayer.selectPlayer = 0;
        newPlayer.idItemWeapon = -1;
        newPlayer.isCave = false;
        return newPlayer;
    }
}
