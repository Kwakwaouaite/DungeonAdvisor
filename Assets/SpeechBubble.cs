using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] GameObject m_Happy;
    [SerializeField] GameObject m_Med;
    [SerializeField] GameObject m_Sad;

    private GameObject[] reactionGameObjects;

    // We have to force manually the similarity of the enum and reactionGameObjects
    public enum EReactionType
    {
        Happy,
        Medium,
        Sad,
        Count
    }

    private void Awake()
    {
        reactionGameObjects = new GameObject[] { m_Happy, m_Med, m_Sad} ;
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
