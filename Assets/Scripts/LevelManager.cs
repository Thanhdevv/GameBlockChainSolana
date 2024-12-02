using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Button btnPlayForest;
    public Button btnPlayCave;
    public List<Button> btnSelectPlayers = new List<Button>();

    public Health health;

    public Image level10;
    public Image level20;
    public Image level35;
    public List<Image> useWeapons = new List<Image>();

    public Sprite bgrActive;
    public Sprite bgrInactive;

    public bool isForest;
    public bool isCave;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isCave = FileReadWrite.Instance.player.isCave;
        isForest = true;
        if (btnPlayForest == null || btnPlayCave == null) return;
        ActiveButton(btnPlayForest, isForest);
        ActiveButton(btnPlayCave, isCave);
        SelectPlayer(FileReadWrite.Instance.player.selectPlayer);
        useWeapons.Add(level10);
        useWeapons.Add(level20);
        useWeapons.Add(level35);

    }


    public void FillExp()
    {
        int level = FileReadWrite.Instance.player.level;
        health.UpdateHealth(level, 40);
        if (FileReadWrite.Instance.player.idItemWeapon != -1)
        {

        }
        if (level >= 35)
        {
            level10.sprite = bgrActive;
            level20.sprite = bgrActive;
            level35.sprite = bgrActive;
            level10.gameObject.GetComponent<Button>().interactable = true;
            level20.gameObject.GetComponent<Button>().interactable = true;
            level35.gameObject.GetComponent<Button>().interactable = true;
        }
        else if (level >= 20)
        {
            level10.sprite = bgrActive;
            level20.sprite = bgrActive;
            level35.sprite = bgrInactive;
            level10.gameObject.GetComponent<Button>().interactable = true;
            level20.gameObject.GetComponent<Button>().interactable = true;
            level35.gameObject.GetComponent<Button>().interactable = false;
        }
        else if (level >= 10)
        {
            level10.sprite = bgrActive;
            level20.sprite = bgrInactive;
            level35.sprite = bgrInactive;
            level10.gameObject.GetComponent<Button>().interactable = true;
            level20.gameObject.GetComponent<Button>().interactable = false;
            level35.gameObject.GetComponent<Button>().interactable = false;
        } else
        {
            level10.sprite = bgrInactive;
            level20.sprite = bgrInactive;
            level35.sprite = bgrInactive;
            level10.gameObject.GetComponent<Button>().interactable = false;
            level20.gameObject.GetComponent<Button>().interactable = false;
            level35.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void UseWeapon(int index)
    {
        for (int i = 0; i < useWeapons.Count; i++)
        {
            Image item = useWeapons[i];
            Transform weapon = item.transform.Find("Weapon").Find("Image");
            if (i == index)
            {
                weapon.gameObject.SetActive(true);
                item.gameObject.GetComponent<Button>().interactable = false;
                FileReadWrite.Instance.UpdateItemWeapon(i);
            } else
            {
                weapon.gameObject.SetActive(false);
                item.gameObject.GetComponent<Button>().interactable = true;
            }
        }
        FillExp();
    }

    public void SelectPlayer(int player)
    {
        FileReadWrite.Instance.UpdateSelectPlayer(player);
        for (int i = 0; i < btnSelectPlayers.Count; i++)
        {
            btnSelectPlayers[i].interactable = i == player ? false:true;
        }
    }
    private void ActiveButton(Button btn, bool check)
    {
        if (check)
        {
            btn.interactable = true;
        } else
        {
            btn.interactable = false;
        }
    }

    public void LoadForest()
    {
        //if (!isForest) return;
        LoadScene("Forest");
    }

    public void LoadCave()
    {
        //if (!isCave) return;
        LoadScene("Cave");
    }

    public void LoadMenu()
    {
        LoadScene("Menu");
    }

    private void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        FileReadWrite.Instance.WriteFile();
    }
}
