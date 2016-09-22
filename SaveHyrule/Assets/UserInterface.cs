using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	public Text caminhoParcial;
	public Text caminhoTotal;

	public static UserInterface instance = null;

	public static UserInterface getInstance()
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

	public void setCaminhoParcial(int valor)
	{
		caminhoParcial.text = ("g(n) = " + valor);
	}

	public void setCaminhoTotal(int valor)
	{
		caminhoTotal.text = ("g(n) total = " + valor);
	}
}
