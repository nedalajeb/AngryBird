using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotHandler : MonoBehaviour
{
    [Header("LineRenderers")]
    [SerializeField] private LineRenderer _LeftLineRenderer;
    [SerializeField] private LineRenderer _RightLineRender;

    [Header("Transform References")]
    [SerializeField] private Transform _LeftStartPosition;
    [SerializeField] private Transform _RightStartPosition;

    [SerializeField] private float _maxDistance = 3.5f;

    [Header("SlingShot Stats")]
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBetweenBirdRespawns = 2f;


    [Header("Scripts")]

    [SerializeField] private SlingShotArea _slingShotArea;


    [Header("Bird")]

    [SerializeField] private AngieBird _angieBirdPrefab;
    [SerializeField] private float _angieBirdPositionOffset = 2f;

    private Vector2 _slingShotLinePosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;
    private bool _birdOnSlingshot;

    private AngieBird _spawnedAngieBird;
    

    private void Awake()
    {
        _LeftLineRenderer.enabled = false;
        _RightLineRender.enabled = false;

        SpawnAngieBird(); 


    }

    private void Start()
    {
        if (_LeftLineRenderer == null || _RightLineRender == null)
        {
            Debug.LogError("Please assign the LineRenderer components in the Inspector.");
        }

        if (_LeftStartPosition == null || _RightStartPosition == null || _centerPosition == null || _idlePosition == null)
        {
            Debug.LogError("Please assign the start positions and center position in the Inspector.");
        }
    }

    private void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed && _slingShotArea.IsWithinSlingshotArea())
        {
            _clickedWithinArea = true;
        }

        if (InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingshot)
        {
            DrawSlingShot();

            PositionAndRotateAngieBird();
        }

        if (InputManager.WasRightMouseButtonReleased && _birdOnSlingshot && GameManager.Instance.HasEnoughShots())
        {
            _clickedWithinArea = false;

            // Launch the bird
            _spawnedAngieBird.LaunchBird(_direction, _shotForce);

            // Use a shot in the game manager
            GameManager.Instance.UseShot();

            // Update the state
            _birdOnSlingshot = false;

            // Set the lines to the center position
            SetLines(_centerPosition.position);

            // Check if there are enough shots left to spawn another bird
            if (GameManager.Instance.HasEnoughShots())
            {
                StartCoroutine(SpawnAngieBirdAfterTime());
            }
        }
    }


    #region SlingShot Methods


    private void DrawSlingShot()
    {
      

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        touchPosition.z = 0;

        _slingShotLinePosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);
        
        SetLines(_slingShotLinePosition);

        _direction = (Vector2)_centerPosition.position - _slingShotLinePosition;
        _directionNormalized = _direction.normalized;


          


    }

    private void SetLines(Vector2 position)
    {
        if (!_LeftLineRenderer.enabled && !_RightLineRender.enabled)
        {
            _LeftLineRenderer.enabled = true;
            _RightLineRender.enabled = true;
        }



        _LeftLineRenderer.SetPosition(0, _LeftStartPosition.position);
        _LeftLineRenderer.SetPosition(1, position);

        _RightLineRender.SetPosition(0, _RightStartPosition.position);
        _RightLineRender.SetPosition(1, position);
    }



    #endregion


    #region Angie Bird Methods



    private void SpawnAngieBird()

    {
        SetLines(_idlePosition.position);

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + dir * _angieBirdPositionOffset;

        _spawnedAngieBird = Instantiate(_angieBirdPrefab, spawnPosition, Quaternion.identity);
        _spawnedAngieBird.transform.right = dir;
        _birdOnSlingshot = true;


    }
     private  void  PositionAndRotateAngieBird()
    {

        _spawnedAngieBird.transform.position = _slingShotLinePosition + _directionNormalized * _angieBirdPositionOffset;

        _spawnedAngieBird.transform.right = _directionNormalized;

    }

    private IEnumerator SpawnAngieBirdAfterTime()
    {

        yield return new WaitForSeconds (_timeBetweenBirdRespawns);

        SpawnAngieBird(); 



    }


    #endregion

}//_spawnedAngieBird = Instantiate(_angieBirdPrefab, spawnPosition, Quaternion.identity);