namespace Domino_Engine;

public class Random_Player<T> : Istrategy<T> where T : IComparable
{
    public (int, IFicha<T>) Jugar(Iestado<T> estadoactual, List<IFicha<T>> Hand)
    {
        if (Hand.Count != 0)
        {
            Random random = new Random();
            List<IFicha<T>> Posibles_Tiros = new List<IFicha<T>> { };
            List<int> Posibles_Lugares = new List<int> { };//para tener los posibles lugares x donde se puede lanzar una ficha
            if (estadoactual.Pieces_In_Board.Count == 0)
            {
                IFicha<T> dev = Hand[random.Next(Hand.Count)];
                Hand.Remove(dev);
                return (1, dev);
            }
            foreach (var k in estadoactual.Pieces_In_Board)//para agregar todas las posibles fichas que puedo tirar
            {
                foreach (var item in Hand)
                {
                    if (estadoactual.Reglas.IsValid(k.Key, item, estadoactual) && !Posibles_Tiros.Contains(item))
                    {
                        Posibles_Tiros.Add(item);
                    }
                }
            }
            IFicha<T> Ficha_a_Devolver = Posibles_Tiros[random.Next(Posibles_Tiros.Count)];
            Hand.Remove(Ficha_a_Devolver);
            for (int i = 0; i < estadoactual.Pieces_In_Board.Count; i++)
            {
                if (estadoactual.Reglas.IsValid(i, Ficha_a_Devolver, estadoactual))
                {
                    Posibles_Lugares.Add(i);//agrego una posicion posible por la que tirar
                }
            }
            return (Posibles_Lugares[random.Next(Posibles_Lugares.Count)], Ficha_a_Devolver);
        }
        throw new Exception("No existen fichas en la mano");
    }
}
public class Botagorda<T> : Istrategy<T> where T : IComparable
{
    protected bool organizado = false;
    public (int, IFicha<T>) Jugar(Iestado<T> estadoactual, List<IFicha<T>> Hand)
    {
        if (Hand.Count != 0)
        {
            if (!organizado)//verifico si ya organice mis fichas de mayor a menor
            {
                for (int i = 0; i < Hand.Count - 1; i++)
                {
                    for (int j = i + 1; j < Hand.Count; j++)
                    {
                        if (estadoactual.Reglas.Comparar.Compare(Hand[i].Valor, Hand[j].Valor) < 0)///menor valor
                        {
                            IFicha<T> tmp = Hand[i];
                            Hand[i] = Hand[j];
                            Hand[j] = tmp;
                        }
                    }
                }
                organizado = true;
            }

            if (estadoactual.Pieces_In_Board.Count == 0)//cuando no hay ninguna ficha en el tablero
            {
                IFicha<T> dev = Hand[0];
                Hand.Remove(dev);
                return (2, dev);
            }
            List<IFicha<T>> Posibles_Tiros = new List<IFicha<T>> { };
            foreach (var k in estadoactual.Pieces_In_Board)//para agregar todas las posibles fichas que puedo tirar
            {
                foreach (var item in Hand)
                {
                    if (estadoactual.Reglas.IsValid(k.Key, item, estadoactual) && !Posibles_Tiros.Contains(item))
                    {
                        Posibles_Tiros.Add(item);
                    }
                }
            }

            for (int i = 0; i < Posibles_Tiros.Count - 1; i++)///aki
            {
                for (int j = i + 1; j < Posibles_Tiros.Count; j++)
                {
                    if (estadoactual.Reglas.Comparar.Compare(Posibles_Tiros[i].Valor, Posibles_Tiros[j].Valor) < 0)
                    {
                        IFicha<T> temp = Posibles_Tiros[i];
                        Posibles_Tiros[i] = Posibles_Tiros[j];
                        Posibles_Tiros[j] = temp;
                    }
                }
            }
            IFicha<T> devolver = Posibles_Tiros[0];
            int num = 0;
            foreach (var item in estadoactual.Pieces_In_Board)
            {
                if (estadoactual.Reglas.IsValid(item.Key, devolver, estadoactual))
                {
                    num = item.Key;//agrego una posicion por la que tirar
                    break;
                }
            }
            Hand.Remove(devolver);
            return (num, devolver);
        }
        throw new Exception("No existen fichas en la mano");
    }
}
public class Pro_Player<T> : Istrategy<T> where T : IComparable
{
    private (int, IFicha<T>) Jugador_Random(Iestado<T> estadoactual, List<IFicha<T>> Hand)
    {
        if (Hand.Count != 0)
        {
            Random random = new Random();
            List<IFicha<T>> Posibles_Tiros = new List<IFicha<T>> { };
            List<int> Posibles_Lugares = new List<int> { };//para tener los posibles lugares x donde se puede lanzar una ficha
            if (estadoactual.Pieces_In_Board.Count == 0)
            {
                IFicha<T> dev = Hand[random.Next(Hand.Count)];
                Hand.Remove(dev);
                return (1, dev);
            }
            foreach (var k in estadoactual.Pieces_In_Board)//para agregar todas las posibles fichas que puedo tirar
            {
                foreach (var item in Hand)
                {
                    if (estadoactual.Reglas.IsValid(k.Key, item, estadoactual) && !Posibles_Tiros.Contains(item))
                    {
                        Posibles_Tiros.Add(item);
                    }
                }
            }
            IFicha<T> Ficha_a_Devolver = Posibles_Tiros[random.Next(Posibles_Tiros.Count)];
            Hand.Remove(Ficha_a_Devolver);
            for (int i = 0; i < estadoactual.Pieces_In_Board.Count; i++)
            {
                if (estadoactual.Reglas.IsValid(i, Ficha_a_Devolver, estadoactual))
                {
                    Posibles_Lugares.Add(i);//agrego una posicion posible por la que tirar
                }
            }
            return (Posibles_Lugares[random.Next(Posibles_Lugares.Count)], Ficha_a_Devolver);
        }
        throw new Exception("No existen fichas en la mano");
    }

