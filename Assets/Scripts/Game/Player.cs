using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public TMP_Text _score;
    
    public float moveSpeed = 600f;
    
    private float movement = 0;

    private void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, movement * Time.fixedDeltaTime * -moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var scoreValue = 0;
        var scoreText = "score:";
        var tableID = 94328;
        
        if (col.CompareTag("score"))
        {
            scoreValue = scoreValue + 1;
            scoreText = scoreText + scoreValue;
            _score.text =  scoreText;
            
            GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, null, (bool success) => {
                Debug.Log($"Score Add {(success ? "Successful" : "Failed")}.");
            });
        }else if(col.CompareTag("collider")){
            SceneManager.LoadScene("MainMenu");
        }
    }
}