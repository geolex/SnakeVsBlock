using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


// Each Stage is 50m  so 5s of Gameplay
[System.Serializable]
struct Stage
{
	// Bonuses per stage
	public int minBonus;
	public int maxBonus;

	// Value per Bonus
	public int minBonusValue;
	public int maxBonusValue;

	// Blocks per Wall
	public int minBlocks;
	public int maxBlocks;

	// Resistance per Block
	public int minResistance;
	public int maxResistance;

	// Pipes (Vertical Walls) per Wall
	public int minPipes;
	public int maxPipes;
}

[RequireComponent(typeof(Carriage))]
public class LevelGenerator : MonoBehaviour
{
	[Header("Level Pattern")]
	[SerializeField] float m_stageLength;
	[SerializeField] int m_stageWidth = 5;
	[SerializeField] List<Stage> m_stages;
	[SerializeField] bool m_isRepeating;

	[SerializeField] int m_nbWalls = 10;


	[Header("Prefabs")]
	[SerializeField] GameObject m_finishPrefab;
	[SerializeField] GameObject m_pipePrefab;
	[SerializeField] GameObject m_blockPrefab;

	int currentIndex = 0;
	Carriage m_carriage;

	private void Awake()
	{
		m_carriage = GetComponent<Carriage>();

		GenerateStageAtIndex(0);
	}

	// Start is called before the first frame update
	void Start()
    {
		
    }


    // Update is called once per frame
    void Update()
    {
		if(currentIndex != Mathf.RoundToInt(transform.position.y / m_stageLength)){
			currentIndex = Mathf.RoundToInt(transform.position.y / m_stageLength);
			GenerateStageAtIndex(currentIndex);
		}
    }


	void GenerateStageAtIndex(int _index)
	{
		Stage nextStage;

		if (m_isRepeating)
		{
			nextStage = m_stages[_index % m_stages.Count];
		}
		else if(_index >= m_stages.Count)
		{
			SpawnEnd(_index * m_stageLength);
			return;
		}
		else
		{
			nextStage = m_stages[_index];
		}

		GameObject currentStage = GenerateStage(nextStage);

		currentStage.transform.localPosition = new Vector3(0, _index * m_stageLength + 5f);

		Destroy(currentStage, 2 * (m_stageLength / m_carriage.GetScrollSpeed()));
	}

	
	GameObject GenerateStage(Stage _stage)
	{
		GameObject stageObject = new GameObject();

		int nbBonuses = Random.Range(_stage.minBonus, _stage.maxBonus);

		for (int wallIndex = 0; wallIndex < m_nbWalls; ++wallIndex){

			GameObject currentWall = CreateWall(_stage.minResistance, _stage.maxResistance, _stage.minBlocks, _stage.maxBlocks);


			// Pipes Positions are Random and might Overlap
			int nbPipes = Random.Range(_stage.minPipes, _stage.maxPipes);
			for (int pipeIndex = 0; pipeIndex < nbPipes; ++pipeIndex)
			{
				GameObject currentPipe = Instantiate(m_pipePrefab, currentWall.transform);
				currentPipe.transform.localPosition = new Vector3(-(m_stageWidth/2) + Random.Range(0, m_stageWidth-1) + 0.5f, (m_stageLength / m_nbWalls) / 2f);
			}

			currentWall.transform.parent = stageObject.transform;
			currentWall.transform.localPosition = new Vector2(0, (wallIndex / (float)m_nbWalls) * m_stageLength);
		}

		return stageObject;
	}




	// Block Positions are Random and might Overlap
	GameObject CreateWall(int _minResistance, int _maxResistance, int _minBlocks, int _maxBlocks)
	{
		GameObject wallObject = new GameObject();

		int nbBlocks = Random.Range(_minBlocks, _maxBlocks);
		for(int blockIndex = 0; blockIndex < nbBlocks; ++blockIndex)
		{
			GameObject currentBlock = Instantiate(m_blockPrefab, wallObject.transform);

			currentBlock.GetComponent<Block>().SetResistance(Random.Range(_minResistance, _maxResistance));

			currentBlock.transform.localPosition = new Vector3(-(m_stageWidth / 2) + Random.Range(0, m_stageWidth), 0);
		}

		return wallObject;
	}



	void SpawnEnd(float _posY)
	{
		Instantiate(m_finishPrefab, new Vector3(0, _posY), Quaternion.identity);
	}
}
