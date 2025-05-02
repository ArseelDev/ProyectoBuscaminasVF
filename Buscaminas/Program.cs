using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buscaminas
{
    class Program
    {
        static void Main()
        {
            const int iLongitudCampo = 10; // Longitud del campo de juego (10 casillas)
            const int iNivelesTotales = 5; // Número total de niveles del juego
            Random random = new Random();  // Generador de números aleatorios para ubicar minas

            // Ciclo principal por niveles
            for (int iNivel = 1; iNivel <= iNivelesTotales; iNivel++)
            {
                Console.Clear();
                Console.WriteLine($"Nivel {iNivel}: encuentra todas las casillas seguras (hay {iNivel} mina(s))");

                // Campo visible para el jugador
                char[] cCampoVisible = new char[iLongitudCampo];

                // Campo real con minas (0 = seguro, 1 = mina)
                int[] iCampoReal = new int[iLongitudCampo];

                // Inicializar campo visible con caracteres '#'
                for (int i = 0; i < iLongitudCampo; i++)
                {
                    cCampoVisible[i] = '#';
                }

                // Colocar minas aleatoriamente en el campo real
                int iMinasColocadas = 0;
                while (iMinasColocadas < iNivel)
                {
                    int iPos = random.Next(0, iLongitudCampo); // Posición aleatoria
                    if (iCampoReal[iPos] == 0) // Solo colocar si está libre
                    {
                        iCampoReal[iPos] = 1; // Coloca una mina
                        iMinasColocadas++;
                    }
                }

                int iCasillasSeguras = iLongitudCampo - iNivel; // Total de casillas sin mina
                int iDescubiertas = 0; // Contador de casillas descubiertas
                int iVidas = 5; // Vidas por nivel
                bool bPerdiste = false; // Manera para saber si el jugador perdió

                // Bucle del turno del jugador
                while (!bPerdiste && iDescubiertas < iCasillasSeguras)
                {
                    // Mostrar campo visible al jugador
                    foreach (char c in cCampoVisible)
                        Console.Write(c + " ");
                    Console.WriteLine($"\nVidas restantes: {iVidas}");

                    // Solicitar posición al usuario
                    Console.Write($"Ingresa una posición (1 - {iLongitudCampo}): ");
                    int iPosicion;
                    if (!int.TryParse(Console.ReadLine(), out iPosicion) || iPosicion < 1 || iPosicion > iLongitudCampo)
                    {
                        Console.WriteLine("Entrada inválida. Intenta de nuevo.");
                        continue;
                    }

                    int iIndice = iPosicion - 1; // Convertir a índice de arreglo

                    // Verificar si la casilla ya fue descubierta
                    if (cCampoVisible[iIndice] != '#')
                    {
                        Console.WriteLine("Ya descubriste esta posición.");
                        continue;
                    }

                    // Verificar si hay mina en la posición
                    if (iCampoReal[iIndice] == 1)
                    {
                        cCampoVisible[iIndice] = '*'; // Mostrar mina
                        iVidas--; // Pierde una vida
                        Console.WriteLine("¡Pisaste una mina!");

                        if (iVidas == 0)
                        {
                            Console.WriteLine("Te quedaste sin vidas. Fin del juego.");
                            bPerdiste = true; // Terminar juego
                        }
                    }
                    else
                    {
                        // Contar minas adyacentes a izquierda y derecha
                        int iMinasadyacentes = 0;
                        if (iIndice > 0 && iCampoReal[iIndice - 1] == 1) iMinasadyacentes++;
                        if (iIndice < iLongitudCampo - 1 && iCampoReal[iIndice + 1] == 1) iMinasadyacentes++;

                        // Mostrar '1' si hay minas cerca, '0' si no
                        cCampoVisible[iIndice] = iMinasadyacentes > 0 ? '1' : '0';
                        iDescubiertas++; // Incrementa casillas seguras descubiertas
                    }
                }

                // Mostrar resultado del nivel
                Console.WriteLine("\nResultado del nivel:");
                foreach (char c in cCampoVisible)
                    Console.Write(c + " ");
                Console.WriteLine();

                // Verificar si el jugador perdió o superó el nivel
                if (bPerdiste)
                {
                    Console.WriteLine("Perdiste. ¡Intenta otra vez desde el nivel 1!");
                    break; // Salir del bucle de niveles
                }
                else
                {
                    Console.WriteLine("¡Nivel superado!");
                    if (iNivel < iNivelesTotales)
                    {
                        Console.WriteLine("Presiona Enter para continuar al siguiente nivel...");
                        Console.ReadLine(); // Esperar entrada antes de continuar
                    }
                }
            }

            // Fin del juego
            Console.WriteLine("\n¡Gracias por jugar!");
        }
    }
}