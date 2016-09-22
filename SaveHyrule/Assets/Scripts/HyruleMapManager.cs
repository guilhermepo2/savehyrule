using UnityEngine;
using System.Collections;

public class HyruleMapManager : MonoBehaviour {


	// Variáveis relacionadas ao mapa
	// Guarda a referência de cada mapa do mundo do problema
	private Map hyruleMap = new MainMap();
	private Map dungeon1Map = new Dungeon1();
	private Map dungeon2Map = new Dungeon2();
	private Map dungeon3Map = new Dungeon3();

	// Variável que implementa o padrão Strategy
	// as funções podem obter dados do mapa e tomar decisões baseadas nas informações sem
	// referenciar exatamente o mapa em que está
	public Map map = new MainMap();


	// Referente aos tiles do mapa
	// Vetor de tiles que serão instanciados, formando o mapa
	// vetores privados que guardam as referências aos tiles do mapa, aos tiles abertos
	// aos tiles fechados e aos tiles que formam o caminho
	[Header("Tiles Gameobjects")]
	public GameObject[] tiles;
	private GameObject tilesHolder;
	private GameObject openTilesHolder;
	private GameObject closedTilesHolder;
	private GameObject pathTilesHolder;

	// Tiles que marcam informações do A* visualmente
	[Header("A* Marcation Tiles")]
	public GameObject openTile;
	public GameObject closedTile;
	public GameObject pathTile;

	// Informações sobre os tipos de terreno e seus custos
	[Header("Tile Cost Information")]
	public char[] tileLetter;
	public int[] tileCost;

	/*
	 *  O agente sempre inicia a jornada na casa do Link (ponto onde está o Link no mapa [25, 28]).
	 *  Nesse caso, o ponto inicial pode ser entrado pela interface do Unity.
	 *  Ela foi salva como default para o valor do mapa (25, 28) de acordo com a figura e (24,27) no Unity.
	 */

	// Informações referentes ao Agente
	// Posição Inicial
	// Referência ao Agente a ser Criado
	// Referência ao Agente criado
	[Header("Player Information")]
	public Vector2 playerStartingPosition;
	private GameObject playerRef;
	public GameObject player;


	// pendant 0 will be on dungeon 0
	// pendant 1 will be on dungeon 1
	// and so on...
	// Informação sobre os pingentes
	// Suas posições nas dungeons, seus objetos
	// a referência aos pingentes e um vetor booleana indicando se cada pingente foi coletado
	[Header("Pendants Information and Positions\t")]
	public Vector2[] pendantsPositions;
	public GameObject[] pendants;
	private GameObject[] pendantsReference;
	private bool[] pendantsTaken;

	// Informações sobre as Dungeons e suas Posições
	[Header("Dungeon Information and Positions\t")]
	public Vector2[] dungeonPositions;
	public Vector2[] dungeonsExitPosition;
	private GameObject[] dungeonsExitReference;
	public GameObject[] dungeons;
	private GameObject[] dungeonsReference;

	// Informações sobre Lost Woods e sua Posição
	[Header("Lost Woods Information and Position")]
	public Vector2 lostWoodsPosition;
	public GameObject lostWoods;
	private GameObject lostWoodsReference;

	// Informações sobre a Master Sword e sua posição
	[Header("Master Sword Information and Position")]
	public Vector2 masterSwordPosition;
	public GameObject masterSword;
	private GameObject masterSwordReference;

	// ===================================================
	// Padrão Singleton

	public static HyruleMapManager instance;

	public static HyruleMapManager getInstance()
	{
		return instance;
	}
	// Fim Padrão Singleton
	// ===================================================


