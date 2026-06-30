using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialPanel : MonoBehaviour
{
    public static TutorialPanel instance;

    [Header("Referencias UI")]
    public GameObject panel;
    public Image imageDisplay;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject closeButton;

    private Sprite[] currentImages;
    private int currentIndex;
    private string currentKey;
    private readonly HashSet<string> seen = new HashSet<string>();

    void Awake()
    {
        instance = this;
    }

    public void ShowAgain(string key, Sprite[] images)
    {
        if (images == null || images.Length == 0) return;
        currentKey = key;
        currentImages = images;
        currentIndex = 0;
        Show();
    }

    public void TryShow(string key, Sprite[] images)
    {
        if (images == null || images.Length == 0) return;
        if (seen.Contains(key)) return;

        currentKey = key;
        currentImages = images;
        currentIndex = 0;
        Show();
    }

    void Show()
    {
        UIBlocker.Open();
        panel.SetActive(true);
        UpdateDisplay();
    }

    public void Next()
    {
        if (currentIndex < currentImages.Length - 1)
        {
            currentIndex++;
            UpdateDisplay();
        }
        else
        {
            Close();
        }
    }

    public void Back()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateDisplay();
        }
    }

    public void Close()
    {
        seen.Add(currentKey);
        UIBlocker.Close();
        panel.SetActive(false);
    }

    void UpdateDisplay()
    {
        if (imageDisplay != null)
            imageDisplay.sprite = currentImages[currentIndex];

        bool isFirst = currentIndex == 0;
        bool isLast = currentIndex == currentImages.Length - 1;

        if (backButton != null) backButton.SetActive(!isFirst);
        if (nextButton != null) nextButton.SetActive(!isLast);
        if (closeButton != null) closeButton.SetActive(isLast);
    }
}
