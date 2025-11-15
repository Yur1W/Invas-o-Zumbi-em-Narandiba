using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorDeInimigo : MonoBehaviour
{
    // Prefab do inimigo (arraste no Inspector)
    public GameObject ZumbiComum;
    public GameObject ZumbiPolicial;
    public GameObject ZumbiBomba;

    // Intervalo entre spawns (segundos)
    public float intervalo = 1.3f;

    // Limites de spawn no cenário
   public BoxCollider2D limiteDeSpawn;

    // Velocidade de movimento dos inimigos
    int Enemy;

    void Start()
    {
        limiteDeSpawn = GetComponent<BoxCollider2D>();
        // Começa a gerar inimigos repetidamente
        InvokeRepeating("GerarInimigo", 0f, intervalo);
    }
    void Update()
    {
        Enemy = Random.Range(1, 4);       
    }

    void GerarInimigo()
    {
        // Define posição de spawn (à direita da tela)
        Vector2 posicaoAleatoria = GetRandomPositionCollider();

        // Instancia o inimigo
        switch (Enemy)
        {
            case 1:
                Instantiate(ZumbiComum, posicaoAleatoria, Quaternion.identity);
                break;
            case 2:
                Instantiate(ZumbiPolicial, posicaoAleatoria, Quaternion.identity);
                break;
            case 3:
                Instantiate(ZumbiBomba, new Vector2(9.5f,-2.2f), Quaternion.identity);
                break;
        }
    }
    Vector2 GetRandomPositionCollider()
    {
        Bounds bounds = limiteDeSpawn.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
}
