using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private int score;
	private int scoreLimit;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI restartText;

	public float timeStopSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
		score = 0;
		scoreLimit = GameObject.FindGameObjectsWithTag( "Collectible" ).Length;
    }

    // Update is called once per frame
    void Update()
    {
        score = scoreLimit - GameObject.FindGameObjectsWithTag( "Collectible" ).Length;

		if( score != scoreLimit )
			scoreText.text = "Score: " + score + "/" + scoreLimit;
		else
		{
			scoreText.text = "YOU WIN!";

			if( Time.timeScale != 0 )
			{
				Time.timeScale = Mathf.MoveTowards( Time.timeScale, 0, timeStopSpeed );
			}
			else
			{
				restartText.gameObject.SetActive( true );

				if( Input.GetKeyDown( KeyCode.R ) )
				{
					Time.timeScale = 1;
		
					if( SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1 )
						SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
					else
						SceneManager.LoadScene( 0 );
				}
			}
		}
	}
}
