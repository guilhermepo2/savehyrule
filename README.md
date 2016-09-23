# Save Hyrule
Algoritmo A* para um mapa baseado em Zelda, trabalho da disciplina Inteligência Artificial (GBC063)

## Heurística

Duas Heurísicas estão implementadas no código, a **Distância Euclidiana** e a **Distância de Manhattan**, as duas conseguem alcançar o menor custo através da execução do algoritmo A*, mas quando o coeficiente da Heurística pela Distância de Manhattan aumenta, a performance do algoritmo tende a cair, segue o resultado de alguns testes executados:

Esses testes foram executados percorrendo as seguintes ordem de dungeons:
1. Inferior à Esquerda
2. Superior Central
3. À direita, no meio do mapa
4. Lost Woods

Heurística Utilizada | Coeficiente | Custo Encontrado
-------------------- | ----------- | ---------------
Distância Manhattan  | 1 | 6170
Distância Manhattan  | 2 | 6170
Distância Manhattan  | 3 | 6170
Distância Manhattan  | 4 | 6170
Distância Manhattan  | 5 | 6190
Distância Manhattan  | 10 | 6210
Distância Manhattan  | 50 | 7310
Distância Manhattan  | 100 | 8150
Distância Euclidiana | 1 | 6170

Uma parte do Trabalho era implementar um Algoritmo de Busca para calcular o menor caminho para se percorrer as três dungeons.
Foi implementado o Algoritmo Hill Climbing para fazer isso, o resultado com a utilização do Hill Climbing seguem abaixo:

Heurística Utilizada | Coeficiente | Custo Encontrado
-------------------- | ----------- | ---------------
Distância Manhattan  | 1 | 6110
Distância Manhattan  | 2 | 6110
Distância Manhattan  | 3 | 6110
Distância Manhattan  | 4 | 6110
Distância Manhattan  | 5 | 6110
Distância Manhattan  | 9 | 6110
Distância Manhattan  | 10 | 6130
Distância Manhattan  | 50 | 7400
Distância Manhattan  | 100 | 7880
Distância Euclidiana | 1 | 6110
