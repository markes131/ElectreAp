﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectreAp;
/*using static ElectreAp.Tables;*/

namespace ElectreAp
{
    class ElectreIII : DataForAlgorithm, IAlgorithm, IElectreIII
    {

        public ElectreIII() {

        }

        public double CalculateSDeltaK(double deltaLast) {
            sDeltaK = beta - (alfa * deltaLast);
            return sDeltaK;
        }

        public void CalculateThreshold(double[,,] table, double value, int i, int mod) {
            switch (mod) {

                // mod invers
                case 0:
                    Console.WriteLine("CASE 0");
                    Console.WriteLine("Q PRZEL. ALFA * var + PRZEL. BETA");
                    progQ = Math.Round((DoInvers(table[i,0,0], table[i,0,1], 0) * value + DoInvers(table[i,0,0], table[i,0,1], 1)), miejscPoPrzecinku);
                    Console.WriteLine(DoInvers(table[i,0,0], table[i,0,1], 0) + " * " + value + " + " + DoInvers(table[i,0,0], table[i,0,1], 1));
                    Console.WriteLine("PROG Q = " + progQ);
                    Console.WriteLine();

                    Console.WriteLine("P PRZEL. ALFA * var + PRZEL. BETA");
                    progP = Math.Round((DoInvers(table[i,1,0], table[i,1,1], 0) * value + DoInvers(table[i,1,0], table[i,1,1], 1)), miejscPoPrzecinku);
                    Console.WriteLine(DoInvers(table[i,1,0], table[i,1,1], 0) + " * " + value + " + " + DoInvers(table[i,1,0], table[i,1,1], 1));
                    Console.WriteLine("PROG P = " + progP);
                    Console.WriteLine();

                    Console.WriteLine("V PRZEL. ALFA * var + PRZEL. BETA");
                    progV = Math.Round((DoInvers(table[i,2,0], table[i,2,1], 0) * value + DoInvers(table[i,2,0], table[i,2,1], 1)), miejscPoPrzecinku);
                    Console.WriteLine(DoInvers(table[i,2,0], table[i,2,1], 0) + " * " + value + " + " + DoInvers(table[i,2,0], table[i,2,1], 1));
                    Console.WriteLine("PROG V = " + progV);
                    Console.WriteLine();
                    break;

                // mod direct
                case 1:
                    Console.WriteLine("CASE 1");
                    progQ = Math.Round((table[i,0,0] * value + table[i,0,1]), miejscPoPrzecinku);
                    Console.WriteLine(table[i,0,0] + " * " + value + " + " + table[i,0,1]);

                    progP = Math.Round((table[i,1,0] * value + table[i,1,1]), miejscPoPrzecinku);
                    Console.WriteLine(table[i,1,0] + " * " + value + " + " + table[i,1,1]);

                    progV = Math.Round((table[i,2,0] * value + table[i,2,1]), miejscPoPrzecinku);
                    Console.WriteLine(table[i,2,0] + " * " + value + " + " + table[i,2,1]);
                    break;
            }
        }

        public void CheckingIsThereThisSameOptions(int y) {
            if (y < punktacjaOpcjiZmienna.GetLength(1) - 1) {
                if (Int32.Parse(punktacjaOpcjiZmienna[1, y]) == Int32.Parse(punktacjaOpcjiZmienna[1, y + 1])) {
                    if (CboxRankingsChecked) {
                        Console.WriteLine(punktacjaOpcjiZmienna[0, y] + " ");
                    }
                    y++;
                    CheckingIsThereThisSameOptions(y);
                }
                else {
                    if (CboxRankingsChecked) {
                        Console.WriteLine(punktacjaOpcjiZmienna[0, y] + " ");
                    }
                    y++;
                    CheckingIsThereThisSameOptions(y);
                }
            }
            else if (y == punktacjaOpcjiZmienna.GetLength(1) - 1) {
                if (Int32.Parse(punktacjaOpcjiZmienna[1, y - 1]) == Int32.Parse(punktacjaOpcjiZmienna[1, y])) {
                    if (CboxRankingsChecked) {
                        Console.WriteLine(punktacjaOpcjiZmienna[0, y] + " ");
                    }
                }
                else {
                    if (CboxRankingsChecked) {
                        Console.WriteLine(punktacjaOpcjiZmienna[0, y] + " ");
                    }
                }
            }
            if (CboxRankingsChecked) {
                Console.WriteLine("\n\n");
            }
        }

