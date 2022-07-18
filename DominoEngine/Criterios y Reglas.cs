namespace Domino_Engine;

//Aki van los diferentes tipos de Criterios 
#region Diferentes Grupos
//Para saber quienes estan en un grupo
public class No_Couples<T> : ICouple<T> where T : IComparable//sin parejas
{
    Dictionary<int, List<IJugador<T>>> ICouple<T>.Couples(List<IJugador<T>> List_Of_Players)
    {
        Dictionary<int, List<IJugador<T>>> Couple = new Dictionary<int, List<IJugador<T>>>();
        for (int i = 0; i < List_Of_Players.Count; i++)
        {
            Couple.Add(i, new List<IJugador<T>>());
            Couple[i].Add(List_Of_Players[i]);
        }
        return Couple;
    }
}
public class Two_in_Two_Couples<T> : ICouple<T> where T : IComparable//parejas de dos en dos
{
    public Dictionary<int, List<IJugador<T>>> Couples(List<IJugador<T>> List_Of_Players)
    {
        if (List_Of_Players.Count % 2 != 0)
        {
            throw new Exception("Para elegir de 2 en 2 debe haber una cantidad par de jugadores");
        }
        Dictionary<int, List<IJugador<T>>> Couple = new Dictionary<int, List<IJugador<T>>>();
        for (int i = 0; i < List_Of_Players.Count / 2; i++)
        {
            int count = Couple.Count;
            Couple.Add(count, new List<IJugador<T>>());
            Couple[count].Add(List_Of_Players[i]);
            Couple[count].Add(List_Of_Players[(List_Of_Players.Count / 2) + i]);
        }
        return Couple;
    }
}
#endregion
#region Diferente valor de las manos 
//Valor de la mano generico, 
public class ValorHand<T> : IValor_Hand<T> where T : IComparable
{
    int IValor_Hand<T>.Valor(List<IFicha<T>> item)//se le pasa la mano a quien desea hallarle su valor
    {
        //valor de la mano de toda la vida
        int a = 0;
        foreach (var t in item)
        {
            a += t.Valor;
        }
        return a;
    }
}
//valor de mano Promedio
public class ValorPromedioHand<T> : IValor_Hand<T> where T : IComparable
{
    int IValor_Hand<T>.Valor(List<IFicha<T>> item)//se le pasa la mano a quien desea hallarle su valor
    {
        //valor de la mano por el promedio
        int a = 0;
        foreach (var t in item)
        {
            a += t.Valor / t.Valores.Count;//Valor promedio de esa Ficha
        }
        return a / item.Count;//Valor promedio entre todos los promedios
    }
}
#endregion
#region Diferentes maneras de generar las piezas
//posibles piezas generando todas la piezaz del domino usual
public class Posible_Pieces<T> : IPosible_Pieces<T> where T : IComparable
{
    List<IFicha<T>> IPosible_Pieces<T>.Posible_Pieces(T[] CuantasFichas)
    {
        bool[] auxiliar = new bool[CuantasFichas.Length];//para ir marcando las Posicionnes que ya visite
        List<IFicha<T>> Posible_Pieces = new List<IFicha<T>>();// lista a devolver
        for (int i = 0; i < CuantasFichas.Length; i++)
        {
            for (int j = 0; j < CuantasFichas.Length; j++)
            {
                if (!auxiliar[j])
                {
                    List<T> list = new List<T>();
                    list.Add(CuantasFichas[i]);
                    list.Add(CuantasFichas[j]);
                    IFicha<T> Ficha = new Ficha<T>(list);
                    Posible_Pieces.Add(Ficha);
                }
            }
            auxiliar[i] = true;
        }
        return Posible_Pieces;
    }
}
//simetrico (si esta el [1,2] => esta [2,1]; si esta el [1,4,2] => esta [2,4,1] )
public class Posible_Pieces_Simetric<T> : IPosible_Pieces<T> where T : IComparable
{
    List<IFicha<T>> IPosible_Pieces<T>.Posible_Pieces(T[] CuantasFichas)
    {
        List<IFicha<T>> Posible_Pieces = new List<IFicha<T>>();//Lista a devolver
        for (int i = 0; i < CuantasFichas.Length; i++)
        {
            for (int j = 0; j < CuantasFichas.Length; j++)
            {
                List<T> list = new List<T>();
                list.Add(CuantasFichas[i]);
                list.Add(CuantasFichas[j]);
                IFicha<T> Ficha = new Ficha<T>(list);
                Posible_Pieces.Add(Ficha);
            }
        }
        return Posible_Pieces;
    }
}
#endregion
#region Diferentes maneras deque un jugador escoja su mano
//repartir aleatorio
public class Repartir_Usual<T> : IRepartir<T> where T : IComparable
{
    List<IFicha<T>> IRepartir<T>.Repartir(List<IFicha<T>> Posible_Pieces, int CantFichaXjugador)
    {
        //Repartir Ramdom
        Random random = new Random();//Creo una instancia ramdom
        List<IFicha<T>> Hand = new List<IFicha<T>> { };//Mano que voy a devolver
        for (int i = 0; i < CantFichaXjugador; i++)
        {
            int x = random.Next(Posible_Pieces.Count);//doy un valor ramdom en tre 0 y el maximo de piezas que hay en la mesa
            Hand.Add(Posible_Pieces.ElementAt(x));//agrego a mi mano esa pieza
            Posible_Pieces.Remove(Posible_Pieces.ElementAt(x));//la remuevo de las posibles piezas a elegir
        }
        return Hand;
        //llenar la mano que escoge cada jugador
    }
}
public class Repartir_Fibonacci<T> : IRepartir<T> where T : IComparable
{
    List<IFicha<T>> IRepartir<T>.Repartir(List<IFicha<T>> Posible_Pieces, int CantFichaXjugador)
    {
        List<int> Fibonacci_Numbres = new List<int>();//Guardo todos los numeros fibonacci menores que las piezas posibles para indexarlos
        int ultimo = 1;
        int penultimo = 0;
        Fibonacci_Numbres.Add(penultimo);
        Fibonacci_Numbres.Add(ultimo);
        while (true)
        {
            int k = ultimo;
            ultimo = ultimo + penultimo;
            penultimo = k;
            if (ultimo < Posible_Pieces.Count)
            {
                Fibonacci_Numbres.Add(ultimo);
            }
            else break;
        }
        List<IFicha<T>> Hand = new List<IFicha<T>> { };//Mano que voy a devolver
        int valor = 0;
        for (int i = 0; i < CantFichaXjugador; i++)
        {
            if (valor < Fibonacci_Numbres.Count)//verifico poder indexar para obtener un numero de la secuencia de fibonacci
            {
                if (Fibonacci_Numbres[valor] < Posible_Pieces.Count)//Verifico poder indexar para obtener un numero de las posibles piezas
                {
                    Hand.Add(Posible_Pieces[Fibonacci_Numbres[valor]]);//Agrego en v-esimo elemento de fibonacci en posibles piezas a la mano
                    Posible_Pieces.Remove(Posible_Pieces[Fibonacci_Numbres[valor]]);//lo remuevo
                    valor++;
                    continue;//paso aanalizar el proceso para la sgte Ficha
                }
                valor = 0;//renuevo el contador y hago el mismo proceso
                Hand.Add(Posible_Pieces[Fibonacci_Numbres[valor]]);
                Posible_Pieces.Remove(Posible_Pieces[Fibonacci_Numbres[valor]]);
                valor++;
            }
            valor = 0;
        }
        return Hand;
        //llenar la mano que escoge cada jugador
    }
}
#endregion
#region Reglas de un juego
//reglas del juego segun los criterios pasados
public class Rules<T> : IRules<T> where T : IComparable
{
    public ITipeOfGame<T> TipeOfGame { get; private set; }//como se va a desarrollar el juego
    public IValor_Hand<T> Criterio { get; private set; }//criterio para dar el valor de las manos de cada jugador
    public IPosible_Pieces<T> Posible_Pieces { get; private set; }//posibles piezas en total sobre el tablero
    public IRepartir<T> Repartir { get; private set; }//de que forma se van a repartir las fichas
    public int Cantidad { get; private set; }//Cantidad de fichas por jugador
    public ICouple<T> Couples { get; private set; }
    public IComparer<int> Comparar { get; private set; }
    public IUnion<T> Compare { get; private set; }
    public Rules(IRepartir<T> Criterio_Repartir, IValor_Hand<T> CriterioHand, int Cantidad_Fichas_x_Jugador, IPosible_Pieces<T> Posible_Pieces, ITipeOfGame<T> TipeOfGame, ICouple<T> Couples, IComparer<int> Comparar, IUnion<T> Compare)
    {
        this.Repartir = Criterio_Repartir;
        this.Criterio = CriterioHand;
        this.Cantidad = Cantidad_Fichas_x_Jugador;
        this.Posible_Pieces = Posible_Pieces;
        this.TipeOfGame = TipeOfGame;
        this.Couples = Couples;
        this.Comparar = Comparar;
        this.Compare = Compare;
    }
    public Iestado<T> Actualizar_Board(int Posicionn, IFicha<T> Ficha, Iestado<T> estado)//actualizar el tablero
    {
        if (estado.Pieces_In_Board.Count != 0)//Tablero no vacio
        {
            estado.Pieces[Posicionn].Add(Ficha);
            T old = estado.Pieces_In_Board[Posicionn];
            List<T> newPieces = new List<T>();
            for (int i = 0; i < Ficha.Valores.Count; i++)
            {
                newPieces.Add(Ficha.Valores[i]);//paso una copia de la Ficha
            }
            foreach (var item in newPieces)
            {
                if (estado.Reglas.Compare.Comparar(old, item))
                {
                    newPieces.Remove(item);//remuevo de la copia el valor x el q voy a jugar
                    break;
                }
            }
            //newPieces.Remove(old);//remuevo de la copia el valor x el q voy a jugar
            estado.Pieces_In_Board[Posicionn] = newPieces[newPieces.Count - 1];
            newPieces.Remove(newPieces[newPieces.Count - 1]);//elimino la Posicionn q ya agrege para despues agregar las restantes al tablero
            for (int i = 0; i < newPieces.Count; i++)
            {
                estado.Pieces_In_Board.Add(estado.Pieces_In_Board.Count, newPieces[i]);
            }
        }
        else//Tablero vacio
        {

            for (int i = 0; i < Ficha.Valores.Count; i++)//Juego la Ficha que me tiran
            {
                int num = estado.Pieces_In_Board.Count;
                estado.Pieces_In_Board.Add(num, Ficha.Valores[i]);
                estado.Pieces.Add(num, new List<IFicha<T>> { });//agrego una lista vacia para las fichas que me tiran
            }
        }
        return estado;
    }
    public (IJugador<T>, List<IJugador<T>>) Winner(List<IJugador<T>> jugadors)//Para si se tranca el juego
    {

        List<int> valors = new List<int>();//lista de valores por jugador
        List<int> Posicionnes = new List<int>();//Posicionnes de los jugadores en el tablero
        for (int i = 0; i < jugadors.Count; i++)
        {
            valors.Add(jugadors[i].Valor(this.Criterio));
            Posicionnes.Add(i);
        }
        for (int i = 0; i < jugadors.Count - 1; i++)//de menor a mayor
        {
            for (int j = i + 1; j < jugadors.Count; j++)
            {
                if (this.Comparar.Compare(valors[i], valors[j]) > 0)
                {
                    int temp0 = valors[i];
                    int temp1 = Posicionnes[i];
                    valors[i] = valors[j];
                    Posicionnes[i] = Posicionnes[j];
                    valors[j] = temp0;
                    Posicionnes[j] = temp1;
                }
            }
        }
        IJugador<T> Ganador = jugadors[Posicionnes[0]];
        Dictionary<int, List<IJugador<T>>> Couple = this.Couples.Couples(jugadors);
        List<IJugador<T>> Ganadores = new List<IJugador<T>>();
        foreach (var item in Couple)
        {
            if (item.Value.Contains(Ganador))
            {
                Ganadores = item.Value;
            }
        }
        return (Ganador, Ganadores);
    }
    public (IJugador<T>, List<IJugador<T>>) Winner(IJugador<T> jugador, List<IJugador<T>> Jugadores)//para devolver a el ganador junto a su equipo
    {
        List<IJugador<T>> Ganadores = new List<IJugador<T>>();
        foreach (var item in Couples.Couples(Jugadores))
        {
            if (item.Value.Contains(jugador))
            {
                Ganadores = item.Value;
                break;
            }
        }
        return (jugador, Ganadores);
    }
    public Iestado<T> Not_Pieces(IJugador<T> jugador, Iestado<T> estado)//para cuando un jugador se pasa
    {
        foreach (var item in estado.Pieces_In_Board)
        {
            if (!estado.Not_Pieces[jugador].Contains(item.Value))
            {
                estado.Not_Pieces[jugador].Add(item.Value);
            }
        }
        return estado;
    }
    public bool IsValid(int Posicionn, IFicha<T> Ficha, Iestado<T> estado)
    {
        if (estado.Pieces_In_Board.Count == 0)
        {
            return !(Ficha == null);
        }
        if (estado.Pieces_In_Board.ContainsKey(Posicionn))//verific0 si en el tablero esta la Posicionn por la que quiere tirar
        {
            foreach (var item in Ficha.Valores)
            {
                if (estado.Reglas.Compare.Comparar(estado.Pieces_In_Board[Posicionn], item))
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    public Dictionary<int, int[]> Max_Rounds(IJugador<T> ganador, List<IJugador<T>> Jugadores, Iestado<T> estado)//Para tener la cuenta de los partidos ganados y los puntos que tiene cada team
    {
        for (int i = 0; i < estado.Reglas.Couples.Couples(Jugadores).Count; i++)
        {

            if (estado.Reglas.Couples.Couples(Jugadores)[i].Contains(ganador))
            {
                estado.Score[i][0] += 1;
                estado.Score[i][1] += Sumar(ganador, Jugadores, estado);
            }
        }
        return estado.Score;
    }
    private int Sumar(IJugador<T> ganador, List<IJugador<T>> Jugadores, Iestado<T> estado)
    {
        int count = 0;
        foreach (var item in estado.Reglas.Couples.Couples(Jugadores))
        {
            if (!item.Value.Contains(ganador))
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    count += item.Value[i].Valor(estado.Reglas.Criterio);
                }
            }
        }
        return count;
    }
}
public class Comparer_Usually : IComparer<int>//Para especificar que ficha tiene mas valor que otra
{
    int IComparer<int>.Compare(int x, int y)
    {
        if (x.Equals(y)) return 0;
        if (x < y) return -1;
        return 1;
    }
}
public class Comparer_Deluxe : IComparer<int>
{
    int IComparer<int>.Compare(int x, int y)
    {
        if (x.Equals(y)) return 0;
        if (x < y) return 1;
        return -1;
    }
}
public class Usually<T> : IUnion<T> where T : IComparable//Union de una ficha con otra usual
{
    public bool Comparar(T T1, T T2)
    {
        return (object.Equals(T1, T2));
    }
}
public class Par_Impar<T> : IUnion<T> where T : IComparable
{
    public bool Comparar(T T1, T T2)
    {
        if (T1.GetHashCode() % 2 == 0)
        {
            return (T2.GetHashCode() % 2 == 0);
        }
        return (T2.GetHashCode() % 2 != 0);
    }
}

#endregion
#region Diferentes Tipos de juegos
//Juego clasico para cualquier cantidad de Fichas hasta doble 9, doble 6, doble 20
public class Clasic_Game<T> : ITipeOfGame<T> where T : IComparable
{
    public virtual IEnumerator<Iestado<T>> Jugar(Iestado<T> estado, List<IJugador<T>> Jugadores)
    {
        Iestado<T> estado_actual = estado;
        bool Hay_Ganador = false;
        int pases = 0;
        System.Console.WriteLine("Juego nuevo");
        while (!Hay_Ganador)
        {
            for (int i = 0; i < Jugadores.Count; i++)
            {
                if (estado_actual.Pieces_In_Board.Count == 0)
                {
                    pases = 0;
                    yield return Estado(Jugadores[i], estado_actual.Reglas, estado_actual);
                }
                else if (Jugadores[i].Contains(estado_actual))
                {
                    pases = 0;
                    yield return Estado(Jugadores[i], estado_actual.Reglas, estado_actual);
                    if (Jugadores[i].CantFichas == 0)
                    {
                        Hay_Ganador = true;
                        yield return Estado_Final(Jugadores[i], Jugadores, estado_actual.Reglas, estado_actual);
                        yield break;
                    }
                }
                else
                {
                    pases++;
                    yield return Estado_Pase(Jugadores[i], estado_actual.Reglas, estado_actual);
                    if (pases == Jugadores.Count)
                    {
                        yield return Estado_Pase_Final(Jugadores[i], Jugadores, estado_actual.Reglas, estado_actual);
                        yield break;
                    }
                }
            }
        }
    }
    protected Iestado<T> Estado_Pase_Final(IJugador<T> jugador, List<IJugador<T>> Jugadores, IRules<T> reglas, Iestado<T> estado_actual)
    {
        (IJugador<T> ganador, List<IJugador<T>> equipo) = reglas.Winner(Jugadores);
        estado_actual.Progreso = $"Se tranco!! {ganador.Nombre} ha ganado esta ronda, equipo ganador: {Win(equipo)}";
        estado_actual.Score = reglas.Max_Rounds(ganador, Jugadores, estado_actual);
        return estado_actual;
    }
    protected Iestado<T> Estado_Pase(IJugador<T> jugador, IRules<T> reglas, Iestado<T> estado_actual)
    {
        estado_actual = reglas.Not_Pieces(jugador, estado_actual);
        estado_actual.Progreso = $"{jugador.Nombre} se ha pasado";
        return estado_actual;
    }
    protected Iestado<T> Estado(IJugador<T> Jugadors, IRules<T> Reglas, Iestado<T> estado_actual)
    {
        (int k, IFicha<T> Ficha) = Jugadors.Jugar(estado_actual);
        if (Reglas.IsValid(k, Ficha, estado_actual))
        {
            estado_actual = Reglas.Actualizar_Board(k, Ficha, estado_actual);
            estado_actual.Pieces_Played.Add(Ficha);
            estado_actual.Progreso = $"{Jugadors.Nombre} ha jugado la ficha [{String.Join(" ", Ficha.Valores)}]";
            return estado_actual;
        }
        estado_actual.Progreso = $"Jugada Invalida";
        return Estado(Jugadors, Reglas, estado_actual);
    }
    protected Iestado<T> Estado_Final(IJugador<T> jugador, List<IJugador<T>> Jugadores, IRules<T> reglas, Iestado<T> estado_actual)
    {
        (IJugador<T> ganador, List<IJugador<T>> equipo) = reglas.Winner(jugador, Jugadores);
        estado_actual.Progreso = $"Se ha pegado {ganador.Nombre}, equipo ganador de la ronda: {Win(equipo)}";
        estado_actual.Score = reglas.Max_Rounds(ganador, Jugadores, estado_actual);
        return estado_actual;
    }
    protected string Win(List<IJugador<T>> equipo)
    {
        string equip = string.Empty;
        for (int i = 0; i < equipo.Count; i++)
        {
            equip += $"{equipo[i].Nombre} ";
        }
        return equip;
    }
}
public class Deluxe_Game<T> : Clasic_Game<T>, ITipeOfGame<T> where T : IComparable //este todavia no esta terminado me faltan varias cosas por acoplar
{
    public override IEnumerator<Iestado<T>> Jugar(Iestado<T> estado, List<IJugador<T>> Jugadores)
    {
        List<IFicha<T>> Graveyard = estado.Posible_Pieces;
        int pases = 0;
        Iestado<T> estado_actual = estado;
        bool Hay_Ganador = false;
        while (!Hay_Ganador)
        {
            for (int i = 0; i < Jugadores.Count; i++)
            {
                if (estado.Pieces_In_Board.Count == 0)
                {
                    pases = 0;
                    yield return Estado(Jugadores[i], estado.Reglas, estado_actual);
                }
                else if (Jugadores[i].Contains(estado_actual))
                {
                    pases = 0;
                    yield return Estado(Jugadores[i], estado.Reglas, estado_actual);
                    if (Jugadores[i].CantFichas == 0)
                    {
                        yield return Estado_Final(Jugadores[i], Jugadores, estado.Reglas, estado_actual);
                        yield break;
                    }
                }
                else
                {
                    bool Ha_Jugado = false;
                    while (!Ha_Jugado)
                    {
                        if (Graveyard.Count != 0)
                        {
                            System.Console.WriteLine($"Pase: {Jugadores[i].Nombre} robara!!");
                            Jugadores[i].AddFicha(Graveyard);
                            if (Jugadores[i].Contains(estado_actual))
                            {
                                Ha_Jugado = true;///
                                yield return Estado(Jugadores[i], estado.Reglas, estado_actual);
                                //break;
                            }
                        }
                        else
                        {
                            Ha_Jugado = true;
                            pases++;
                            yield return Estado_Pase(Jugadores[i], estado.Reglas, estado_actual);
                            if (pases == Jugadores.Count)
                            {
                                yield return Estado_Pase_Final(Jugadores[i], Jugadores, estado.Reglas, estado_actual);
                                yield break;
                            }
                        }
                    }
                }
            }
        }
    }
}
#endregion
