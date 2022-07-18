using System.Collections;

namespace Domino_Engine;

public class Board<T> : IBoard<T> where T : IComparable
{
    public Dictionary<int, int[]> Top_Score { get; private set; }//para verificar el puntaje de cada eqquipo o las rondas ganadas
    public Dictionary<IJugador<T>, List<T>> Not_Pieces { get; private set; }//lista de fichas que no tiene un jugador...
    public Dictionary<int, T> Pieces_In_Board { get; private set; }//para tener los valores de cada Ficha en el tablero...
    public List<IFicha<T>> Pieces_Played { get; private set; }//Cantidad de fichas jugadas...
    public List<IFicha<T>> Posible_Pieces { get; private set; }//todas las fichas posibles...
    public List<IJugador<T>> Jugadors { get; private set; }//Lista de jugadores....
    public Dictionary<int, List<IJugador<T>>> Couples { get; private set; }//cantidad de parejas
    public IRules<T> Reglas { get; private set; }//Rglas del juego...
    int Total;//total de fichas del juego
    T[] Cuantas;//  
    public Board(T[] CuantasFichas, List<IJugador<T>> Jugadores, IRules<T> Rules)
    {
        this.Not_Pieces = new Dictionary<IJugador<T>, List<T>>();
        this.Pieces_Played = new List<IFicha<T>>();//Inicializo las piezas que se han jugado
        this.Pieces_In_Board = new Dictionary<int, T>();//Para saber los tipos que hay en la mesa
        this.Jugadors = Jugadores;//Cargo lo jugadores al tablero
        for (int i = 0; i < Jugadors.Count; i++)
        {
            this.Not_Pieces.Add(Jugadors[i], new List<T>());
        }//Agrego todos los jugadores al diccionario y a cada uno le creo una lista para guardar las fichas en la que se paso
        this.Reglas = Rules;//Cargo las reglas
        this.Posible_Pieces = Reglas.Posible_Pieces.Posible_Pieces(CuantasFichas);//Cargo todas las posibles piezas
        this.Total = Posible_Pieces.Count;//Cantidad de piezas
        this.Couples = Rules.Couples.Couples(Jugadores);//Guardo los tipos de pareja en esta variable
        this.Top_Score = new Dictionary<int, int[]>();
        foreach (var item in Couples)
        {
            this.Top_Score.Add(item.Key, new int[2]);
        }
        this.Cuantas = CuantasFichas;
    }
    public int TotalDeFichas { get => Total; }
    public Iestado<T> Estado { get => new Estados<T>(Reglas, Pieces_In_Board, Pieces_Played, Not_Pieces, Posible_Pieces, Jugadors, Top_Score); }

