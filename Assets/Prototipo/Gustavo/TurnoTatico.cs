using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TurnoTatico : MonoBehaviour
{
    [Header("Configuração de Ações")]
    public int pontosDeAcao = 4;
    public int ganhoPorTurno = 2;
    public int maxPontos = 8;

    [Header("Movimentação")]
    public float distanciaMaxima = 5f;
    public float velocidade = 5f;
    private Vector3 posicaoInicialTurno;
    private CharacterController controller;

    public bool turnoPlayer = false;

    [HideInInspector] public bool CanMove = true;

    public Action OnTurnBegin;
    public Action OnTurnEnd;

    [Header("Turnos")]
    public int turnoAtual = 1;
    public int totalTurnos = 10;

    // Eventos para UI
    public event Action<int, int> OnPontosDeAcaoAtualizados; // (atuais, max)
    public event Action<int, int> OnTurnoIniciado; // (turnoAtual, totalTurnos)
    public event Action<int, int> OnTurnoTerminado; // (turnoAtual, totalTurnos)

    private bool iniciouJogo = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        IniciarTurno();
    }

    void Update()
    {
        if (turnoPlayer && CanMove)
        {
            // Input de movimento
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");
            Vector3 movimento = new Vector3(inputX * (-1f), 0, inputZ * (-1f));

            if (movimento.magnitude > 0.01f)
            {
                controller.Move(movimento.normalized * velocidade * Time.deltaTime);
            }

            // Testa se ainda está dentro da distância máxima do turno
            float distanciaPercorrida = Vector3.Distance(posicaoInicialTurno, transform.position);
            if (distanciaPercorrida > distanciaMaxima)
            {
                //Debug.Log("Distância máxima de movimentação atingida!");

                // Corrige para não ultrapassar o limite
                Vector3 direcao = (transform.position - posicaoInicialTurno).normalized;
                Vector3 posicaoCorreta = posicaoInicialTurno + direcao * distanciaMaxima;
                controller.enabled = false; // desativa momentaneamente para setar posição manualmente
                transform.position = posicaoCorreta;
                controller.enabled = true;
            }

            // Pular turno (Espaço)
            if (Input.GetKeyDown(KeyCode.Space) && turnoPlayer)
            {
                TerminarTurno();
            }
        }

        
    }

    public bool UsarHabilidade(int custo)
    {
        if (pontosDeAcao >= custo)
        {
            pontosDeAcao -= custo;
            Debug.Log("Habilidade usada. Pontos restantes: " + pontosDeAcao);
            // Aqui entra a logica da habilidade
            OnPontosDeAcaoAtualizados?.Invoke(pontosDeAcao, maxPontos);
            return true;
        }
        else
        {
            Debug.Log("Pontos insuficientes");
            return false;
        }
    }

    public void IniciarTurno()
    {
        turnoPlayer = true;
        posicaoInicialTurno = transform.position;

        OnTurnBegin?.Invoke();

        Debug.Log("Início do turno. Pontos de ação: " + pontosDeAcao);
        if (iniciouJogo)
        {
            // Avança contador de turnos a partir do segundo início
            turnoAtual = Mathf.Min(turnoAtual + 1, totalTurnos);
        }
        iniciouJogo = true;
        OnTurnoIniciado?.Invoke(turnoAtual, totalTurnos);
        OnPontosDeAcaoAtualizados?.Invoke(pontosDeAcao, maxPontos);
    }

    public void TerminarTurno()
    {
        turnoPlayer = false;

        // Recupera pontos de ação
        pontosDeAcao = Mathf.Min(pontosDeAcao + ganhoPorTurno, maxPontos);

        OnTurnEnd?.Invoke();

        Debug.Log("Turno terminado. Próximo turno terá " + pontosDeAcao + " pontos.");
        OnTurnoTerminado?.Invoke(turnoAtual, totalTurnos);
        OnPontosDeAcaoAtualizados?.Invoke(pontosDeAcao, maxPontos);
    }
}
