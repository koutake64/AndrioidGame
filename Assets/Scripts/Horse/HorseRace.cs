using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorseRace : MonoBehaviour
{
    public List<Transform> horses;
    public Transform finishLine;
    public float raceDuration = 10f; // ���[�X���ԁi�b�j
    private List<float> horseSpeeds;

    private void Start()
    {
        StartRace();
    }

    private void StartRace()
    {
        horseSpeeds = new List<float>();
        foreach (Transform horse in horses)
        {
            float speed = Random.Range(0.1f, 0.5f);
            horseSpeeds.Add(speed);
        }
        StartCoroutine(RunRace());
    }

    private IEnumerator RunRace()
    {
        float elapsedTime = 0f;
        while (elapsedTime < raceDuration)
        {
            for (int i = 0; i < horses.Count; i++)
            {
                horses[i].Translate(Vector3.forward * horseSpeeds[i] * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DetermineWinner();
    }

    private void DetermineWinner()
    {
        // �S�[�����C���܂ł̋�������ԒZ���n�����҂Ƃ���
        Transform winner = null;
        float minDistance = float.MaxValue;
        foreach (Transform horse in horses)
        {
            float distance = Vector3.Distance(horse.position, finishLine.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                winner = horse;
            }
        }
        Debug.Log("Winner is: " + winner.name);
    }
}
