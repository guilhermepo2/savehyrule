#define VISUALIZE_ASTAR_STEPS

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameManager : MonoBehaviour {
	/*
	 * Função de Avaliação do Algoritmo A*:
	 * f(n) = g(n) + h(n)
	 * 
	 * f(n) = custo estimado total
	 * g(n) = custo gasto até n
	 * h(n) = heurística de n
	 */


	/*
	 * Classe Tile que contem a posição do Tile no mapa (Vector2)
	 * - contém o valor de heurística (g(n) + h(n))
	 * - contém o valor do custo do caminho percorrido até chegar naquele nodo
	 * - contém uma referência ao nodo pai, o nodo que o originou na busca, afim de facilitar o backtracking
	 */

	public class Tile
	{
		private Vector2 position;
		private int f_value;
		private int g_value;
		Tile parent;

		public Tile(Vector2 position, int f_value, int g_value, Tile parent)
		{
			this.position = position;
			this.f_value = f_value;
			this.g_value = g_value;
			this.parent = parent;
		}

		public Vector2 getPosition() { return this.position; }
		public int getF() { return this.f_value; }
		public Tile getParent() { return this.parent; }
		public int getG() { return this.g_value; }
	}


	// armazenar os custos dos caminhos
	private int custoCaminho = 0;
	private int custoAcumulado = 0;
	// tempo de delay para mostrar os passos da execucao do A*
	private float delayTime = 0f;

	// Pilha que armazena a posição dos objetivos do agente
	List<Vector2> positionStack = new List<Vector2>();

	// Variável que segurará a posição a ser desempilhada
	Vector2 objective = Vector2.zero;

	// Lista de movimentos para o Agente se locomover após realizar a busca
	List<char> movements = new List<char>();

	// Booleana para verificar se o agente está ocupado ou não
	private bool agentBusy = false;

	// Listas para a execução do Algoritmo A*
	// Evaluation Queue -> Lista de nodos que ainda serão avaliados
	// Closed Evaluations -> Lista de nodos que já foram avaliadas
	List<Tile> evaluationQueue = new List<Tile>();
	List<Tile> closeEvaluations = new List<Tile> ();

	// Referência ao agente inteligente
	private GameObject player;

	// ===================================================================
	// Implementação do Padrão Singleton
	public static GameManager instance = null;


	public static GameManager getInstance()
	{
		return instance;
	}


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (this);
	}

	// Fim de implementação do padrão Singleton
	// ===================================================================


	// Funções referentes à manipulação da pilha de objetivos
	// push -> coloca uma novo posição no inicio da pilha
	// pop -> remove o primeiro elemento da pilha e o retorna
	public void push(Vector2 v)
	{
		positionStack.Insert (0, v);	
	}

	public Vector2 pop()
	{
		Vector2 v = positionStack [0];
		positionStack.RemoveAt (0);
		return v;
	}


	/*
	 * 
	 * Função que resolve o problema do caixeiro viajante utilizando
	 * algoritmo Hillclimbing
	 * 
	 * O primeiro candidato é a própria lista dos nodos
	 * A troca de posição se dá da seguinte forma:
	 * É selecionado o ultimo nodo, a cada iteração ele troca com o de sua esquerda
	 * Caso ele seja o primeiro nodo do vetor, o último atual é selecionado
	 * Esse processo se repete até que o último atual seja igual ao primeiro último
	 */

	public void travelSalesman(Vector2 starting_point, List<Vector2> nodes)
	{
		Debug.Log ("Executando o Travel Salesman!");
		if (nodes.Count == 1)
			return;
			//cabo

		// O primeiro candidato vai ser a sequência 0, ..., n dos nodos
		List<int> solution = new List<int>();
		List<int> candidate = new List<int>();
		for (int i = 0; i < nodes.Count; i++) {
			candidate.Add (i);
			solution.Add (i);
		}

		// avalia a função e faz trocas de posições
		float evaluation = 0;
		// holds the value of the first chosen
		int first_chosen = candidate[0];
		// chose the index of the first chosen
		int actual_chosen = candidate.Count - 1;

		// calcula o valor da iteracao atual
		evaluation += Vector2.Distance(starting_point, nodes[solution[0]]);
		for (int i = 0; i < candidate.Count - 1; i++) {
			evaluation += Vector2.Distance (nodes [solution [i]], nodes [solution [i + 1]]);
		}
		evaluation += Vector2.Distance(nodes[solution[solution.Count - 1]], starting_point);

		// troca o atual para a esquerda
		if (actual_chosen > 0) {
			int aux = candidate [actual_chosen];
			candidate [actual_chosen] = candidate [actual_chosen - 1];
			candidate [actual_chosen - 1] = aux;
			actual_chosen--;
		} else
			actual_chosen = candidate.Count - 1;
		
		// comeca o loop
		float new_evaluation;
		while (first_chosen != candidate [actual_chosen]) {
			Debug.Log ("Loop do Travel Salesman!");
			new_evaluation = 0;

			evaluation += Vector2.Distance(starting_point, nodes[candidate[0]]);
			for (int i = 0; i < candidate.Count - 1; i++) {
				evaluation += Vector2.Distance (nodes [solution [i]], nodes [candidate [i + 1]]);
			}
			evaluation += Vector2.Distance(nodes[candidate[solution.Count - 1]], starting_point);

			if (new_evaluation < evaluation) {
				evaluation = new_evaluation;
				for (int i = 0; i < candidate.Count; i++) {
					solution [i] = candidate [i];
				}
			}

			if (actual_chosen > 0) {
				int aux = candidate [actual_chosen];
				candidate [actual_chosen] = candidate [actual_chosen - 1];
				candidate [actual_chosen - 1] = aux;
				actual_chosen--;
			} else
				actual_chosen = candidate.Count - 1;

		}

		// aqui devemos ter a solucao no vetor de solution
		// agora so empilhar o starting point e os nodos na ordem
		for (int i = 0; i < solution.Count; i++) {
			this.push (nodes[solution[i]]);
		}
	}

	// Função Start() do Unity, executada ao iniciar a execução do programa
	void Start()
	{
		// Obtendo a Referência do Agente
		player = GameObject.Find ("HyruleMapManager").GetComponent<HyruleMapManager> ().getPlayerReference ();


		// Comentários auxiliares durante a implementação

		// 1. add to stack -> lost woods, dungeons 1, 2, 3 and house, dungeons are the travelling salesman problem
		// 2. on the begginig the stack will be: lostWoords | link's house | dungeon | dungeon | dungeon
		// 3. remove first on the stack and A* to it
		// 4. when reach dungeon, will go inside it, add to stack -> exit | get pendant
		// 5. go to 3.

		// sample execution:
		// Init stack.     -> stack: lost woods | house | dungeon a | dungeon b | dungeon c
		// go to dungeon c -> stack: lost woods | house | dungeon a | dungeon b | exit dungeon c | get pendant c
		// get pendant c   -> stack: lost woods | house | dungeon a | dungeon b | exit dungeon c
		// exit dungeon c  -> stack: lost woods | house | dungeon a | dungeon b
		// go to dungeon b -> stack: lost woods | house | dungeon a | exit dungeon b | get pendant b
		// get pendant b   -> stack: lost woods | house | dungeon a | exit dungeon b
		// exit dungeon b  -> stack: lost woods | house | dungeon a
		// go to dungeon a -> stack: lost woods | house | exit dungeon a | get pendant a
		// get pendant a   -> stack: lost woods | house | exit dungeon a 
		// exit dungeon a  -> stack: lost woods | house 
		// go to house     -> stack: lost woods 
		// go to lost woods
		// the end

		// for each iteration -> remove objective from stack, A* until reach it, when accomplished, get next objective

		/*   ALGORITHM:
		 * while(objectivesStack is not empty)
		 * 
		 *   if(!objective)
		 * 		objective = objectivesStack.pop();
		 * 
		 * 	while(!reached_object)
		 * 		calculate_path_to_objective_using_A*()
		 * 		execute_commands_and_go_to_objective()
		 * 		delete(objective)
		 * 
		 * 
		 * 
		 * calculate_path_to_objective_using_A*():
		 *    calculate_heuristics_on_map()
		 * 
		 *    evaluated_node = current_node
		 * 	  while(evaluting_node != goal_node)
		 *        foreach(neighbour_tiles)
		 *            select_nodes_to_evaluate()
		 *            get(f(n))
		 *            push them into the queue()
		 *            evaluating_node = queueNext
		 *    
		 */

		// Função que dará inicio aos planejamentos e ações do agente
		saveHyrule ();
	}

	/*
	 * 
	 * Função de inserir movimentos na lista de movimentos do agente
	 * 
	 * Recebe o nodo objetivo que foi atingido através da busca
	 * 
	 * Através da informação do nodo e de seu pai é possivel obter o movimento que o agente
	 * deve realizar para ir do nodo pai para o filho.
	 * 
	 * É executado um laço percorrendo os pais dos nodos até que seja encontrada a raiz (pai nulo)
	 */
	private void insertMovement(Tile goal)
	{
		//Debug.Log ("found goal!");

		// temos o objetivo, agora podemos atualizar os custos dos caminhos!
		custoCaminho = goal.getG();
		custoAcumulado = custoAcumulado + custoCaminho;
		UserInterface.getInstance ().setCaminhoParcial (custoCaminho);
		UserInterface.getInstance ().setCaminhoTotal (custoAcumulado);

		Tile p = goal;
		Vector2 pos;

		while (p.getParent () != null) {

			// Marca na interface que essa posição faz parte do caminho final
			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markPath ((int)p.getPosition().x, (int)p.getPosition().y);
			#endif

			// obtém a diferença entre a posição atual e do pai
			pos = new Vector2 (p.getPosition().x - p.getParent().getPosition().x,
				p.getPosition().y - p.getParent().getPosition().y);

			//Debug.Log ("p position: " + p.getPosition());
			//Debug.Log ("p parent position: " + p.getParent ().getPosition ());
			//Debug.Log ("pos: " + pos);

			// de acordo com a posição obtida calcula o movimento que o agente terá que fazer
			// e empilha esse movimento
			if (pos == Vector2.up) {
				//Debug.Log ("up: " + pos);
				movements.Insert (0, 'R');
			} else if (pos == Vector2.down) {
				//Debug.Log ("down: " + pos);
				movements.Insert (0, 'L');
			} else if (pos == Vector2.left) {
				//Debug.Log ("left: " + pos);
				movements.Insert (0, 'U');
			} else if (pos == Vector2.right) {
				//Debug.Log ("right:" + pos);
				movements.Insert (0, 'D');
			}

			p = p.getParent ();
			
		}
	}



	/*
	 * 
	 * Bubble Sort para ordenar os tiles por ordem de valor de f(n)
	 * Atualmente não utilizado
	 * 
	 */

	private void queueBubbleSort()
	{
		Tile aux;

		for (int i = 0; i < evaluationQueue.Count; i++) {
			for (int j = 0; j < evaluationQueue.Count; j++) {
				if (evaluationQueue [i].getF () < evaluationQueue [j].getF ()) {
					aux = evaluationQueue [i];
					evaluationQueue [i] = evaluationQueue [j];
					evaluationQueue [j] = aux;
				}
			}
		}

	}


	/*
	 * 
	 * Função para inserir na Fila
	 * A intenção era originalmente fazer uma inserção ordenada
	 * A função ainda remanesce para que talvez essa alteração possa ser feita no futuro
	 * 
	 */
	private void insertOnQueueList(Tile t)
	{
		evaluationQueue.Add (t);
	}


	/*
	 * 
	 * Função para expandir nodo, recebe um nodo.
	 * 
	 * A função recebe um nodo e coloca na fila os 4 nodos adjacentes, cada um com
	 * seus custos de g(n) e f(n)
	 * 
	 * Nessa função, os nodos expandidos são marcados como visitados e são mostrados na interface
	 * na lista de proximos a serem expandidos
	 * 
	 */
	private void expandNode(Tile node) {

		//Debug.Log ("expanding node");
		Vector2 pos;
		int h;
		int g_value;


		// obtendo a posição do nodo resultado da expansão para cima
		pos = node.getPosition() + Vector2.up;
		// obtendo o valor de g(n) para esse nodo
		g_value = node.getG () + (HyruleMapManager.getInstance ().getCostForTile ((int)pos.x, (int)pos.y));
		// calculando o valor de h(n)
		h = (g_value +
			(HyruleMapManager.getInstance().getHeuristicValue((int)pos.x, (int)pos.y)));
		
		// Verificando se o nodo é possível de ser visitado (h >= 0) e se ele não foi visitado
		if (h >= 0 && HyruleMapManager.getInstance ().visit ((int)pos.x, (int)pos.y)) {
			this.insertOnQueueList (new Tile (pos, h, g_value, node));

			// Marca na interface gráfica que o nodo foi aberto para expansão
			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markOpen ((int)pos.x, (int)pos.y);
			#endif
		}

		// obtendo a posição do nodo resultado da expansão para baixo
		pos = node.getPosition () + Vector2.down;
		g_value = node.getG () + (HyruleMapManager.getInstance ().getCostForTile ((int)pos.x, (int)pos.y));
		h = (g_value +
			(HyruleMapManager.getInstance().getHeuristicValue((int)pos.x, (int)pos.y)));
		if (h >= 0 && HyruleMapManager.getInstance ().visit ((int)pos.x, (int)pos.y)) {
			this.insertOnQueueList (new Tile (pos, h, g_value, node));

			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markOpen ((int)pos.x, (int)pos.y);
			#endif
		}

		// obtendo a posição do nodo resultado da expansão para a esquerda
		pos = node.getPosition () + Vector2.left;
		g_value = node.getG () + (HyruleMapManager.getInstance ().getCostForTile ((int)pos.x, (int)pos.y));
		h = (g_value +
			(HyruleMapManager.getInstance().getHeuristicValue((int)pos.x, (int)pos.y)));
		if (h >= 0 && HyruleMapManager.getInstance ().visit ((int)pos.x, (int)pos.y)) {
			this.insertOnQueueList (new Tile (pos, h, g_value, node));

			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markOpen ((int)pos.x, (int)pos.y);
			#endif
		}

		// obtendo a posição do nodo resultado da expansão para a direita
		pos = node.getPosition () + Vector2.right;
		g_value = node.getG () + (HyruleMapManager.getInstance ().getCostForTile ((int)pos.x, (int)pos.y));
		h = (g_value +
			(HyruleMapManager.getInstance().getHeuristicValue((int)pos.x, (int)pos.y)));
		if (h >= 0 && HyruleMapManager.getInstance ().visit ((int)pos.x, (int)pos.y)) {
			this.insertOnQueueList (new Tile (pos, h, g_value, node));

			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markOpen ((int)pos.x, (int)pos.y);
			#endif
		}
	}


	/*
	 * Aqui tem o inicio o Algoritmo de Busca Heurística A*
	 * 
	 * Algoritmo:
	 * 
	 * calcula os valores de heurística no mapa para o objetivo
	 * marca todos as posições do mapa como não visitadas
	 * 
	 * coloca a posição inicial do agente na fila
	 * escolhe para expandir o nodo com menor valor de f(n) = g(n) + h(n)
	 * expande o nodo, adicionando os resultados na fila
	 * repete essas ações até encontrar um nodo que corresponde ao objetivo
	 * 
	 */
	private IEnumerator astar_search(Vector2 goal)
	{
		//Debug.Log ("goal: " + goal);

		// Calculando as Heurísticas no Mapa
		HyruleMapManager.getInstance().setHeuristicBoard((int)goal.x, (int)goal.y);
		// Marcando todas as posições do mapa como não visitadas
		HyruleMapManager.getInstance ().unvisit ();
	
		//Debug.Log("player current position: " + player.transform.position);

		// Caso haja alguma posição resquiciosa de outras execuções limpa a fila
		if (evaluationQueue.Count > 0)
			evaluationQueue.Clear ();

		// Colocando posição atual na fila de avaliação
		evaluationQueue.Add (new Tile(player.transform.position, 0, 0, null));

		Tile evaluating;

		// Escolhe o nodo a ser expandido de menor valor f(n)
		// Remove-o da fila de nodos a serem avaliados e adiciona na lista de nodos avaliados
		evaluating = evaluationQueue [0];
		for (int i = 1; i < evaluationQueue.Count; i++) {
			if (evaluating.getF () > evaluationQueue [i].getF ()) {
				evaluating = evaluationQueue [i];
			}
		}
		evaluationQueue.Remove (evaluating);
		//Debug.Log("evaluating position " + evaluating.getPosition());
		closeEvaluations.Add (evaluating);

		// Marca na interface gráfica que o nodo foi expandido
		#if VISUALIZE_ASTAR_STEPS
		HyruleMapManager.getInstance ().markClosed ((int)evaluating.getPosition().x, (int)evaluating.getPosition().y );
		#endif

		// Faz a expansão do nodo
		expandNode (evaluating);

		#if VISUALIZE_ASTAR_STEPS
		yield return new WaitForSeconds (delayTime);
		#endif

		// Prepara o próximo nodo para ser avaliado pelo mesmo próximo feito anteriormente
		evaluating = evaluationQueue [0];
		for (int i = 1; i < evaluationQueue.Count; i++) {
			if (evaluating.getF () > evaluationQueue [i].getF ()) {
				evaluating = evaluationQueue [i];
			}
		}
		evaluationQueue.Remove (evaluating);
		closeEvaluations.Add (evaluating);

		#if VISUALIZE_ASTAR_STEPS
		HyruleMapManager.getInstance ().markClosed ((int)evaluating.getPosition().x, (int)evaluating.getPosition().y );
		#endif

		// Executa essas ações já descritas acima enquanto houverem elementos a serem avaliados
		while (evaluationQueue.Count > 0) {
			expandNode (evaluating);
			#if VISUALIZE_ASTAR_STEPS
			yield return new WaitForSeconds (delayTime);
			#endif

			// Caso o nodo atual seja o objetivo
			if (evaluating.getPosition () == goal) {
				// São calculados os movimentos gerados pelo algoritmo
				insertMovement (evaluating);
				// O Agente então se movimenta
				StartCoroutine (move_to (movements));
				#if VISUALIZE_ASTAR_STEPS
				return true;
				#else
				yield return new WaitForSeconds(delayTime);
				#endif
			}

			evaluating = evaluationQueue [0];
			for (int i = 1; i < evaluationQueue.Count; i++) {
				if (evaluating.getF () > evaluationQueue [i].getF ()) {
					evaluating = evaluationQueue [i];
				}
			}
			evaluationQueue.Remove (evaluating);
			closeEvaluations.Add (evaluating);
			#if VISUALIZE_ASTAR_STEPS
			HyruleMapManager.getInstance ().markClosed ((int)evaluating.getPosition().x, (int)evaluating.getPosition().y );
			#endif
		}
	}

	/*
	 * Função que movimenta o agente, recebe uma lista de movimentos
	 * 
	 * Para cada movemento da lista de movimentos, o agente efetivamente realiza o movimento
	 * É a função que transforma todo o planejamento em ação
	 * 
	 */
	private IEnumerator move_to(List<char> moveSequence)
	{
		//Debug.Log ("movimentos: " + moveSequence.Count);
		for (int i = 0; i < moveSequence.Count; i++) {
			switch (moveSequence [i]) {
			case 'U':
				player.GetComponent<Player> ().goUp ();
				break;
			case 'D':
				player.GetComponent<Player> ().goDown ();
				break;
			case 'L':
				player.GetComponent<Player> ().goLeft ();
				break;
			case 'R':
				player.GetComponent<Player> ().goRight ();
				break;
			}

			UserInterface.getInstance ().setActualTileCost (HyruleMapManager.getInstance().getCostForTile((int)player.transform.position.x, (int)player.transform.position.y));
			yield return new WaitForSeconds (0.1f);
		}

		//yield return new WaitForSeconds (1.0f);
		movements.Clear();

		string objective = HyruleMapManager.getInstance ().reachedObjectiveAction ((int)player.transform.position.x, (int)player.transform.position.y);

		if (objective == "pendant0" || objective == "pendant1" || objective == "pendant2")
			player.GetComponent<Player>().gotPendant ();

		agentBusy = false;
		saveHyrule ();
	}


	/*
	 * Função SaveHyrule()
	 * 
	 * É onde se dará o inicio de todas as ações do Agente.
	 * 
	 * A função verifica se há alguma tarefa a ser realizada, verificando a  pilha de objetivos,
	 * também verifica se o agente não está executando outra tarefa
	 * 
	 * Caso haja uma tarefa a ser executada e o agente não esteja ocupado, o agente passa a estar ocupado
	 * verifica o próximo objetivo e inicia a busca pelo melhor caminho para chegar até o seu objetivo.
	 * 
	 * Caso contrário, ele cumpriu todos seus objetivos e provavelmente está pronto para salvar a princesa Zelda.
	 */

	public void saveHyrule()
	{
		//Debug.Log ("SAVING HYRULE: " + player);

		if (positionStack.Count > 0 && !agentBusy) {
			agentBusy = true;
			//Debug.Log ("popping objective");
			objective = this.pop ();
			StartCoroutine (astar_search (objective));
		} else {
			player.GetComponent<Player>().gotPendant ();
		}
	}

	void Update () {
		//Debug.Log ("Movements: " + movements.Count);
		//Debug.Log ("Pilha de Objetivo: " + positionStack.Count);
		//Debug.Log ("Fila do A*: " + evaluationQueue.Count);
	}
}
