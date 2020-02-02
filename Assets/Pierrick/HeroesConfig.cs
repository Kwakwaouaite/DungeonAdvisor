using UnityEngine;

[CreateAssetMenu(fileName = "HeroesConfig", menuName = "Dungeon Advisor/HeroesConfig", order = 1)]
public class HeroesConfig : ScriptableObject
{
    public float speedCellPerSecond = 0.5f;

    public int m_MaxWaitingStep = 5;
    public float m_MaxWaitingTimes = 1.0f;

    public int MaxHappiness = 100;
    public int MinHappiness = 0;
    public int StartHappiness = 50;

    public int HappinessUsingObject = 10;
    public int HappinessObjectAlreadyUsed = -10;
    public int HappinessObjectiveComplete = 50;
    public int HappinessCannotLeave = -50;

}