    private (int, IFicha<T>) Jugador_Botagorda(Iestado<T> estadoactual, List<IFicha<T>> Hand)
    {
        bool organizado = false;
        if (Hand.Count != 0)//aki
        {
            if (!organizado)//verifico si ya organice mis fichas de mayor a menor
            {
                for (int i = 0; i < Hand.Count - 1; i++)
                {
                    for (int j = i + 1; j < Hand.Count; j++)
                    {
                        if (estadoactual.Reglas.Comparar.Compare(Hand[i].Valor, Hand[j].Valor) < 0)///menor valor
                        {
                            IFicha<T> tmp = Hand[i];
                            Hand[i] = Hand[j];
                            Hand[j] = tmp;
                        }
                    }
                }
                organizado = true;
            }

            if (estadoactual.Pieces_In_Board.Count == 0)//cuando no hay ninguna ficha en el tablero
            {
                IFicha<T> dev = Hand[0];
                Hand.Remove(dev);
                return (2, dev);
            }
            List<IFicha<T>> Posibles_Tiros = new List<IFicha<T>> { };
            foreach (var k in estadoactual.Pieces_In_Board)//para agregar todas las posibles fichas que puedo tirar
            {
                foreach (var item in Hand)
                {
                    if (estadoactual.Reglas.IsValid(k.Key, item, estadoactual) && !Posibles_Tiros.Contains(item))
                    {
                        Posibles_Tiros.Add(item);
                    }
                }
            }

            for (int i = 0; i < Posibles_Tiros.Count - 1; i++)///aki
            {
                for (int j = i + 1; j < Posibles_Tiros.Count; j++)
                {
                    if (estadoactual.Reglas.Comparar.Compare(Posibles_Tiros[i].Valor, Posibles_Tiros[j].Valor) < 0)
                    {
                        IFicha<T> temp = Posibles_Tiros[i];
                        Posibles_Tiros[i] = Posibles_Tiros[j];
                        Posibles_Tiros[j] = temp;
                    }
                }
            }
            IFicha<T> devolver = Posibles_Tiros[0];
            int num = 0;
            foreach (var item in estadoactual.Pieces_In_Board)
            {
                if (estadoactual.Reglas.IsValid(item.Key, devolver, estadoactual))
                {
                    num = item.Key;//agrego una posicion por la que tirar
                    break;
                }
            }
            Hand.Remove(devolver);
            return (num, devolver);
        }
        throw new Exception("No existen fichas en la mano");
    }
    public (int, IFicha<T>) Jugar(Iestado<T> estadoactual, List<IFicha<T>> Hand)
    {
        Random ramdom = new Random();
        int num = ramdom.Next(10);
        if (num % 2 == 0)
        {
            return Jugador_Random(estadoactual, Hand);
        }
        return Jugador_Botagorda(estadoactual, Hand);
    }
}