    public void Repartir()//Para Repartir la Fichas a cada jugador
    {
        if (Reglas.Cantidad <= (Total / Jugadors.Count))//verifico si la cantidad de fichas a cojer por es valida
        {
            for (int i = 0; i < Jugadors.Count; i++)
            {
                Jugadors[i].AddMano(Posible_Pieces, Reglas);//Cada jugador escoge su mano segun un determinado criterio
            }
        }
        else
        {
            throw new Exception($"cantidad de fichas por jugador invalida, maximo {Total / Jugadors.Count} por jugador");
        }
    }
    public IEnumerator<Iestado<T>> Jugar()
    {
        return Reglas.TipeOfGame.Jugar(this.Estado, Jugadors);//que empiece el juego
    }
    public void Reset()//reetea el tablero a sus valores originales
    {
        this.Pieces_Played = new List<IFicha<T>>(); ;
        Not_Pieces = new Dictionary<IJugador<T>, List<T>>();
        Pieces_In_Board = new Dictionary<int, T>();
        for (int i = 0; i < Jugadors.Count; i++)
        {
            Not_Pieces.Add(Jugadors[i], new List<T>());
        }
        Posible_Pieces = Reglas.Posible_Pieces.Posible_Pieces(Cuantas);//Cargo todas las posibles piezas
        Total = Posible_Pieces.Count;//Cantidad de piezas
        Couples = new Dictionary<int, List<IJugador<T>>>();
        for (int i = 0; i < Jugadors.Count; i++)
        {
            Jugadors[i].Hand_Reset();
        }
    }
    public IEnumerator<Iestado<T>> GetEnumerator()
    {
        return this.Jugar();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
public class Jugador<T> : IJugador<T> where T : IComparable
{
    private List<IFicha<T>> Han;//Mano del jugador
    String Name;//Nombre del jugador
    private Istrategy<T> Estrategia;//estrategia del jugador
    public Jugador(String Nombre, Istrategy<T> Strategy)
    {
        Name = Nombre;
        Han = new List<IFicha<T>> { };
        Estrategia = Strategy;
    }
    public int CantFichas { get => Han.Count; }//Para saber cuantas fichas tiene el jugador
    public string Nombre { get => Name; }//Nombre del jugador
    public List<IFicha<T>> Hand { get => Han; }
    public void AddMano(List<IFicha<T>> Posible_Pieces, IRules<T> reglas)
    {
        Han = reglas.Repartir.Repartir(Posible_Pieces, reglas.Cantidad);//El jugador excoge su mano segun el criterio dado
    }
    public void Hand_Reset()
    {
        Hand.Clear();
    }
    public int Valor(IValor_Hand<T> Criterio)
    {
        return Criterio.Valor(Hand);//Devuelve el valor en forma de puntuacion de una mano
    }
    public (int, IFicha<T>) Jugar(Iestado<T> estadoactual)
    {
        return Estrategia.Jugar(estadoactual, Hand);//Para que el jugador juegue
    }
    public bool Contains(Iestado<T> estadoactual)//Verificar si puede teira una ficha
    {
        bool hay_ficha = false;
        foreach (var k in estadoactual.Pieces_In_Board)
        {
            foreach (var item in Hand)
            {
                if ((estadoactual.Reglas.IsValid(k.Key, item, estadoactual)))
                {
                    hay_ficha = true;
                    break;
                }
            }
        }
        return hay_ficha;
    }
    public void AddFicha(List<IFicha<T>> Posible_Pieces)
    {
        if (Posible_Pieces.Count != 0)
        {
            Random ramd = new Random();
            int pos = ramd.Next(Posible_Pieces.Count);
            Hand.Add(Posible_Pieces[pos]);
            Posible_Pieces.Remove(Posible_Pieces[pos]);
        }
        else
        {
            throw new Exception("Deben haber elementos para escoger");
        }
    }
}
public class Ficha<T> : IFicha<T> where T : IComparable
{
    List<T> Valors;//componentes de una ficha de una ficha
    public Ficha(List<T> Valore)
    {
        Valors = Valore;
    }
    public List<T> Valores { get => Valors; }
    public virtual int Valor//Valor total entre los componentes de la ficha
    {
        get
        {
            int value = 0;
            for (int i = 0; i < Valors.Count; i++)
            {
                value += Valors[i].GetHashCode();
            }
            return value;
        }
    }
    public bool IsDoble()//verifica si una ficha es dobl o no
    {
        T k = Valores[0];
        for (int i = 0; i < Valores.Count; i++)
        {
            if (!Valores[i].Equals(k)) return false;
        }
        return true;
    }
}
public class Estados<T> : Iestado<T> where T : IComparable
{
    public Dictionary<int, T> Pieces_In_Board { get; private set; }//para tener los valores de cada Ficha en el tablero...
    public Dictionary<int, List<IFicha<T>>> Pieces { get; private set; }
    public List<IFicha<T>> Pieces_Played { get; private set; }//Cantidad de fichas jugadas...
    public Dictionary<IJugador<T>, List<T>> Not_Pieces { get; private set; }//lista de fichas que no tiene un jugador
    public List<IFicha<T>> Posible_Pieces { get; private set; }//todas las fichas posibles...
    public List<IJugador<T>> Jugadores { get; private set; }//Lista de jugadores en mesa
    private string Progress = String.Empty;//para imprimir cada jugada
    public IRules<T> Reglas { get; private set; }//reglas del juego en momento de la jugada
    public Dictionary<int, int[]> Score { get; set; }

    public Estados(IRules<T> Reglas, Dictionary<int, T> Pieces_In_Board, List<IFicha<T>> Pieces_Played, Dictionary<IJugador<T>, List<T>> Not_Pieces, List<IFicha<T>> Posible_Pieces, List<IJugador<T>> Jugadores, Dictionary<int, int[]> Top_Score)
    {
        this.Pieces_Played = Pieces_Played;
        this.Not_Pieces = Not_Pieces;
        this.Posible_Pieces = Posible_Pieces;
        this.Pieces_In_Board = Pieces_In_Board;
        this.Jugadores = Jugadores;
        this.Reglas = Reglas;
        this.Score = Top_Score;
        this.Pieces = new Dictionary<int, List<IFicha<T>>>();
        foreach (var k in Pieces_In_Board)
        {
            Pieces.Add(k.Key, new List<IFicha<T>>());
        }
    }
    public string Progreso { get => Progress; set => Progress = value; }
    public int CantJugadores => this.Jugadores.Count;
}
public class Manager_Rounds<T> : IManager<T> where T : IComparable
{
    protected Dictionary<int, int[]> Score;
    protected IBoard<T> board;
    protected int[] Rounds_Jug;//para guardar los partidos ganados de cada jugador
    protected int Max_Rounds;
    protected List<IJugador<T>> Jugadores;
    protected IRules<T> Rules;
    protected Iestado<T> estado;
    //private Dictionary<int, int> Top_Score { get; }
    public Manager_Rounds(T[] CuantasFichas, List<IJugador<T>> Jugadores, IRules<T> Rules, int Top_Score)
    {
        board = new Board<T>(CuantasFichas, Jugadores, Rules);//creo un tablero para desarrollar el juego 
        estado = board.Estado;//estado principal
        Rounds_Jug = new int[Jugadores.Count];
        this.Max_Rounds = Top_Score;//maximo de rondas que desea jugar
        this.Jugadores = Jugadores;///listade los jugadores
        this.Rules = Rules;//reglas del manager
        this.Score = board.Top_Score;//guardo en cada posicion los puntos que tiene cada pareja
    }

    public IEnumerator<Iestado<T>> GetEnumerator()
    {
        return this.Jugar();
    }
    public IEnumerator<Iestado<T>> Jugar()
    {
        while (true)
        {
            board.Repartir();
            foreach (var item in board)
            {
                yield return item;
                estado = item;
            }
            if (Hay_Ganadores())
            {
                estado = Ganador(Score);
                break;
            }
            board.Reset();
        }
        yield return estado;
    }
    protected virtual Iestado<T> Ganador(Dictionary<int, int[]> score)
    {
        List<IJugador<T>> list = new List<IJugador<T>>();
        foreach (var item in score)
        {
            if (item.Value[0] == this.Max_Rounds)
            {
                list = estado.Reglas.Couples.Couples(Jugadores)[item.Key];
                break;
            }
        }
        string dev = $"Equipo ganador del juego:";
        for (int i = 0; i < list.Count; i++)
        {
            dev += $" {list[i].Nombre}";
        }
        estado.Progreso = dev;
        return estado;
    }
    protected virtual bool Hay_Ganadores()
    {
        foreach (var item in this.Score)
        {
            if (item.Value[0] >= Max_Rounds)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
public class Manager_Points<T> : Manager_Rounds<T> where T : IComparable
{
    public Manager_Points(T[] CuantasFichas, List<IJugador<T>> Jugadores, IRules<T> Rules, int Max_Point) : base(CuantasFichas, Jugadores, Rules, Max_Point) { }
    protected override Iestado<T> Ganador(Dictionary<int, int[]> score)
    {
        List<IJugador<T>> list = new List<IJugador<T>>();
        foreach (var item in score)
        {
            if (item.Value[1] >= this.Max_Rounds)
            {
                list = estado.Reglas.Couples.Couples(Jugadores)[item.Key];
                break;
            }
        }
        string dev = $"Equipo ganador del juego:";
        for (int i = 0; i < list.Count; i++)
        {
            dev += $" {list[i].Nombre}";
        }
        estado.Progreso = dev;
        return estado;
    }
    protected override bool Hay_Ganadores()
    {
        foreach (var item in this.Score)
        {
            if (item.Value[1] >= Max_Rounds)
            {
                return true;
            }
        }
        return false;
    }
}
