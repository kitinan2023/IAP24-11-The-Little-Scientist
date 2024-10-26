using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text timeText;
    [SerializeField] Image timeImage;
    [SerializeField] float duration , currentTime;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        currentTime = duration;
        timeText.text = currentTime.ToString();
        StartCoroutine(TimeIEN());
    }
    IEnumerator TimeIEN()
    {
        while(currentTime >= 0)
        {
            timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
            timeText.text = currentTime.ToString();

            yield return new WaitForSeconds(1f);
            currentTime--;

        }
        OpenPanel();
    }

    // Update is called once per frame
    void OpenPanel()
    {
        timeText.text = "";
        panel.SetActive(true);
    }
}
