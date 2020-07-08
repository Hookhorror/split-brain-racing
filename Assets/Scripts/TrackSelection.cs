using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class TrackSelection : MonoBehaviour
{
    public static TrackSelection Instance;

    public TextMeshProUGUI highscoreList;


    void Awake()
    {
        if (Instance == null)
        {
            // DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }


    public void ShowResults(string trackTag)
    {
        // Record[] records = RecordManager.Instance.GetRecords(trackTag);
        // Debug.Log("Loading records for " + trackTag);
        // if (records == null || records.Length == 0)
        //     return;

        // Debug.Log("Records found");
        // highscoreList.SetText("koira");
        // StringBuilder sb = new StringBuilder();
        // for (int i = 0; i < records.Length; i++)
        // {
        //     sb.Append(i + 1);
        //     sb.Append(". ");
        //     sb.Append(records[i].name);
        //     sb.Append(" - ");
        //     sb.Append(records[i].finalTime());
        //     sb.Append("\n");

        // }
        // highscoreList.color = Color.cyan;

        highscoreList.SetText(RecordManager.Instance.TopResultAsText(trackTag));
    }


    public void LoadTrack(string trackTag)
    {
        Debug.Log("Loading " + trackTag + " scene");
        SceneManager.LoadScene(trackTag);
    }


    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
