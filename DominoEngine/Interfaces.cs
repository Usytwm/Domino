namespace Domino_Engine;

public interface IFicha<T> where T : IComparable
{
    List<T> Valores { get; }//para almacenar los tipos de ficha
    bool IsDoble();//para ver si una ficha es doble
    int Valor { get; }//Cada ficha viene con un valor
}
public interface IJugador<T> where T : IComparable
{
    public int CantFichas { get; }//Cantidad de fichas que tiene en la mano..
    public string Nombre { get; }//Nombre del jugador...
    bool Contains(Iestado<T> estadoactual);
    public List<IFicha<T>> Hand { get; }//fichas de un jugador
    int Valor(IValor_Hand<T> Criterio);//total de puntos que tiene en la manos
    void AddMano(List<IFicha<T>> Posible_Pieces, IRules<T> reglas);//coger la mano de count piezas de las Posibles Fichas
    void AddFicha(List<IFicha<T>> Posible_Pieces);//agrega una ficha a su mano
    public (int, IFicha<T>) Jugar(Iestado<T> estadoactual);//Segun el estado del tablero devuelve una ficha y la posicion por donde tirarla en la mesa
    public void Hand_Reset();
}
public interface IBoard<T> : IEnumerable<Iestado<T>> where T : IComparable
{
    public Dictionary<int, int[]> Top_Score { get; }//tupla de rondas ganadas y puntage
    public Iestado<T> Estado { get; }
    public int TotalDeFichas { get; }//cant de fichas en total 
    public List<IFicha<T>> Pieces_Played { get; }//piezas jugadas
    public void Repartir();//Hora de que los jugadores cojan sus fichas
    public IEnumerator<Iestado<T>> Jugar();//Para que inicie el juego
    public void Reset();//
}
public interface ITipeOfGame<T> where T : IComparable
{
    public IEnumerator<Iestado<T>> Jugar(Iestado<T> estado, List<IJugador<T>> Jugadores);
}
public interface ICouple<T> where T : IComparable
{
    public Dictionary<int, List<IJugador<T>>> Couples(List<IJugador<T>> List_Of_Players);
}
public interface Istrategy<T> where T : IComparable
{
    (int, IFicha<T>) Jugar(Iestado<T> estadoactual, List<IFicha<T>> Hand);
}
public interface IPosible_Pieces<T> where T : IComparable
{
    List<IFicha<T>> Posible_Pieces(T[] CuantasFichas);//para generar todas las posibles piezas del tablero
}
public interface IRules<T> where T : IComparable
{
    ITipeOfGame<T> TipeOfGame { get; }
    IValor_Hand<T> Criterio { get; }
    IPosible_Pieces<T> Posible_Pieces { get; }
    IRepartir<T> Repartir { get; }
    ICouple<T> Couples { get; }
    IComparer<int> Comparar { get; }
    IUnion<T> Compare { get; }
    int Cantidad { get; }//Cantidad de fichas por jugador(propiedad contructor)
    bool IsValid(int Posicionn, IFicha<T> Ficha, Iestado<T> estado);
    (IJugador<T>, List<IJugador<T>>) Winner(List<IJugador<T>> jugadors);//Para si se tranca el juego, devuelve un ganador y la lista de ganadores
    Iestado<T> Actualizar_Board(int posicion, IFicha<T> ficha, Iestado<T> estado);//actualizar el tablero
    (IJugador<T> Ganador, List<IJugador<T>> Ganadores) Winner(IJugador<T> jugador, List<IJugador<T>> Jugadores);//Para verificar cuando se pega un jugador quie en es el equipo ganador
    Iestado<T> Not_Pieces(IJugador<T> jugador, Iestado<T> estado);
    Dictionary<int, int[]> Max_Rounds(IJugador<T> ganador, List<IJugador<T>> Jugadores, Iestado<T> estado);
}
public interface IIs_Valid<T> where T : IComparable
{
    public bool IsValid(int Posicionn, IFicha<T> Ficha, Iestado<T> estado);//verificar si una Posicionn es valida
}
public interface IManager<T> : IEnumerable<Iestado<T>> where T : IComparable
{
    public IEnumerator<Iestado<T>> Jugar();
}
public interface IRepartir<T> where T : IComparable
{
    List<IFicha<T>> Repartir(List<IFicha<T>> Posible_Pieces, int CantFichaXjugador);//Para repartir las fichas a cada jugador segun las piezas q hay
}
public interface IValor_Hand<T> where T : IComparable
{
    int Valor(List<IFicha<T>> item);//determina el valor de una mano
}
public interface Iestado<T> where T : IComparable
{
    public List<IFicha<T>> Pieces_Played { get; }//piezas jugadas
    public Dictionary<IJugador<T>, List<T>> Not_Pieces { get; }//piezas que no tiene un jugador
    public List<IFicha<T>> Posible_Pieces { get; } //posibles piezas para el juego
    public Dictionary<int, List<IFicha<T>>> Pieces { get; }
    public Dictionary<int, T> Pieces_In_Board { get; } //tipos de valores en mesa
    public string Progreso { get; set; }//imprime cada jugada que se realiza
    public int CantJugadores { get; }//jugadores que estan en juego
    Dictionary<int, int[]> Score { get; set; }
    IRules<T> Reglas { get; }//reglas 
}
public interface IUnion<T> where T : IComparable
{
    bool Comparar(T T1, T T2);
}