        public void CreateConcordanceSets() {
            // var1 i var2 są to zmienne do których przypisujemy odpowiednie wartości alternatyw, a te są wykorzystywane w algorytmie
            double var2 = 0;
            double var1 = 0;
            // y1 i z1 są to współrzędne odpowiednio wiersza i kolumny zmiennej var1
            int y1 = 0;
            int z1 = 0;

            // TABELA ALTERNATYW INACZEJ MACIERZ -> WYCIETA TABELA Z INTERFEJSU CZYLI KOLUMNY TO KRYTERIA A WIERSZE TO ALTERNATYWY
            /* licz1 (poruszanie się po kolumnach(kryteriach) tabeliAlternatyw)*/
            for (int licz1 = 0; licz1 < numberOfCriterias; licz1++)
            {

                /* tworzymy nowy obiekt matrixa (wymiar zależy od zadeklarowanej liczby alternatyw), do którego będą zapisywane nowe wartości */
                Double[,] matrixKryterium = new Double[numberOfAlternatives, numberOfAlternatives];

                // sprawdzamy czy kierunek rosnący
                if (listaKierunkow[licz1] == 1) {
                    //textAreaAll.appendText("Kierunek Rosnący\n");

                    // poruszanie się po kolumnach alternatyw
                    for (int i = 0; i < numberOfAlternatives; i++) {

                        /* licz2 (poruszanie się po wierszach) */
                        for (int licz2 = 0; licz2 < numberOfAlternatives; licz2++) {
                            if (i == 0 && licz2 == 0) {
                                // na początku obydwie porównywalne wartości są takie same
                                var1 = tabelaAlternatyw[licz2,licz1];
                                y1 = licz2;
                                z1 = licz1;

                                var2 = tabelaAlternatyw[licz2,licz1];
                            }
                            else {
                                // w kolejnych krokach pierwsza wartość zmienia się dopiero przy pętli licz1
                                // a druga wartość właśnie teraz w tej pętli - jest to poruszanie się po tabelaAlternatyw[][]
                                // dla danej tabeli będzie to wartość z wiersza LICZ2 i kolumny LICZ1
                                var2 = tabelaAlternatyw[licz2,licz1];
                            }
                            switch (listaModow[licz1]) {
                                // stałe - bez wzoru
                                case -1:
                                    progQ = listaProguQB[licz1];
                                    progP = listaProguPB[licz1];
                                    progV = listaProguVB[licz1];
                                    break;
                                // inverse    
                                case 0:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var1, licz1, 0); }
                                    else { CalculateThreshold(listaWartProgKryt, var2, licz1, 0); }
                                    break;
                                // direct    
                                case 1:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var2, licz1, 1); }
                                    else { CalculateThreshold(listaWartProgKryt, var1, licz1, 1); }
                                    break;
                            }
                            //                      WEDŁUG ELECTRE III-IV ROYA #1
                            if (var2 >= (var1 + progP)) {
                                /* TO ZWRÓĆ WARTOŚĆ 0 */
                                matrixKryterium[i, licz2] = 0;
                            }
                            if ((var1 + progQ) < var2 && var2 < (var1 + progP)) {
                                double suma = (progP - var2 + var1) / (progP - progQ);
                                matrixKryterium[i,licz2] = Math.Round(suma, miejscPoPrzecinku);
                                suma = 0;
                            }
                            if (var2 <= (var1 + progQ)) {
                                /* TO ZWRÓĆ WARTOŚĆ 1 */
                                matrixKryterium[i, licz2] = 1;
                            }
                        }
                        if (y1 + 1 < numberOfAlternatives) {
                            y1 = y1 + 1;
                            var1 = tabelaAlternatyw[y1,z1];
                        }
                    }
                }
                // sprawdzamy czy kierunek malejący
                else if (listaKierunkow[licz1] == 0) {
                    for (int i = 0; i < numberOfAlternatives; i++) {

                        /* licz2 (poruszanie się po wierszach) */
                        for (int licz2 = 0; licz2 < numberOfAlternatives; licz2++) {
                            if (i == 0 && licz2 == 0) {
                                // na początku obydwie porównywalne wartości są takie same
                                var1 = tabelaAlternatyw[licz2,licz1];
                                y1 = licz2;
                                z1 = licz1;

                                var2 = tabelaAlternatyw[licz2,licz1];
                            }
                            else {
                                // w kolejnych krokach pierwsza wartość zmienia się dopiero przy pętli licz1
                                // a druga wartość właśnie teraz w tej pętli - jest to poruszanie się po tabelaAlternatyw[][]
                                // dla danej tabeli będzie to wartość z wiersza LICZ2 i kolumny LICZ1
                                var2 = tabelaAlternatyw[licz2,licz1];
                            }
                            switch (listaModow[licz1]) {
                                case -1:
                                    progQ = listaProguQB[licz1];
                                    progP = listaProguPB[licz1];
                                    progV = listaProguVB[licz1];
                                    break;
                                case 0:
                                    if (var1 < var2) {
                                        CalculateThreshold(listaWartProgKryt, var2, licz1, 0);
                                    }
                                    else {
                                        CalculateThreshold(listaWartProgKryt, var1, licz1, 0);
                                    }
                                    break;
                                case 1:
                                    if (var1 < var2) {
                                        CalculateThreshold(listaWartProgKryt, var1, licz1, 1);
                                    }
                                    else {
                                        CalculateThreshold(listaWartProgKryt, var2, licz1, 1);
                                    }
                                    break;
                            }
                            //                      WEDŁUG ELECTRE III-IV ROYA
                            if (var1 - progP > var2) {
                                /* TO ZWRÓĆ WARTOŚĆ 0 */
                                matrixKryterium[i, licz2] = 0;
                            }
                            if (progQ < (var1 - var2) && (var1 - var2) < progP) {
                                double suma = (progP - var1 + var2) / (progP - progQ);
                                matrixKryterium[i, licz2] = Math.Round(suma, miejscPoPrzecinku);
                                suma = 0;
                            }
                            if (var1 - var2 <= progQ) {
                                /* TO ZWRÓĆ WARTOŚĆ 1 */
                                matrixKryterium[i, licz2] = 1;
                            }
                        }
                        if (y1 + 1 < numberOfAlternatives) {
                            y1 = y1 + 1;
                            var1 = tabelaAlternatyw[y1,z1];
                        }
                    }
                }
                else {
                    Console.WriteLine("BŁĘDNE DANE W KOMORKACH KIERUNKOW, może być tylko 0 lub 1");
                }
                listaZbiorowZgodnosci.Add(matrixKryterium);
            }
        }

        public void CreateDiscordanceSets() {
            // var1 i var2 są to zmienne do których przypisujemy odpowiednie wartości alternatyw, a te są wykorzystywane w algorytmie
            double var2 = 0;
            double var1 = 0;
            // y1 i z1 są to współrzędne odpowiednio wiersza i kolumny zmiennej var1
            int y1 = 0;
            int z1 = 0;

            /* licz1 (poruszanie się po kolumnach / kryteriach)*/
            for (int licz1 = 0; licz1 < numberOfCriterias; licz1++) {
                /* tworzymy nowy obiekt matrixa (wymiar zależy od zadeklarowanej liczby alternatyw), do którego będą zapisywane nowe wartości */
                Double[,] matrixKryterium = new double[numberOfAlternatives, numberOfAlternatives];
                // sprawdzamy czy kierunek rosnący
                if (listaKierunkow[licz1] == 1) {
                    // poruszanie się po kolumnach alternatyw
                    for (int i = 0; i < numberOfAlternatives; i++) {
                        /* licz2 (poruszanie się po wierszach) */
                        for (int licz2 = 0; licz2 < numberOfAlternatives; licz2++) {
                            if (i == 0 && licz2 == 0) {
                                // na początku obydwie porównywalne wartości są takie same
                                var1 = tabelaAlternatyw[licz2, licz1];
                                y1 = licz2;
                                z1 = licz1;
                                var2 = tabelaAlternatyw[licz2, licz1];
                            }
                            else {
                                // w kolejnych krokach pierwsza wartość zmienia się dopiero przy pętli licz1
                                // a druga wartość właśnie teraz w tej pętli - jest to poruszanie się po tabelaAlternatyw[][]
                                // dla danej tabeli będzie to wartość z wiersza LICZ2 i kolumny LICZ1
                                var2 = tabelaAlternatyw[licz2, licz1];
                            }

                            switch (listaModow[licz1]) {
                                case -1:
                                    progQ = listaProguQB[licz1];
                                    progP = listaProguPB[licz1];
                                    progV = listaProguVB[licz1];
                                    break;
                                case 0:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var1, licz1, 0); }
                                    else { CalculateThreshold(listaWartProgKryt, var2, licz1, 0); }
                                    break;
                                case 1:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var2, licz1, 1); }
                                    else { CalculateThreshold(listaWartProgKryt, var1, licz1, 1); }
                                    break;
                            }

                            if (listaWartProgKryt[licz1,2,0] == 999999999.9) {
                                matrixKryterium[i, licz2] = 0;
                            }
                            //                  WEDŁUG ROYA
                            else if (var2 <= var1 + progP) {
                                matrixKryterium[i, licz2] = 0;
                            }
                            else if (progP < var2 - var1 && var2 - var1 < progV) {
                                Console.WriteLine("var2 - var1 - progP / progV - progP");
                                Console.WriteLine(var2 + " - " + var1 + " - " + progP + " / " + progV + " - " + progP);
                                double suma = ((var2 - var1 - progP) / (progV - progP));
                                matrixKryterium[i, licz2] = Math.Round(suma, miejscPoPrzecinku);
                                suma = 0;
                            }
                            else if (var2 >= var1 + progV) {
                                matrixKryterium[i, licz2] = 1;
                            }
                        }

                        if (y1 + 1 < numberOfAlternatives) {
                            y1 = y1 + 1;
                            var1 = tabelaAlternatyw[y1, z1];
                        }
                    }
                }
                // sprawdzamy czy kierunek malejący
                else if (listaKierunkow[licz1] == 0) {
                    for (int i = 0; i < numberOfAlternatives; i++) {
                        /* licz2 (poruszanie się po wierszach) */
                        for (int licz2 = 0; licz2 < numberOfAlternatives; licz2++) {
                            if (i == 0 && licz2 == 0) {
                                // na początku obydwie porównywalne wartości są takie same
                                var1 = tabelaAlternatyw[licz2, licz1];
                                y1 = licz2;
                                z1 = licz1;
                                var2 = tabelaAlternatyw[licz2, licz1];
                            }
                            else {
                                // w kolejnych krokach pierwsza wartość zmienia się dopiero przy pętli licz1
                                // a druga wartość właśnie teraz w tej pętli - jest to poruszanie się po tabelaAlternatyw[][]
                                // dla danej tabeli będzie to wartość z wiersza LICZ2 i kolumny LICZ1
                                var2 = tabelaAlternatyw[licz2, licz1];
                            }

                            switch (listaModow[licz1]) {
                                case -1:
                                    progQ = listaProguQB[licz1];
                                    progP = listaProguPB[licz1];
                                    progV = listaProguVB[licz1];
                                    break;
                                case 0:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var2, licz1, 0); }
                                    else { CalculateThreshold(listaWartProgKryt, var1, licz1, 0); }
                                    break;
                                case 1:
                                    if (var1 < var2) { CalculateThreshold(listaWartProgKryt, var2, licz1, 1); }
                                    else { CalculateThreshold(listaWartProgKryt, var1, licz1, 1); }
                                    break;
                            }

                            if (listaWartProgKryt[licz1, 2, 0] == 999999999.9) {
                                matrixKryterium[i, licz2] = 0;
                            }
                            //                  WEDŁUG ROYA
                            else if ((var1 - var2) <= progP) {
                                Console.WriteLine(var1 + " - " + var2 + " <= " + progP);
                                matrixKryterium[i, licz2] = 0;
                            }
                            else if (var1 - progV < var2 && var2 < var1 - progP) {
                                Console.WriteLine("progP < var1 - var2 && var1 - var2 < progV");
                                Console.WriteLine(progP + " < " + var1 + " - " + var2 + " && " + var1 + " - " + var2 + " < " + progV);
                                double suma = ((var1 - var2 - progP) / (progV - progP));
                                matrixKryterium[i, licz2] = Math.Round(suma, miejscPoPrzecinku);
                                suma = 0;
                            }
                            else if ((var1 - var2) >= progV) {
                                matrixKryterium[i, licz2] = 1;
                            }
                        }

                        if (y1 + 1 < numberOfAlternatives) {
                            y1 = y1 + 1;
                            var1 = tabelaAlternatyw[y1, z1];
                        }
                    }
                }

                listaZbiorowNieZgodnosci.Add(matrixKryterium);
            }
        }

        //public void CreateFinalRanking(double[,] tabSum)
        public void CreateFinalRanking() {
            listaAlternatyw = new List<List<Int32>>();
            // budowanie listy kto z kim przegrywa
            for (int i = 0; i < TabSum.GetLength(1); i++) {
                listaKtoZKimPrzegral = new List<Int32>();
                for (int j = 0; j < TabSum.GetLength(0); j++) {
                    if (TabSum[i, j] == -1) {
                        listaKtoZKimPrzegral.Add((j + 1));
                    }
                }
                listaAlternatyw.Add(listaKtoZKimPrzegral);
            }
            // wypisywanie powyższej listy
            for (int i = 1; i < listaAlternatyw.Count + 1; i++) {
                Console.WriteLine("A" + i + " ");
                for (int j = 0; j < listaAlternatyw[i - 1].Count; j++) {
                    if (!listaAlternatyw[i - 1].Any()) {
                        Console.Write(" NULL");
                    }
                    else {
                        Console.Write("a" + listaAlternatyw[i - 1][j]+ " ");
                    }
                }
                Console.WriteLine();
            }

            listaRank = new List<List<Int32>>();

            listaAltWRank = new List<Int32>();
            listaAltWRankNieSort = new List<Int32>();
            listaChwilowa = new List<Int32>();

            FindMin();

            Console.WriteLine("RANKING FINALNY:");
            for (int i = 0; i < listaRank.Count; i++) {
                for (int j = 0; j < listaRank[i].Count; j++) {
                    Console.Write("A" + listaRank[i][j] + " ");
                }
                Console.WriteLine();
            }
            ChangeRankListToTable(listaRank);
        }
        
        public void ChangeRankListToTable(List<List<Int32>> listaRank) {
            for (int i = 0; i < listaRank.Count; i++) {
                for (int j = 0; j < listaRank[i].Count; j++) {
                    tabRank[1, listaRank[i][j] - 1] = (i + 1).ToString();
                }
            }
        }

        //public void CreateOutrankingSets(List<double[,]> listOfOutrankingSets)
        public void CreateOutrankingSets() {
            Console.WriteLine("SPRAWDZAMY listaZbiorowNieZgodnosci.size() =" + listaZbiorowNieZgodnosci.Count);
            for (int numerZbioru = 0; numerZbioru < listaZbiorowNieZgodnosci.Count; numerZbioru++) {
                /* tworzymy nowy obiekt matrixa (wymiar zależy od zadeklarowanej liczby alternatyw), do którego będą zapisywane nowe wartości */
                Double[,] matrixKryterium = new double[numberOfAlternatives, numberOfAlternatives];
                for (int numerWiersza = 0; numerWiersza < listaZbiorowNieZgodnosci[numerZbioru].GetLength(1); numerWiersza++) {
                    for (int numerKolumny = 0; numerKolumny < listaZbiorowNieZgodnosci[numerZbioru].GetLength(0); numerKolumny++) {
                        if (listaZbiorowNieZgodnosci[numerZbioru][numerWiersza, numerKolumny] > concordanceMatrix[numerWiersza, numerKolumny]) {
                            matrixKryterium[numerWiersza, numerKolumny] = 1;
                        }
                        else {
                            matrixKryterium[numerWiersza, numerKolumny] = 0;
                        }
                    }
                }
                listaZbiorowPrzewyzszania.Add(matrixKryterium);
            }
        }

       // public void CreateSumTableOfDistillations(double[,] tabTopDownDistillation, double[,] tabUpwardDistillation)
        public void CreateSumTableOfDistillations() {
            tabSum = new double[numberOfAlternatives, numberOfAlternatives];
            for (int i = 0; i < tabSum.GetLength(1); i++) {
                for (int j = 0; j < tabSum.GetLength(0); j++) {
                    if (tabZstep[i, j] == 1 && tabWstep[i, j] == 1) {
                        tabSum[i, j] = 1;
                    }
                    else if ((tabZstep[i, j] == 1 && tabWstep[i, j] == 0) || (tabZstep[i, j] == 0 && tabWstep[i, j] == 1)) {
                        tabSum[i, j] = 1;
                    }
                    else if ((tabZstep[i, j] == 0 && tabWstep[i, j] == 0)) {
                        tabSum[i, j] = 0;
                    }
                    else if ((tabZstep[i, j] == 0 && tabWstep[i, j] == -1) || (tabZstep[i, j] == -1 && tabWstep[i, j] == 0)) {
                        tabSum[i, j] = -1;
                    }
                    else if ((tabZstep[i, j] == -1 && tabWstep[i, j] == -1)) {
                        tabSum[i, j] = -1;
                    }
                    else if ((tabZstep[i, j] == -1 && tabWstep[i, j] == 1) || (tabZstep[i, j] == 1 && tabWstep[i, j] == -1)) {
                        tabSum[i, j] = 2;
                    }
                }
            }
        }

        public void CreateTabOfDistillation(double[,] tableOfDistillation, String[,] placesOfOptionsAfterDistillation, int valueX, int valueY, int valueZ) {
            tableOfDistillation = new double[numberOfAlternatives, numberOfAlternatives];
            for (int i = 0; i < tableOfDistillation.GetLength(1); i++) {
                for (int j = 0; j < tableOfDistillation.GetLength(0); j++) {
                    if (Int32.Parse(placesOfOptionsAfterDistillation[1, i]) < Int32.Parse(placesOfOptionsAfterDistillation[1, j])) {
                        tableOfDistillation[i, j] = valueX;
                    }
                    else if (Int32.Parse(placesOfOptionsAfterDistillation[1, i]) == Int32.Parse(placesOfOptionsAfterDistillation[1, j])) {
                        tableOfDistillation[i, j] = valueY;
                    }
                    else {
                        tableOfDistillation[i, j] = valueZ;
                    }
                }
            }
        }

        public void ShowTabOfDistillation(Double[,] tab) {

        }

        //Double[,] concordanceMatrix;
        double valueOfConcordance;

        //public void CreateConcordanceMatrix(List<Double[,]> listaZbiorowZgodnosci, int numberOfAlternatives) {
        public void CreateConcordanceMatrix() {
            concordanceMatrix = new Double[numberOfAlternatives, numberOfAlternatives];
            double sumOfQuotients = 0;
            double sumOfWeights= 0;

            // wybór kolumny
            for (int j = 0; j < numberOfAlternatives; j++) {
                // wybór wiersza
                for (int k = 0; k < numberOfAlternatives; k++) {
                    sumOfWeights = 0;
                    sumOfQuotients = 0;

                    // wybór zbioru zgodnosci dla i-tego kryterium
                    for (int i = 0; i < listaZbiorowZgodnosci.Count; i++) {
                        sumOfQuotients = sumOfQuotients + (listaWagW[i] * (double)listaZbiorowZgodnosci[i].GetValue(k, j));
                        sumOfWeights = sumOfWeights + listaWagW[i];
                    }
                    if (sumOfWeights == 0) {
                        valueOfConcordance = 0;
                    }
                    else {
                        valueOfConcordance = (sumOfQuotients / sumOfWeights);
                    }

                    concordanceMatrix[k,j] = Math.Round(valueOfConcordance, miejscPoPrzecinku);
                }
            }
        }

        string pathMathImg;
        public void DoCalculations() {

            DivideThresholdsToLists();
            CreateMatrixForAlternativeData(numberOfAlternatives, numberOfCriterias);
            CreateConcordanceSets();

        }

        public void DivideThresholdsToLists() {
            /*
                uzupełnianie listy kierunków -> pobieranie wartości z matrixa tabeli
                wartość kierunku ( 0 lub 1 ) mówi nam czy wartości ujemne ( 0 ) czy wartości dodatnie ( 1 ) 
                dla danego kryterium są lepsze od tych drugich
                np. kryterium pojemność silnika im większa wartość ( wartość kierunku 1 ) tym lepiej dla danej alternatywy (motoru)
                np. kryterium oddległość od centrum miasta im mniejsza wartość ( wartość kierunku 0 ) tym lepiej dla danej alternatywy (nieruchomości)
            */

            for (int z = 0; z < numberOfCriterias; z++) {
                listaKierunkow.Add(tabelaMatrix[0,z]);
            }
            
            Console.WriteLine("K");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaKierunkow[z] + " ");
            }
            Console.WriteLine();


            for (int z = 0; z < numberOfCriterias; z++) {
                listaModow.Add((Int32)(tabelaMatrix[1,z]));
            }
            Console.WriteLine("M");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaModow[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Wag W -> pobieranie wartości z matrixa tabeli
            for (int z = 0; z < numberOfCriterias; z++) {
                listaWagW.Add(tabelaMatrix[2,z]);
            }
            Console.WriteLine("W");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaWagW[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu Q -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguQA.Add(tabelaMatrix[3,z]);
            }
            Console.WriteLine("QA");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaProguQA[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu P -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguPA.Add(tabelaMatrix[4,z]);
            }
            Console.WriteLine("PA");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaProguPA[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu V -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguVA.Add(tabelaMatrix[5,z]);
            }
            Console.WriteLine("VA");
            for (int z = 0; z < numberOfCriterias; z++) {

                Console.Write(listaProguVA[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu Q -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguQB.Add(tabelaMatrix[6,z]);
            }
            Console.WriteLine("QB");
            for (int z = 0; z < numberOfCriterias; z++) {

                Console.Write(listaProguQB[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu P -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguPB.Add(tabelaMatrix[7,z]);
            }
            Console.WriteLine("PB");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaProguPB[z] + " ");
            }
            Console.WriteLine();


            // uzupełnianie listy Progu V -> pobieranie wartości z matrixa tabeli        
            for (int z = 0; z < numberOfCriterias; z++) {
                listaProguVB.Add(tabelaMatrix[8,z]);
            }
            Console.WriteLine("VB");
            for (int z = 0; z < numberOfCriterias; z++) {
                Console.Write(listaProguVB[z] + " ");
            }
            Console.WriteLine();

            for (int z = 0; z < numberOfAlternatives; z++) {
                listaNumerowZNazwOpcji.Add(z);
                listaNumerowZNazwOpcjiOgolZstep.Add(z);
                listaNumerowZNazwOpcjiOgolWstep.Add(z);
            }

            miejscaOpcjiPoDestylacjiZstepujacej = new String[2,numberOfAlternatives];
            miejscaOpcjiPoDestylacjiWstepujacej = new String[2,numberOfAlternatives];
            tabRank = new String[2,numberOfAlternatives];
            punktacjaOpcji = new String[2,numberOfAlternatives];
            punktacjaOpcjiZmienna = new String[2,numberOfAlternatives];

            for (int z = 0; z < numberOfAlternatives; z++) {
                miejscaOpcjiPoDestylacjiZstepujacej[0,z] = "A" + (z + 1);
                miejscaOpcjiPoDestylacjiZstepujacej[1,z] = "0";
                tabRank[0,z] = "A" + (z + 1); ;
                tabRank[1,z] = "0";
                miejscaOpcjiPoDestylacjiWstepujacej[0,z] = "A" + (z + 1);
                miejscaOpcjiPoDestylacjiWstepujacej[1,z] = "0";
                punktacjaOpcji[0,z] = "A" + (z + 1);
                punktacjaOpcji[1,z] = "0";
            }
        }

        public double DoInvers(double symbolA, double symbolB, int positionOfSymbol) {
            double symbol = 0;

            switch (positionOfSymbol) {
                // obliczamy alfe
                case 0:
                    symbol = (symbolA) / (1 + symbolA);
                    break;
                // obliczamy bete
                case 1:
                    symbol = (symbolB) / (1 + symbolA);
                    break;
            }

            return Math.Round(symbol, miejscPoPrzecinku);
        }

        public void DoStageFirst()
        {
            
        }

        public void DoStageSecond()
        {
            
        }

        public void DoStepFifth(double lastDelta, double[,] workingMatrix, bool typeOfDistillation, List<int> workingListOfNumbersOfOptions)
        {
            
        }

        public void DoStepFourth(double deltaLast, double[,] workingMatrix, bool typeOfDistillation, List<int> workingListOfNumbersOfOptions)
        {
            
        }

        public void DoStepSecond(double[,] workingMatrix, bool typeOfDistillation, List<int> workingListOfNumbersOfOptions)
        {
            
        }

        public void DoStepSeventh(double[,] ratingMatrix, double qualificationOfTheBestOption, double[,] workingMatrix, bool typeOfDistillation, List<int> workingListOfNumbersOfOptions)
        {
            
        }

        public void DoStepSixth(double[,] ratingMatrix, double[,] workingMatrix, bool typeOfDistillation, List<int> workingListOfNumbersOfOptions)
        {
            
        }

        public void Exchange(int max, int min)
        {
            
        }

        public void FindMax(string[][] rankingOfOptionsAfterDistillation)
        {
            
        }

        public void FindMax(string[,] rankingOfOptionsAfterDistillation)
        {
            
        }

        public void FindMin()
        {
            
        }

        public void PrepareTopDownDistillation() {
            listaNumerowZNazwOpcjiUsytWRank.Clear();
            newDelta = 0.0;
            typDestylacji = true;
        }

        public void PrepareUpwardDistillation() {
            listaNumerowZNazwOpcjiUsytWRank.Clear();
            newDelta = 0.0;
            typDestylacji = false;
        }

        public void PrepareUpwardScore() {
            FindMax(miejscaOpcjiPoDestylacjiWstepujacej);
            //ZnajdzMaxim(miejscaOpcjiPoDestylacjiWstepujacej);
            Exchange(maxim, 1);
            //Zamien(maxim, 1);
        }

        public void ShowConcordanceSets()
        {
            throw new NotImplementedException();
        }

        public void ShowConcordanceMatrix() {
            // wypisanie CorcordanceMatrix
            Console.WriteLine("MACIERZ ZGODNOSCI");
            for (int y = 0; y < numberOfAlternatives; y++)
            {
                for (int z = 0; z < numberOfAlternatives; z++)
                {
                    Console.Write(concordanceMatrix[y,z] + " | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
            
            //TabComplete(concordanceMatrix, "Macierz Zgod.");
        }

        public void ShowDiscordanceSets()
        {
            throw new NotImplementedException();
        }

        public void ShowOutrankingSets()
        {
            throw new NotImplementedException();
        }

        public void ShowStageFirst()
        {
            throw new NotImplementedException();
        }

        public void ShowStageSecond()
        {
            throw new NotImplementedException();
        }

        public void ShowTableDataOfDistillation(double[,] tabDestillation)
        {
            
        }

        public void CreateOutrankingSets(List<double[,]> listOfOutrankingSets)
        {
            throw new NotImplementedException();
        }

        public void ShowTopDownDiistillation()
        {

        }

        public void ShowUpwardDistillation()
        {

        }
    }
}
