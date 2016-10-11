using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public Text caminhoParcial;
	public Text caminhoTotal;
	public Text debugMessage;
	public GameObject initialScreen;
	public GameObject endingScreen;
	private GameObject initialScreenReference;
	private GameObject endingScreenReference;

	private bool beggining;
	private bool ending;

	public static UserInterface instance = null;

	public static UserInterface getInstance()
	{
		return instance;
	}

	void Start()
	{
		beggining = true;
		ending = false;

		initialScreenReference = Instantiate (initialScreen) as GameObject;
	}

	void Update()
	{
		if(beggining)
		{
			StartCoroutine(begin ());
			if (Input.GetKeyDown (KeyCode.Return)) {
				beggining = false;
				Destroy (initialScreenReference);
				GameManager.getInstance ().saveHyrule ();
			}
		}

		if (ending) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				Destroy (endingScreenReference);
			}
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (this);

		debugMessage.text = "";
	}

	public void setActualTileCost(int value)
	{
		//debugMessage.text = ("Custo do Tile Atual: " + value);
	}

	public void setCaminhoParcial(int valor)
	{
		caminhoParcial.text = ("g(n) = " + valor);
	}

	public void setCaminhoTotal(int valor)
	{
		caminhoTotal.text = ("g(n) total = " + valor);
	}

	public Texture2D fadeOutTexture; //texture that will overlay the screen
	public float fadeSpeed = 0.8f;   // fade velocity

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;       // fade Direction
	// -1 -> black to clear
	// 1  -> into black screen

	void OnGUI()
	{
		if (alpha >= 0.7f)
			alpha = 0.7f;
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect(0,0, Screen.width, Screen.height), fadeOutTexture);
	}

	private float beginFade (int direction)
	{
		fadeDir = direction;
		return (fadeSpeed);	
	}

	public IEnumerator begin()
	{
		fadeSpeed = 0.05f;
		float fadeTime = beginFade (-1);
		yield return new WaitForSeconds (fadeTime);
	}

	public IEnumerator finished()
	{
		fadeSpeed = 0.05f;
		float fadeTime = beginFade (1);
		yield return new WaitForSeconds (fadeTime);
		endingScreenReference = Instantiate (endingScreen) as GameObject;
		ending = true;
	}
}
