using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;
    public Image aniLevelUp; 
    public Health health;
    public TextMeshProUGUI level;

    public int currentLevel;
    public int maxExp;
    public int currentExp;
    public int experienceForLevel1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentLevel = FileReadWrite.Instance.player.level;
        currentExp = FileReadWrite.Instance.player.currentExp;
        maxExp = CalculateExperienceForNextLevel(currentLevel);
        UpdateExp(0);
        level.text = "Lv: " + currentLevel;
    }

    public void UpdateExp(int exp)
    {
        currentExp += exp;
        health.UpdateHealth(currentExp, maxExp);
        if (currentExp >= maxExp) {
            LevelUp();
        } else
        {
            Debug.Log(currentExp);
            FileReadWrite.Instance.UpdateCurrentExp(currentExp);
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        level.text = "Lv: " + currentLevel;
        aniLevelUp.gameObject.SetActive(true);
        currentExp-=maxExp;
        maxExp = CalculateExperienceForNextLevel(currentLevel);
        health.UpdateHealth(currentExp, maxExp);
        FileReadWrite.Instance.UpdateLevel(currentLevel);
        FileReadWrite.Instance.UpdateCurrentExp(currentExp);
        AudioManager.Instance.PlaySFXLevelUp();
    }

    public int CalculateExperienceForNextLevel(int currentLevel)
    {
        return currentLevel * experienceForLevel1;
    }

    public void CloseAniLevelUp()
    {
        aniLevelUp.gameObject.SetActive(false);
    }
}
