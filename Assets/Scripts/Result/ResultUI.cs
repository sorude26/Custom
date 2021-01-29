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
        //stageNameText.text = GameManager.Instance.GetStageName()
        //enemyNumber = GameManager.EnemyNumber;
        stageRewardText.text = "" + 0;
        enemyNumberText.text = "" + enemyNumber;
        enemyRewardText.text = "" + 0;
        totalLossText.text = "" + 0;
        totalRewardText.text = "" + 0;
        //enemyReward = GameManager.EnemyReward;
        //stageReward = GameManager.StageReward;
        //totalLoss = GameManager.TtalLoss;
        stageReward = 1000;
        enemyReward = 3000;
        totalLoss = 800;
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
                    viewScorefinish = true;
                }
                totalRewardText.text = "" + Mathf.Floor(scoreTotal);
            }
        }
        else
        {
            if (viewButtonTimer < 2)
            {
                viewButtonTimer += Time.deltaTime;
                if (viewButtonTimer > 2)
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
