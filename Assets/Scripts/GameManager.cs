using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int MaxNumberOfShots = 3;

    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;

    private int _usedNumberOfShots;
    private IconHandler _iconHandler;
    private List<Baddie> _baddies = new List<Baddie>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _iconHandler = FindObjectOfType<IconHandler>();
        if (_iconHandler == null)
        {
            Debug.LogError("IconHandler not found in the scene.");
        }

        _slingShotHandler = FindObjectOfType<SlingShotHandler>();
        if (_slingShotHandler == null)
        {
            Debug.LogError("SlingShotHandler not found in the scene.");
        }

        Baddie[] baddiesArray = FindObjectsOfType<Baddie>();
        for (int i = 0; i < baddiesArray.Length; i++)
        {
            _baddies.Add(baddiesArray[i]);
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);
        CheckForLastShots();
    }

    public bool HasEnoughShots()
    {
        return _usedNumberOfShots < MaxNumberOfShots;
    }

    public void CheckForLastShots()
    {
        if (_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);
        if (_baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    private void CheckForAllDeadBaddies()
    {
        if (_baddies.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose

    private void WinGame()
    {
        if (_restartScreenObject != null)    
        {
            _restartScreenObject.SetActive(true);
            _slingShotHandler.enabled = false;
        }
        else
        {
            Debug.LogError("Restart screen object is not assigned in the GameManager.");
        }
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        _usedNumberOfShots = 0;
        _baddies.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}