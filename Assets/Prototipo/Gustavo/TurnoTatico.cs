using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TurnoTatico : MonoBehaviour
{
    [Header("Configuração de Ações")]
    public int pontosDeAcao = 4;
    public int ganhoPorTurno = 2;
    public int maxPontos = 8;

    [Header("Movimentação")]
    public float distanciaMaxima = 5f; // distância máxima
    public float velocidade = 5f; // velocidade de movimento
    private Vector3 posicaoInicialTurno;
    private CharacterController controller;

    public bool turnoPlayer = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        IniciarTurno();
    }

    void Update()
    {
        if (!turnoPlayer)
        {
            // Simula turno do chefe
            if (Input.GetKeyDown(KeyCode.Space) && !turnoPlayer)
            {
                IniciarTurno();
            }
        }
        else
        {
            // Input de movimento
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");
            Vector3 movimento = new Vector3(inputX, 0, inputZ);

            if (movimento.magnitude > 0.01f)
            {
                controller.Move(movimento.normalized * velocidade * Time.deltaTime);
            }

            // Testa se ainda está dentro da distância máxima do turno
            float distanciaPercorrida = Vector3.Distance(posicaoInicialTurno, transform.position);
            if (distanciaPercorrida > distanciaMaxima)
            {
                Debug.Log("Distância máxima de movimentação atingida!");

                // Corrige para não ultrapassar o limite
                Vector3 direcao = (transform.position - posicaoInicialTurno).normalized;
                Vector3 posicaoCorreta = posicaoInicialTurno + direcao * distanciaMaxima;
                controller.enabled = false; // desativa momentaneamente para setar posição manualmente
                transform.position = posicaoCorreta;
                controller.enabled = true;
            }

            // Usar habilidade (tecla 1)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UsarHabilidade(3); // habilidade custa 3 pontos
            }

            // Pular turno (Espaço)
            if (Input.GetKeyDown(KeyCode.Space) && turnoPlayer)
            {
                TerminarTurno();
            }

            DesenharCirculo(posicaoInicialTurno, distanciaMaxima, 36, Color.blue);
        }

        
    }

    public void UsarHabilidade(int custo)
    {
        if (pontosDeAcao >= custo)
        {
            pontosDeAcao -= custo;
            Debug.Log("Habilidade usada. Pontos restantes: " + pontosDeAcao);
            // Aqui entra a logica da habilidade
        }
        else
        {
            Debug.Log("Pontos insuficientes");
        }
    }

    public void IniciarTurno()
    {
        turnoPlayer = true;
        posicaoInicialTurno = transform.position;
        Debug.Log("Início do turno. Pontos de ação: " + pontosDeAcao);
    }

    public void TerminarTurno()
    {
        turnoPlayer = false;

        // Recupera pontos de ação
        pontosDeAcao = Mathf.Min(pontosDeAcao + ganhoPorTurno, maxPontos);

        Debug.Log("Turno terminado. Próximo turno terá " + pontosDeAcao + " pontos.");
    }

    void DesenharCirculo(Vector3 centro, float raio, int segmentos, Color cor)
    {
        float angulo = 0f;
        Vector3 pontoAnterior = centro + new Vector3(Mathf.Cos(0) * raio, 0, Mathf.Sin(0) * raio);

        for (int i = 1; i <= segmentos; i++)
        {
            angulo = i * 2 * Mathf.PI / segmentos;
            Vector3 pontoNovo = centro + new Vector3(Mathf.Cos(angulo) * raio, 0, Mathf.Sin(angulo) * raio);

            Debug.DrawLine(pontoAnterior, pontoNovo, cor);
            pontoAnterior = pontoNovo;
        }
    }
}
