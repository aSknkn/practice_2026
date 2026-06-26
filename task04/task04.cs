using System;

namespace task04;

public interface ISpaceship
{
    void MoveForward();
    void Rotate(int angle);
    void Fire();
    int Speed { get; }
    int FirePower { get; }
    // решено добавить свойства для реализации методов:
    // MoveForward, Rotate, Fire
    int Coordinates { get; set; }
    int Angle { get; set; }
    int Missles { get; set; }
}

// абстрактный класс создан для избежания дублирования методов для конкретных кораблей
public abstract class Spaceship : ISpaceship
{
    public abstract int Speed { get; }
    public abstract int FirePower { get; }

    public int Coordinates { get; set; } = 0;
    public int Angle { get; set; } = 0;
    public int Missles { get; set; }

    public void MoveForward()
    {
        Coordinates += Speed;
    }

    public void Rotate(int angle)
    {
        Angle = (Angle + angle) % 360;
    }

    public void Fire()
    {
        if (Missles > 0)
        {
            Missles--;
        }
    }
}
public class Cruiser : Spaceship
{
    public override int Speed => 50;
    public override int FirePower => 100;

    public Cruiser()
    {
        Missles= 10;
    }
}
public class Fighter : Spaceship
{
    public override int Speed => 100;
    public override int FirePower => 20;

    public Fighter()
    {
        Missles = 20;
    }
}