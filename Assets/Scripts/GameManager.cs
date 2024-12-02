using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Button backMenu;
    public Button playAgain;
    public Button btnNext;
    public Canvas nextMap;
    public GameObject menuFinish;
    public TextMeshProUGUI score;
    public TextMeshProUGUI kill;
    public Image iconP;
    public Sprite iconResume;
    public Sprite iconPause;

    public TextMeshProUGUI hideHealthBarText;


    public bool isPaused = false;
    public bool isHide;
    private bool isHideHealthBarEnemy;

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        isHideHealthBarEnemy = false;
        isHide = false;
    }
    public void ClickButton()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Forest":
            case "Cave":
                GameObject lstBoss = GameObject.Find("EnemySkeletonBoss");
                if (lstBoss != null) return;
                nextMap.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E) && nextMap.enabled &&
                    Vector2.Distance(player.transform.position, nextMap.transform.position) < 3f)
                {
                    FillInf();
                }
                break;
        }
    }

    public void FillInf()
    {
        menuFinish.gameObject.SetActive(true);
        score.text = "Score: " + FileReadWrite.Instance.player.score;
        kill.text = "Kill: " + FileReadWrite.Instance.player.kill;
    }

    public void Die()
    {
        FillInf();
        btnNext.gameObject.SetActive(false);
        backMenu.gameObject.SetActive(true);
        playAgain.gameObject.SetActive(true);
    }

    public void HideHealthBarEnemy()
    {
        if (isPaused) return;
        if (!isHide)
        {
            isHide = true;
            hideHealthBarText.text = "Off";
        }
        else
        {
            isHide = false;
            hideHealthBarText.text = "On";
        }
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        isHideHealthBarEnemy = !isHideHealthBarEnemy;
        foreach (GameObject enemy in enemys)
        {
            Transform healthBar = enemy.transform.Find("HealthEnemy");
            if (isHideHealthBarEnemy)
            {
                healthBar.gameObject.SetActive(false);
            }
            else
            {
                healthBar.gameObject.SetActive(true);
            }
        }
    }

    private void PauseGame()
    {
        iconP.sprite = iconResume;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        iconP.sprite = iconPause;
        Time.timeScale = 1f;
    }
}
