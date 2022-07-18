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
}