	/*
	 * 
	 * Considerando que um mapa de dungeon esteja ativo
	 * Essa função é chamada para fazer a renderização correta
	 * do mapa da dungeon na tela
	 * 
	 */
	void renderDungeon()
	{
		Destroy (tilesHolder);
		tilesHolder = new GameObject ("Tiles Holder");

		int index = 0;
		for (int x = (-1 * map.getCount () / 2);
			x < (map.getCount () / 2); x++) {
			for (int y = (-1 * map.getCount() / 2);
				y < (map.getCount () / 2); y++) {
				switch (map.getPos ((x + (map.getCount () / 2)), (y + (map.getCount () / 2)))) {
				case 'X':
					index = 2;
					break;
				case 'D':
					index = 3;
					break;
				}
				GameObject instance = Instantiate (tiles [index], new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				instance.transform.SetParent (tilesHolder.transform);
			}
		}
	}

	/*
	 * 
	 * Função Hyrule() que renderiza o mapa de Hyrule, recebendo como parâmetro de onde o agente veio
	 * Com essa informação o agente sabe sua posição exata no mapa
	 * 
	 */
	void Hyrule(int whereFrom)
	{
		if (tilesHolder)
			Destroy (tilesHolder);

		// destroy all references that can be on screen
		for (int i = 0; i < dungeonsExitReference.Length; i++)
			Destroy (dungeonsExitReference [i]);
		for (int i = 0; i < pendantsReference.Length; i++)
			Destroy (pendantsReference [i]);

		map = hyruleMap;
		map.setVisitedBoard ();

		tilesHolder = new GameObject ("Tiles Holder");

		int index = 0;
		for (int x = (-1 * map.getCount () / 2);
			x < (map.getCount () / 2); x++) {
			for (int y = (-1 * map.getCount() / 2);
				y < (map.getCount () / 2); y++) {
				switch (map.getPos ((x + (map.getCount () / 2)), (y + (map.getCount () / 2)))) {
				case 'F':
					index = 0;
					break;
				case 'M':
					index = 2;
					break;
				case 'S':
					index = 3;
					break;
				case 'G':
					index = 1;
					break;
				case 'W':
					index = 4;
					break;


				}
				GameObject instance = Instantiate (tiles [index], new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
				instance.transform.SetParent (tilesHolder.transform);
			}
		}

		for (int i = 0; i < dungeonPositions.Length; i++) {
			dungeonsReference[i] = Instantiate (dungeons[i], new Vector3 (dungeonPositions[i].y - (map.getCount() / 2),  dungeonPositions[i].x - (map.getCount()/2)  , -1), Quaternion.identity) as GameObject;
			dungeonsReference [i].transform.eulerAngles = new Vector3 (dungeonsReference[i].transform.eulerAngles.x, dungeonsReference[i].transform.eulerAngles.y, dungeonsReference[i].transform.eulerAngles.z + 90);
		}

		lostWoodsReference = Instantiate (lostWoods, new Vector3 (lostWoodsPosition.y - (map.getCount () / 2), lostWoodsPosition.x - (map.getCount () / 2), -1), Quaternion.identity) as GameObject;
		lostWoodsReference.transform.eulerAngles = new Vector3 (lostWoodsReference.transform.eulerAngles.x, lostWoodsReference.transform.eulerAngles.y, lostWoodsReference.transform.eulerAngles.z + 90);
		masterSwordReference = Instantiate (masterSword, new Vector3 (masterSwordPosition.y - (map.getCount() / 2),  masterSwordPosition.x - (map.getCount()/2), -1), Quaternion.identity) as GameObject;
		masterSwordReference.transform.eulerAngles = new Vector3 (masterSwordReference.transform.eulerAngles.x, masterSwordReference.transform.eulerAngles.y, masterSwordReference.transform.eulerAngles.z + 90);

		if(whereFrom >= 0)
			playerRef.transform.position = new Vector3 (dungeonPositions[whereFrom].y - (map.getCount() / 2) , dungeonPositions[whereFrom].x - (map.getCount() / 2), playerRef.transform.position.z);
	}

	/*
	 * 
	 * Estabelece o mapa atual como sendo o mapa da Dungeon 1
	 * Toma todas as medidas necessárias para a renderização correta do mapa na tela
	 * 
	 */
	void Dungeon1()
	{
		map = dungeon1Map;
		map.setVisitedBoard ();

		Destroy (masterSwordReference);
		Destroy (lostWoodsReference);
		for (int i = 0; i < dungeonsReference.Length; i++)
			Destroy (dungeonsReference [i]);

		// we have to leave the dungeon, right?
		GameManager.getInstance ().push(new Vector2(dungeonsExitPosition[0].y - map.getCount()/2, dungeonsExitPosition[0].x - map.getCount()/2));
		// we have to take the pendant, right?
		if (pendantsTaken [0] == false)
			GameManager.getInstance ().push (new Vector2(pendantsPositions[0].y - map.getCount()/2, pendantsPositions[0].x - map.getCount()/2));
		
		renderDungeon ();

		if (pendantsTaken [0] == false) {
			pendantsReference [0] = Instantiate (pendants [0], new Vector3 (pendantsPositions [0].y - (map.getCount () / 2), pendantsPositions [0].x - (map.getCount () / 2), -1), Quaternion.identity) as GameObject;
			pendantsReference [0].transform.eulerAngles = new Vector3 (pendantsReference [0].transform.eulerAngles.x, pendantsReference [0].transform.eulerAngles.y, pendantsReference [0].transform.eulerAngles.z + 90);
		}

		dungeonsExitReference[0] = Instantiate(dungeons[0], new Vector3(dungeonsExitPosition[0].y - (map.getCount() / 2), dungeonsExitPosition[0].x - (map.getCount() / 2), -1), Quaternion.identity) as GameObject;
		dungeonsExitReference[0].transform.eulerAngles = new Vector3 (dungeonsExitReference[0].transform.eulerAngles.x, dungeonsExitReference[0].transform.eulerAngles.y, dungeonsExitReference[0].transform.eulerAngles.z + 90);
		playerRef.transform.position = new Vector3 (dungeonsExitPosition[0].y- (map.getCount() / 2) , dungeonsExitPosition[0].x - (map.getCount() / 2) , playerRef.transform.position.z);
	}

	/*
	 * 
	 * Estabelece o mapa atual como sendo o mapa da Dungeon 2
	 * Toma todas as medidas necessárias para a renderização correta do mapa na tela
	 * 
	 */
	void Dungeon2()
	{
		map = dungeon2Map;
		map.setVisitedBoard ();

		Destroy (masterSwordReference);
		Destroy (lostWoodsReference);
		for (int i = 0; i < dungeonsReference.Length; i++)
			Destroy (dungeonsReference [i]);

		// we have to leave the dungeon, right?
		GameManager.getInstance ().push(new Vector2(dungeonsExitPosition[1].y - map.getCount()/2, dungeonsExitPosition[1].x - map.getCount()/2));
		// we have to take the pendant, right?
		if (pendantsTaken [1] == false)
			GameManager.getInstance ().push (new Vector2(pendantsPositions[1].y - map.getCount()/2, pendantsPositions[1].x - map.getCount()/2));
		

		renderDungeon ();

		if (pendantsTaken [1] == false) {
			pendantsReference [1] = Instantiate (pendants [1], new Vector3 (pendantsPositions [1].y - (map.getCount () / 2), pendantsPositions [1].x - (map.getCount () / 2), -1), Quaternion.identity) as GameObject;
			pendantsReference [1].transform.eulerAngles = new Vector3 (pendantsReference [1].transform.eulerAngles.x, pendantsReference [1].transform.eulerAngles.y, pendantsReference [1].transform.eulerAngles.z + 90);
		}

		dungeonsExitReference[1] = Instantiate(dungeons[1], new Vector3(dungeonsExitPosition[1].y - (map.getCount() / 2), dungeonsExitPosition[1].x - (map.getCount() / 2), -1), Quaternion.identity) as GameObject;
		dungeonsExitReference[1].transform.eulerAngles = new Vector3 (dungeonsExitReference[1].transform.eulerAngles.x, dungeonsExitReference[1].transform.eulerAngles.y, dungeonsExitReference[1].transform.eulerAngles.z + 90);
		playerRef.transform.position = new Vector3 (dungeonsExitPosition[1].y - (map.getCount() / 2) , dungeonsExitPosition[1].x - (map.getCount() / 2) , playerRef.transform.position.z);
	}

	/*
	 * 
	 * Estabelece o mapa atual como sendo o mapa da Dungeon 3
	 * Toma todas as medidas necessárias para a renderização correta do mapa na tela
	 * 
	 */
	void Dungeon3()
	{
		map = dungeon3Map;
		map.setVisitedBoard ();

		Destroy (masterSwordReference);
		Destroy (lostWoodsReference);
		for (int i = 0; i < dungeonsReference.Length; i++)
			Destroy (dungeonsReference [i]);

		// we have to leave the dungeon, right?
		GameManager.getInstance ().push(new Vector2(dungeonsExitPosition[2].y - map.getCount()/2, dungeonsExitPosition[2].x - map.getCount()/2));
		// we have to take the pendant, right?
		if (pendantsTaken [2] == false)
			GameManager.getInstance ().push (new Vector2(pendantsPositions[2].y - map.getCount()/2, pendantsPositions[2].x - map.getCount()/2));
		

		renderDungeon ();

		if (pendantsTaken [2] == false) {
			pendantsReference [2] = Instantiate (pendants [2], new Vector3 (pendantsPositions [2].y - (map.getCount () / 2), pendantsPositions [2].x - (map.getCount () / 2), -1), Quaternion.identity) as GameObject;
			pendantsReference [2].transform.eulerAngles = new Vector3 (pendantsReference [2].transform.eulerAngles.x, pendantsReference [2].transform.eulerAngles.y, pendantsReference [2].transform.eulerAngles.z + 90);
		}

		dungeonsExitReference[2] = Instantiate(dungeons[2], new Vector3(dungeonsExitPosition[2].y - (map.getCount() / 2), dungeonsExitPosition[2].x - (map.getCount() / 2), -1), Quaternion.identity) as GameObject;
		dungeonsExitReference[2].transform.eulerAngles = new Vector3 (dungeonsExitReference[2].transform.eulerAngles.x, dungeonsExitReference[2].transform.eulerAngles.y, dungeonsExitReference[2].transform.eulerAngles.z + 90);
		playerRef.transform.position = new Vector3 (dungeonsExitPosition[2].y - (map.getCount() / 2) , dungeonsExitPosition[2].x - (map.getCount() / 2) , playerRef.transform.position.z);
	}


	/*
	 * Função Awake() pertence ao escopo de funções do Unity
	 * A função Awake é executada antes da função Start()
	 */
	void Awake () 
	{
		// Referentes ao Padrão Singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (this);

		hyruleMap.createHeuristicBoard();
		dungeon1Map.createHeuristicBoard ();
		dungeon2Map.createHeuristicBoard ();
		dungeon3Map.createHeuristicBoard ();
		map.createHeuristicBoard ();

		// Criando os vetores de referências
		dungeonsReference = new GameObject[dungeons.Length];
		pendantsReference = new GameObject[pendants.Length];
		dungeonsExitReference = new GameObject[dungeonsExitPosition.Length];

		// Informando que nenhum pingente ainda foi coletado
		pendantsTaken = new bool[pendantsPositions.Length];
		for (int i = 0; i < pendantsTaken.Length; i++)
			pendantsTaken [i] = false;

		// Renderiza o mapa de Hyrule, indicando que é o inicio
		Hyrule (-1);

		// Adicionando Lost Wood à pilha de Objetivos, afinal, esse é o objetivo principal

		GameManager.getInstance ().push (new Vector2(lostWoodsPosition.y - (map.getCount()/2), lostWoodsPosition.x - (map.getCount()/2)));

		// Link deve voltar à sua casa antes de partir?
		//GameManager.getInstance ().push (new Vector2(playerStartingPosition.y - map.getCount() / 2, playerStartingPosition.x - map.getCount() / 2);

		/*
		 * 
		 * TO DO
		 * 
		 * Para encontrar a melhor ordem para pegar os pingentes é necessário 
		 * resolver o problema do caixeiro viajante (travelling salesman). 
		 * 
		 * Para isto considere que antes do agente chegar à entrada de Lost Woods ele deverá iniciar a 
		 * jornada na casa de Link e voltar a esta casa com todos os pingentes coletados. 
		 * 
		 * Resolva tal problema usando alguma das técnicas de busca especificadas na disciplina.
		 * 
		 * O caixeiro viajante é feito na posição das dungeons, a posição das dungeons são então empilhadas
		 * na pilha de objetivos
		 */


		GameManager.getInstance ().push (new Vector2(dungeonPositions [1].y - (map.getCount()/2), dungeonPositions[1].x - (map.getCount()/2)));
		GameManager.getInstance ().push (new Vector2(dungeonPositions [2].y - (map.getCount()/2), dungeonPositions[2].x - (map.getCount()/2)));
		GameManager.getInstance ().push (new Vector2(dungeonPositions [0].y - (map.getCount()/2), dungeonPositions[0].x - (map.getCount()/2)));

		// Instanciando o Agente e guardando sua referência
		playerRef = Instantiate (player, new Vector3 (playerStartingPosition.y - (map.getCount() / 2),  playerStartingPosition.x - (map.getCount()/2) , -9), Quaternion.identity) as GameObject;
		playerRef.transform.eulerAngles = new Vector3 (playerRef.transform.eulerAngles.x, playerRef.transform.eulerAngles.y, playerRef.transform.eulerAngles.z + 90);

		// Criando os Holders dos tiles referentes ao A*
		openTilesHolder = new GameObject ("Open Tiles Holder");
		closedTilesHolder = new GameObject ("Closed Tiles Holder");
		pathTilesHolder = new GameObject ("Path Tiles Holder");
	}

	// ========================================================
	// ========================================================
	// ========================================================
	//  			FUNÇÕES AUXILIARES
	// ========================================================
	// ========================================================
	// ========================================================

	// Retorna a Referência do Agente
	public GameObject getPlayerReference()
	{
		return this.playerRef;
	}

	/*
	 * 
	 * Quando o agente cumpre um objetivo, esse função é chamada
	 * para que ele possa ver no mapa qual exatamente é o objetivo atingido
	 * e possa reagir de acordo com ele
	 * 
	 */
	public string reachedObjectiveAction (int x, int y)
	{
		Vector2 position = new Vector2 (y + (map.getCount()/2), x + (map.getCount()/2));
		//Debug.Log ("player pos: " + position);
		//Debug.Log ("dungeon 0 pos: " + dungeonPositions[0]);

		if (position == dungeonPositions [0]) {
			Dungeon1 ();
			return "dungeon0";
		} else if (position == dungeonPositions [1]) {
			Dungeon2 ();
			return "dungeon1";
		} else if (position == dungeonPositions [2]) {
			Dungeon3 ();
			return "dungeon2";
		} else if (position == lostWoodsPosition) {
			if (pendantsTaken [0] == true && pendantsTaken [1] == true && pendantsTaken [2] == true) {
				/*
				 * 
				 * O agente sempre termina a sua jornada ao chegar à entrada de Lost Woods.
				 * 
				 */
				// O Agente chegou à Lost Woods e possui os três pingentes!
				// YOU WIN
				Destroy(closedTilesHolder);
				Destroy(pathTilesHolder);
				Destroy(openTilesHolder);
				Debug.Log("YOU SAVED HYRULE CONGRATULATIONS");
			}
			return "lostwoods";
		} else if (position == pendantsPositions [0]) {
			Destroy (pendantsReference [0]);
			pendantsTaken [0] = true;
			return "pendant0";
		} else if (position == pendantsPositions [1]) {
			Destroy (pendantsReference [1]);
			pendantsTaken [1] = true;
			return "pendant1";
		} else if (position == pendantsPositions [2]) {
			Destroy (pendantsReference [2]);
			pendantsTaken [2] = true;
			return "pendant2";
		} else if (position == dungeonsExitPosition [0]) {
			Hyrule (0);
			return "exit0";
		} else if (position == dungeonsExitPosition [1]) {
			Hyrule (1);
			return "exit1";
		} else if (position == dungeonsExitPosition [2]) {
			Hyrule (2);
			return "exit2";
		}

		return "error";
	}

	/*
	 * 
	 * Marca na posição (x,y) recebida que a posição está no caminho final
	 * 
	 */
	public void markPath(int x, int y)
	{
		GameObject instance = Instantiate (pathTile, new Vector3(x ,y, -4), Quaternion.identity) as GameObject;
		instance.transform.SetParent (pathTilesHolder.transform);
	}

	/*
	 * 
	 * Marca na posição (x,y) recebida que a posição é um nodo aberto para expansão
	 * 
	 */
	public void markOpen(int x, int y)
	{
		GameObject instance = Instantiate (openTile, new Vector3(x ,y, -3), Quaternion.identity) as GameObject;
		instance.transform.SetParent (openTilesHolder.transform);
	}

	/*
	 * 
	 * Marca na posição (x,y) recebida que a posição é um nodo já expandido
	 * 
	 */
	public void markClosed(int x, int y)
	{
		GameObject instance = Instantiate (closedTile, new Vector3(x ,y, -4), Quaternion.identity) as GameObject;
		instance.transform.SetParent (closedTilesHolder.transform);
	}

	/*
	 * 
	 * Coloca os valores de heuristica no mapa de acordo com a posição (x,y) recebida.
	 * 
	 */
	public void setHeuristicBoard(int x, int y)
	{
		Debug.Log ("settando heuristica para (" + x + ", " + y + ")");
		Debug.Log ("settando heuristica para (" + (x + map.getCount()/2) + ", " + (y + map.getCount()/2) + ")");
		map.setHeuristicValue ( (x + (map.getCount()/2)) , (y + (map.getCount()/2)) );
	}

	/*
	 * 
	 * Retorna o valor da heurística para a posição (x,y) do mapa.
	 * 
	 */
	public int getHeuristicValue(int x, int y)
	{
		return map.getH ((x + map.getCount()/2),(y + map.getCount() / 2));
	}

	/*
	 * 
	 * Visita o nodo (x,y), retorna verdadeiro se foi visitado, falso caso o nodo já tenha sido visitado
	 * 
	 */
	public bool visit(int x, int y)
	{
		return map.visit ((x + (map.getCount() / 2)), (y + (map.getCount() / 2)));
	}

	/*
	 * 
	 * Marca todos os nodos do mapa como não visitados
	 * Também destroi todas as informações referentes à execução do A*
	 * e prepara para a próxima execução
	 * 
	 */
	public void unvisit()
	{
		map.unvisit ();
		Destroy (pathTilesHolder);
		Destroy (openTilesHolder);
		Destroy (closedTilesHolder);
		openTilesHolder = new GameObject ("Path Tiles Holder");
		closedTilesHolder = new GameObject("Closed Tiles Holder");
		pathTilesHolder = new GameObject ("Path Tiles Holder");
	}

	/*
	 * 
	 * Obtém o custo de se movimentar para o tile da posição (x,y)
	 * 
	 */
	public int getCostForTile(int x, int y)
	{
		int index = 0;

		switch (map.getPos ((x + (map.getCount () / 2)), (y + (map.getCount () / 2)))) {
		case 'F':
			index = 0;
			break;
		case 'M':
			index = 2;
			break;
		case 'S':
			index = 3;
			break;
		case 'G':
			index = 1;
			break;
		case 'W':
			index = 4;
			break;
		case 'D':
			index = 5;
			break;
		case 'X':
			index = 6;
			break;
		case 'E':
			index = -1;
			break;
		}

		if (index == -1)
			return -1;
		
		return tileCost [index];
	}
}
