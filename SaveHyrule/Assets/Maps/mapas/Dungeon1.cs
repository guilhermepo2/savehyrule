using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * O mapa principal deve ser representado por uma matriz 42 x 42 (igual à mostrada na Figura 3). 
 * As Dungeons também devem ser representadas por matrizes de tamanho 28 x 28 (iguais às mostradas na Figura 4).
 */

public class Dungeon1 : Map {
public Dungeon1() { 
map = new List<List<char>> {
new List<char> { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X' },
new List<char> { 'X','D','D','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','D','D','X','X','X','X' },
new List<char> { 'X','D','D','D','X','X','X','X','X','X','X','X','D','D','D','X','X','X','X','X','X','D','D','D','X','X','X','X' },
new List<char> { 'X','X','D','X','X','X','X','D','D','D','D','D','D','D','D','X','X','X','X','X','X','D','D','D','X','X','X','X' },
new List<char> { 'X','X','D','X','X','X','X','D','X','X','X','X','D','D','D','X','X','X','X','X','X','X','D','X','X','X','X','X' },
new List<char> { 'X','X','D','D','D','D','D','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','X','X','X','X','X' },
new List<char> { 'X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','X','X','X','X','X' },
new List<char> { 'X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','D','D','D','D','D','D','X','X' },
new List<char> { 'X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','D','D','D','D','D','D','X','X' },
new List<char> { 'X','X','D','D','D','D','D','D','X','X','X','D','D','D','D','D','X','X','X','D','D','D','D','D','D','D','X','X' },
new List<char> { 'X','X','X','D','X','X','X','D','X','X','X','D','D','D','D','D','X','X','X','D','D','D','D','D','D','D','X','X' },
new List<char> { 'X','X','X','D','X','X','X','D','X','X','X','X','X','D','X','X','X','X','X','X','X','X','D','X','X','X','X','X' },
new List<char> { 'X','X','D','D','D','X','X','D','X','X','X','X','X','D','X','X','X','X','X','X','X','X','D','X','X','X','X','X' },
new List<char> { 'X','X','D','D','D','X','X','D','X','X','X','X','X','D','X','X','X','X','X','X','D','D','D','D','D','X','X','X' },
new List<char> { 'X','X','D','D','D','X','X','D','X','X','X','X','X','D','X','X','X','X','X','X','D','X','X','X','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','D','D','D','D','D','D','D','D','D','D','D','D','D','D','X','X','X','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','D','X','X','X','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','D','X','X','X','D','X','X','X' },
new List<char> { 'X','X','D','D','D','X','X','D','X','X','D','D','D','X','X','X','X','X','D','D','D','D','X','X','D','X','X','X' },
new List<char> { 'X','X','D','D','D','D','D','D','D','D','D','D','D','X','X','X','X','X','D','D','D','D','X','X','D','X','X','X' },
new List<char> { 'X','X','D','D','D','X','X','D','X','X','D','D','D','X','X','X','X','X','D','D','D','D','X','X','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','D','X','X','X','X','X','X','D','D','D','D','D','D','D','D','D','D','D','X','X','X' },
new List<char> { 'X','X','X','X','X','X','D','D','D','D','X','X','X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X' },
new List<char> { 'X','X','X','X','X','X','D','D','D','D','X','X','X','X','D','X','X','X','X','X','X','X','X','X','X','X','X','X' },
new List<char> { 'X','X','X','X','X','X','D','D','D','D','X','X','D','D','D','D','D','X','X','X','X','X','X','X','X','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','X','X','X','X','X','D','D','D','D','D','X','X','X','X','X','X','X','X','X','X','X' },
new List<char> { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X' },
};
}
}
