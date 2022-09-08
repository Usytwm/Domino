using System.Globalization;
using Domino_Engine;

namespace Domino_Server.Data;

public class Params
{
    //Propiedades de las paginas
    public int Mode { get; set; }
    public List<int>? _Numbers;
    public int _NumbOP { get; set; }
    public List<Istrategy<int>> _PlayerType { get; set; } = new List<Istrategy<int>>{};
    public static ICouple<int>? _Teams { get; set; }
    public static IRepartir<int>? _Repart { get; set; }
    public string? _VictoryP { get; set; }
    public static IUnion<int>? _UnionType { get; set; }
    public static IComparer<int>? _Comparer { get; set; }
    public static IValor_Hand<int>? _ValorHandType { get; set; }
    public static IPosible_Pieces<int>? _Pieces { get; set; }
    public static ITipeOfGame<int>? _GameType { get; set; }


    public string[] Game_Modes { get; set; } = { "Domino Habanero (doble 9)", "Domino Oriental (doble 6)", "Otro criterio" };
    public int[] NumberOfPlayers { get; set; } = { 2, 4 };
    public string[] VictoryParams { get; set; } = { 
        "De 3 a ganar 2", "De 5 a ganar 3", "De 8 a ganar 5", "A 100 puntos", "A 200 puntos", "A 500 puntos",
        "A 1000 puntos", "All night long", "Otro criterio"
    };
    public string[] PlayersT { get; set; } = { "Botagorda", "Pro-Player", "Random-Player" };
    public string[] Bots { get; set; } = { "Brian_Bot", "Camiso_Bot", "Dariel_Bot", "Amalia_Bot", "Henry_Bot", "Tony_Bot", 
        "Piad_Bot", "Kathy_Bot", "Amy_Bot", "Ginovart_Bot", "Celia_Bot" };
    public string[] Teams { get; set; } = { "Sin parejas", "Parejas de 2", "Otro criterio" };
    public string[] Repart { get; set; } = { "Aleatorio", "Fibonacci", "Otro criterio" };
    public string[] Union { get; set; } = { "Usual", "Segun Paridad" };
    public string[] Comparer { get; set; } = { "Usual", "El menor valor ahora es el mayor" };
    public string[] Valor_Hand { get; set; } = { "Usual", "Valor promedio" };
    public string[] Pieces { get; set; } = { "Usual", "Duplicar piezas" };
    public string[] GameType { get; set; } = { "Clasica", "Deluxe" };


    public void _Gamemode(string gm){
        if (gm == "0"){
            Mode = 10;
            _Numbers = new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }
        else if (gm == "1"){
            Mode = 7;
            _Numbers = new List<int>{ 0, 1, 2, 3, 4, 5, 6 };
        }
    }
    public void _NumberOfPlayers(string numb){
        if (numb == "0") _NumbOP = 2;
        else if (numb == "1") _NumbOP = 4;
    }
    public void _PlayersT(string type){
        if (type == "0") _PlayerType.Add(new Botagorda<int>());
        else if (type == "1") _PlayerType.Add(new Pro_Player<int>());
        else if (type == "2") _PlayerType.Add(new Random_Player<int>());
    }
    public void _MyTeams(string tm){
        if (tm == "0") _Teams = new No_Couples<int>();
        else if (tm == "1") _Teams = new Two_in_Two_Couples<int>();
    }
    public void _MyRepart(string rp){
        if (rp == "0") _Repart = new Repartir_Usual<int>();
        else if (rp == "1") _Repart = new Repartir_Fibonacci<int>();
    }
    public void _PuntRules(string punt){
        if (punt == "0") _VictoryP = "3";
        else if (punt == "1") _VictoryP = "5";
        else if (punt == "2") _VictoryP = "8";
        else if (punt == "7") _VictoryP = $"{int.MaxValue}";
    }
    public void _Union(string union){
        if (union == "0") _UnionType = new Usually<int>();
        else if (union == "1") _UnionType = new Par_Impar<int>();
    }
    public void _MyComparer(string comp){
        if (comp == "0") _Comparer = new Comparer_Usually();
        else if (comp == "1") _Comparer = new Comparer_Deluxe();
    }
    public void _ValorHand(string value){
        if (value == "0") _ValorHandType = new ValorHand<int>();
        else if (value == "1") _ValorHandType = new ValorPromedioHand<int>();
    }
    public void _PossiblePieces(string pos){
        if (pos == "0") _Pieces = new Posible_Pieces<int>();
        else if (pos == "1") _Pieces = new Posible_Pieces_Simetric<int>();
    }
    public void _TypeOfGame(string type){
        if (type == "0") _GameType = new Clasic_Game<int>();
        else if (type == "1") _GameType = new Deluxe_Game<int>();
    }
}
