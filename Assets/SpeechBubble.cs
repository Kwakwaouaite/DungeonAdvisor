using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] GameObject m_Happy;
    [SerializeField] GameObject m_Med;
    [SerializeField] GameObject m_Sad;
    [SerializeField] GameObject m_Timer1;
    [SerializeField] GameObject m_Timer2;
    [SerializeField] GameObject m_Timer3;
    [SerializeField] GameObject m_Timer4;
    [SerializeField] GameObject m_Timer5;

    private GameObject[] reactionGameObjects;

    // We have to force manually the similarity of the enum and reactionGameObjects
    public enum EReactionType
    {
        Happy,
        Medium,
        Sad,
        Timer1,
        Timer2,
        Timer3,
        Timer4,
        Timer5,
        Count
    }

    private void Awake()
    {
        reactionGameObjects = new GameObject[] { m_Happy, m_Med, m_Sad, m_Timer1, m_Timer2, m_Timer3, m_Timer4, m_Timer5 } ;
    }

    public IEnumerator SaySomething(EReactionType message, float duration = 0.5f)
    {
        for (int i = 0; i < (int) EReactionType.Count; i++)
        {
          reactionGameObjects[i].SetActive(i == (int)message);
        }

        yield return new WaitForSeconds(duration);

        reactionGameObjects[(int)message].SetActive(false);
    }
}
