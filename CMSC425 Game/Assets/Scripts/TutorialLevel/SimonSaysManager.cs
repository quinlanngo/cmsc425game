using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimonSaysManager : MonoBehaviour
{
    public TargetBehavior[] targets;
    public int sequenceLength = 4;
    public float delayBetweenLights = 0.7f;

    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private int puzzlesCompleted = 0;
    private bool isPlayerTurn = false;
    private bool sequenceStarted = false;
    private HashSet<int> hitTargets = new HashSet<int>(); // Track unique hits
    public List<EnemySpawner> enemySpawner;

    void Start()
    {
        // Sequence now starts on interaction instead of game start
    }

    public void StartSequence()
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            GenerateSequence();
            StartCoroutine(DisplaySequence());
        }
    }

    void GenerateSequence()
    {
        sequence.Clear();
        List<int> availableIndices = new List<int>();

        // Populate the list with target indices
        for (int i = 0; i < targets.Length; i++)
        {
            availableIndices.Add(i);
        }

        // Randomly shuffle the indices
        while (availableIndices.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            sequence.Add(availableIndices[randomIndex]);
            availableIndices.RemoveAt(randomIndex); // Ensure non-repeating by removing the chosen index
        }
    }

    IEnumerator DisplaySequence()
    {
        isPlayerTurn = false;
        yield return new WaitForSeconds(1f);

        foreach (int index in sequence)
        {
            targets[index].LightUp();
            yield return new WaitForSeconds(delayBetweenLights);
        }

        isPlayerTurn = true;
        playerIndex = 0;
        hitTargets.Clear(); // Reset hit targets for the new round
    }

    private void lightUpAll()
    {
        foreach (TargetBehavior target in targets)
        {
            target.LightUp();
        }
    }

    public void RegisterPlayerChoice(int targetIndex)
    {
        Debug.Log("Registering Player's Choice: " + targetIndex);
        if (!isPlayerTurn) return;

        // Only proceed if this target hasn't been hit in this round
        if (!hitTargets.Contains(targetIndex))
        {
            hitTargets.Add(targetIndex); // Add target to hit list

            if (sequence[playerIndex] == targetIndex)
            {
                playerIndex++;

                // Check if player has completed the current sequence
                if (playerIndex >= sequence.Count)
                {
                    puzzlesCompleted++;
                    Debug.Log("Puzzle completed! Total puzzles solved: " + puzzlesCompleted);

                    // Check if player has solved at least 3 puzzles
                    if (puzzlesCompleted >= 3)
                    {
                        Debug.Log("Tutorial completed!");
                        lightUpAll();
                        DeployEnemies();
                    }
                    else
                    {
                        // Show a new puzzle sequence
                        lightUpAll();
                        GenerateSequence();
                        StartCoroutine(DisplaySequence());
                    }
                }
            }
            else
            {
                // If wrong target hit, reset sequence without affecting puzzlesCompleted
                Debug.Log("Incorrect hit, restarting sequence display.");
                StartCoroutine(DisplaySequence());
                playerIndex = 0;
            }
        }
        else
        {
            Debug.Log("Target already hit in this sequence. Ignoring duplicate.");
        }
    }

    private void DeployEnemies()
    {
        foreach (EnemySpawner enemySpawner in enemySpawner)
        {
            if (enemySpawner != null)
            {
                enemySpawner.SpawnEnemies(); // Spawn enemies at specified points
                Debug.Log("Enemies deployed at all spawn points!");
            }
            else
            {
                Debug.LogWarning("EnemySpawner is not assigned.");
            }
        }
        SaveLoadController.Instance.SaveAtCheckpoint("Simon");
    }
}
