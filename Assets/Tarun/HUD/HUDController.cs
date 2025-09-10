
using DLearners;
using TMPro;

public class HUDController : Singleton<HUDController>
{

    public TextMeshProUGUI TEXM_instruction;
    public TextMeshProUGUI TEXM_instruction2;
    public TextMeshProUGUI pointsText;//TEX_points
    public TextMeshProUGUI TEX_questionCount;
    public TextMeshProUGUI cashPoints;//TM_pointFx;

    private int addpoints;
    private int removepoints;
    private int removepoints1;
    private int removepoints2;

    public int score { get; private set; }
    public void Init()
    {
        pointsText.text = "";
    }
    
    public void UpdateScore(bool isAdd)
    {
        string cash = "";
        int cashScore = 0;
        if(isAdd)
        {
            cash = "+" + addpoints + " points";
            cashScore += addpoints;
        }
        else
        {
            cash = "-" + removepoints + " points";
            cashScore -= addpoints;
        }


        cashPoints.text = cash;


        score += cashScore;
        pointsText.text = score.ToString();
    }
}
