using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] Text stageNameText;
    [SerializeField] Text stageRewardText;
    [SerializeField] Text enemyNumberText;
    [SerializeField] Text enemyRewardText;
    [SerializeField] Text totalLossText;
    [SerializeField] Text totalRewardText;
    private int enemyNumber;
    private int stageReward;
    private int enemyReward;
    private int totalLoss;
    private int totalReward;
    private float scoreReward;
    private float scoreEnemy;
    private float scoreLoss;
    private float scoreTotal;
    private int scoreSpeed = 1000;
    private bool viewScorefinish = false;
    private float viewButtonTimer = 0;
    [SerializeField] GameObject sceneChangeButton;
    private void Start()
    {
        sceneChangeButton.SetActive(false);
        stageNameText.text = GameManager.StageScoreData.StageName;
        enemyNumber = GameManager.StageScoreData.EnemyNumber;
        stageRewardText.text = "" + 0;
        enemyNumberText.text = "" + enemyNumber;
        enemyRewardText.text = "" + 0;
        totalLossText.text = "" + 0;
        totalRewardText.text = "" + 0;
        enemyReward = GameManager.StageScoreData.EnemyReward;
        stageReward = GameManager.StageScoreData.StageReward;
        totalLoss = GameManager.StageScoreData.TotalLoss;
        totalReward = stageReward + enemyReward - totalLoss;
    }

    private void Update()
    {
        if (!viewScorefinish)
        {
            if (scoreReward < stageReward)
            {
                scoreReward += scoreSpeed * Time.deltaTime;
                if (scoreReward > stageReward)
                {
                    scoreReward = stageReward;
                }
                stageRewardText.text = "" + Mathf.Floor(scoreReward);
            }
            else if (scoreEnemy < enemyReward)
            {
                scoreEnemy += scoreSpeed * Time.deltaTime;
                if (scoreEnemy > enemyReward)
                {
                    scoreEnemy = enemyReward;
                }
                enemyRewardText.text = "" + Mathf.Floor(scoreEnemy);
            }
            else if (scoreLoss < totalLoss)
            {
                scoreLoss += scoreSpeed * Time.deltaTime;
                if (scoreLoss > totalLoss)
                {
                    scoreLoss = totalLoss;
                }
                totalLossText.text = "" + Mathf.Floor(-scoreLoss);
            }
            else if (scoreTotal < totalReward)
            {
                scoreTotal += scoreSpeed * Time.deltaTime;
                if (scoreTotal > totalReward)
                {
                    scoreTotal = totalReward;
                }
                totalRewardText.text = "" + Mathf.Floor(scoreTotal);
            }
            else if(!viewScorefinish)
            {
                viewScorefinish = true;
            }
        }
        else
        {
            if (viewButtonTimer < 5)
            {
                viewButtonTimer += Time.deltaTime;
                if (viewButtonTimer > 5)
                {
                    sceneChangeButton.SetActive(true);
                }
            }
        }
    }

    public void OnClickSceneChange()
    {
        if (viewScorefinish)
        {
            GameManager.Instance.SceneChange(3);
        }
    }
}
