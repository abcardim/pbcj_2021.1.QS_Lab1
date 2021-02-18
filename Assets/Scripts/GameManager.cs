using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int numTentativas;          // Armazena as tentativas válidas da rodada
    private int maxNumTentativas;       // Número máximo de tentativas para a Forca ( = suas 'vidas')
    int score = 0;

    public GameObject letra;            // prefab da letra no Game
    public GameObject centro;           // objeto de texto que indica o centro da tela 

    private string palavraOculta = "";  // palavra oculta a ser descoberta (usado no Lab1-parte A)
    private string[] palavrasOcultas = new string[] { "carro", "elefante", "futebol" }; // array de palavras ocultas possíveis

    private int tamanhoPalavraOculta;   // tamanho desta palavra oculta
    char[] letrasOcultas;               // letras da palavra oculta
    bool[] letrasDescobertas;           // indicador de quais letras foram descobertas

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        InitGame();
        InitLetras();
        numTentativas = 0;
        maxNumTentativas = 10;
        UpdateNumTentativas();
        UpdateScore();

        
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras()
    {
        int numLetras = tamanhoPalavraOculta;
        for (int i = 0; i < numLetras; i++)
        {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f)*80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject)Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);         // nomeia na hierarquia a GameObject com letra-(iésima + 1), i = 1...numLetras
            l.transform.SetParent(GameObject.Find("Canvas").transform);     // posiciona-se como filho do GameObject

        }
    }

    void InitGame()
    {
        // palavraOculta = "Elefante";                         // definição da palavra oculta a ser descoberta  (usado no Lab1-parte A)
        int numeroAleatorio = Random.Range(0, palavrasOcultas.Length); // sorteamos um número dentro do número de palavras do array
        palavraOculta = palavrasOcultas[numeroAleatorio];   // selecionamos uma palavra sorteada pra ser a oculta;
        tamanhoPalavraOculta = palavraOculta.Length;        // determinação do número de letras da palavra oculta
        palavraOculta = palavraOculta.ToUpper();            // transforma-se a palavra em maiúscula
        letrasOcultas = new char[tamanhoPalavraOculta];     // instanciamento do array char das letras da palavra
        letrasDescobertas = new bool[tamanhoPalavraOculta]; // instanciamento da array bool de indicacao de letras descobertas
        letrasOcultas = palavraOculta.ToCharArray();        // cópia da palavra para o array de chars (letras)

    }

    void checkTeclado()
    {
        if (Input.anyKeyDown)
        {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if(letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122)
            {
                numTentativas++;
                UpdateNumTentativas();
                if(numTentativas >= maxNumTentativas)
                {
                    SceneManager.LoadScene("Lab1_badEnding");
                }
                for(int i = 0; i <= tamanhoPalavraOculta; i++)
                {
                    if (!letrasDescobertas[i])
                    {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if(letrasOcultas[i] == letraTeclada)
                        {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                            score = PlayerPrefs.GetInt("score");
                            score++;
                            PlayerPrefs.SetInt("score", score);
                            UpdateScore();
                            verificaSePalavraDescoberta();
                        }
                    }
                }
            }
        }
    }

    void UpdateNumTentativas()
    {
        GameObject.Find("numTentativas").GetComponent<Text>().text = numTentativas + " / " + maxNumTentativas + " tentativas";
    }

    void UpdateScore()
    {
        GameObject.Find("scoreUI").GetComponent<Text>().text = "Pontuação: " + score;
    }

    void verificaSePalavraDescoberta()
    {
        bool condicao = true;
        for(int i = 0; i < tamanhoPalavraOculta; i++)
        {
            condicao = condicao && letrasDescobertas[i];
        }
        if (condicao)
        {
            PlayerPrefs.SetString("mensagemVitoria", "A palavra era: " + palavraOculta);
            SceneManager.LoadScene("Lab1_goodEnding");
        }
    }

}
