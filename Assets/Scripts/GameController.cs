using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private int score;
	private int scoreLimit;

	public GameObject[] collectables;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI restartText;

	public float timeStopSpeed = 0.01f;

	void Awake()
    {
		score = 0;
		collectables = GameObject.FindGameObjectsWithTag( "Collectible" );
		scoreLimit = collectables.Length;
    }

	public void LoadSaveData( SaveData saveData )
	{
		for( int i = 0; i < saveData.collectableIDs.Length; i++ )
			if( saveData.collectableIDs[ i ] == 0 )
				Destroy( collectables[ i ] );
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
