using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    private Quest currentQuest;

    // Start is called once before the first execution of Update after the MonoBehavio
    void Start()
    {

    }

    public void talkToNPC(Quest npcQuest)
    {
        if (npcQuest.state == QuestState.Completed)
        {
            Debug.Log("이미 완료한 퀘스트 입니다.");
            return;
        }

        if (currentQuest == null || currentQuest.state == QuestState.Completed)
        {
            StartQuest(npcQuest);
            return;
        }

        if (currentQuest != npcQuest)
        {
            Debug.Log("이미 다른 퀘스트를 진행 중입니다.");
            return;
        }

        if (currentQuest.state == QuestState.Inprgress)
        {
            Debug.Log("아직 아이템이 부족합니다.");
        }
        else if (currentQuest.state == QuestState.ReadyToComplete)
        {
            CompleteQuest();
        }
    }

    private void StartQuest(Quest newQuest)
    {
        currentQuest = newQuest;
        currentQuest.currentAmount = 0;
        currentQuest.state = QuestState.Inprgress;

        Debug.Log(currentQuest.questName + "퀘스트 시작!");
        UpdateQuestText();
    }



    public void AddQuestItem(string itemName, int amount)
    {
        if (currentQuest == null) return;
        if (currentQuest.state != QuestState.Inprgress) return;
        if (currentQuest.targetItemName != itemName) return;

        currentQuest.currentAmount += amount;

        if (currentQuest.currentAmount >= currentQuest.targetAmount)
        {
            currentQuest.currentAmount = currentQuest.targetAmount;
            currentQuest.state = QuestState.ReadyToComplete;
        }

        UpdateQuestText();
    }

    public void CompleteQuest()
    {
        currentQuest.state = QuestState.Completed;

        Debug.Log(currentQuest.questName + "퀘스트 완료!");
        UpdateQuestText();
    }


    private void UpdateQuestText()
    {
        if (currentQuest.state == QuestState.Inprgress)
        {
            questText.text =
                currentQuest.questName + "\n" +
                currentQuest.targetItemName + " " +
                currentQuest.currentAmount + "/" +
                currentQuest.targetAmount;
        }
        else if (currentQuest.state == QuestState.ReadyToComplete)
        {
            questText.text = "수집 완료 ! \nNPC에게 돌아가세요";
        }
        else if (currentQuest.state == QuestState.Completed)
        {
            questText.text = currentQuest.questName + "\n퀘스트 완료";
        }
    }
}
