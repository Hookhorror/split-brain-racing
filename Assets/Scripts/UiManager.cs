using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI chasingInfo;
    public TextMeshProUGUI splitTime;
    public TextMeshProUGUI playerCount;
    public Transform playerInfoBox;
    public GameObject results;
    public GameObject scoreList;
    public GameObject goldMedal;
    public GameObject silverMedal;
    public GameObject bronzeMedal;
    public GameObject countdown;
    public Animator animator;
    private TrackController trackController;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        countdown.SetActive(false);
    }


    private void Start()
    {
        trackController = GameObject.FindGameObjectWithTag("Track")
                .GetComponent<TrackController>();

        goldMedal.SetActive(false);
        silverMedal.SetActive(false);
        bronzeMedal.SetActive(false);
        results.SetActive(false);
    }

    public void SetCurrentTime(float time)
    {
        currentTime.text = (string.Format("{0:0.00}", time));
    }


    public void SetSplitTime(float time)
    {
        splitTime.color = (time < 0) ? Color.green : Color.red;
        splitTime.text = (string.Format("{0:+0.00;-0.00}", time));
    }


    public void ResetTimes()
    {
        SetCurrentTime(0);
        splitTime.ClearMesh();
    }


    public void SetPlayerCount(int count)
    {
        playerCount.text = count.ToString();
    }


    public void SetChasingText(int medalLevel)
    {
        string text = "";
        switch (medalLevel)
        {
            case 0:
                text = "CHASING RECORD";
                chasingInfo.color = new Color(0.3f, 0.4f, 0.6f, 1.3f);
                break;
            case 1:
                text = "CHASING GOLD";
                chasingInfo.color = new Color(1.0f, 0.89f, 0.0f, 1.0f);
                break;
            case 2:
                text = "CHASING SILVER";
                chasingInfo.color = new Color(0.91f, 0.91f, 0.91f, 1.0f);
                break;
            default:
                text = "CHASING BRONZE";
                chasingInfo.color = new Color(0.7f, 0.55f, 0.34f, 1.0f);
                break;
        }

        chasingInfo.SetText(text);
    }


    public void ShowResultScreen(string TrackTag, int placement, int medal)
    {
        string scores = RecordManager.Instance.TopResultAsText(TrackTag, placement);
        results.SetActive(true);
        if (medal == 1)
            goldMedal.SetActive(true);
        if (medal == 2)
            silverMedal.SetActive(true);
        if (medal == 3)
            bronzeMedal.SetActive(true);
        scoreList.GetComponent<TextMeshProUGUI>().SetText(scores);
    }


    public void HideResultScreen()
    {
        results.SetActive(false);
        goldMedal.SetActive(false);
        silverMedal.SetActive(false);
        bronzeMedal.SetActive(false);

    }


    public void HidePlayerCount()
    {
        // playerCount.enabled = false;
        playerInfoBox.gameObject.SetActive(false);
    }


    public void PlayCountdown()
    {
        countdown.SetActive(true);
        Invoke("DisableCountdown", 4);
        // animator.Play("Countdown_Countdown");
    }


    private void DisableCountdown()
    {
        countdown.SetActive(false);
    }


    public void BackToTrackSelection()
    {
        SceneManager.LoadScene("TrackSelection");
    }


    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }


    public void ResumeGame()
    {
        Debug.Log("Resuming");
        trackController.Pause();
    }
}