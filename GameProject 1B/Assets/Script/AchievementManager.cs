using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float displayTime = 3f;

    private int totalCollected;

    private bool firstItemUnlocked;
    private bool collectorUnlocked;
    private bool healUnlocked;
    private bool dropUnlocked;
    private bool QuestUnlocked;

    private Coroutine popupRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (achievementPanel != null) achievementPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemCollected(int amount)
    {
        totalCollected += amount;

        if (firstItemUnlocked == false)
        {
            firstItemUnlocked = true;
            Unlock("첫 수집", "처음으로 아이템을 획득했습니다. ");
        }

        if (collectorUnlocked == false && totalCollected >= 5)
        {
            collectorUnlocked = true;
            Unlock("초보 수집가", "아이템을 총 5개 획득했습니다.");
        }

    }

    public void ItemUsed(string itemName)
    {
        if (healUnlocked) return;

        if (itemName == "물약" || itemName == "회복약" || itemName == "구급키트")
        {
            healUnlocked = true;
            Unlock("건강이 최고", "회복 아이템을 처음 사용 했습니다.");
        }
    }

    public void ItemDropped()
    {
        if (dropUnlocked) return;

        dropUnlocked = true;

        Unlock("과감한 정리", "아이템을 처음 버렸습니다.");
    }

    public void QuestCompleted()
    {
        if (QuestUnlocked) return;

        QuestUnlocked = true;
        Unlock("퀘스트 해결사", "퀘스트를 처음 완료했습니다.");
    }

    private void Unlock(string title, string description)
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            achievementPanel.SetActive(false);
        }

        popupRoutine = StartCoroutine(ShowPopup(title, description));
    }   

    private IEnumerator ShowPopup(string title, string description)
    {
        if (achievementPanel == null || titleText == null || descriptionText == null)
        {
            Debug.LogWarning("AchievementManager 의 UI 가 연결되지 않았습니다. ");
            yield break;
        }

        achievementPanel.SetActive(true);
        titleText.text = "업적 달성!" + title;
        descriptionText.text = description;

        yield return new WaitForSeconds(displayTime);

        achievementPanel.SetActive(false);
        popupRoutine = null;
    }

}
