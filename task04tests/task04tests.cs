using task04;
namespace task04tests;

using Xunit;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();

        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(20, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void MoveForwardCheck_ShouldTakeCorrectCoordinates()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        fighter.MoveForward();
        fighter.MoveForward();
        cruiser.MoveForward();

        Assert.Equal(200, fighter.Coordinates);
        Assert.Equal(50, cruiser.Coordinates);
    }

    [Fact]
    public void RotateCheck_ShouldTakeCorrectAngle()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        fighter.Rotate(370);
        cruiser.Rotate(30);

        Assert.Equal(10, fighter.Angle);
        Assert.Equal(30, cruiser.Angle);
    }

    [Fact]
    public void FireCheck_ShouldTakeCorrectValue()
    {
        var cruiser = new Cruiser();

        for (int i=10-1; i>=0; i--)
        {
            cruiser.Fire();
            Assert.Equal(i, cruiser.Missles);
        }
    }
}
