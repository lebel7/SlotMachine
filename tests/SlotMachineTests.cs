using SlotMachineGame;

namespace SlotMachineTests;


[TestClass]
public class SlotMachineTests
{
    [TestMethod]
    public void TestGenerateSymbol()
    {
        SlotMachine slotMachine = new();
        char symbol = slotMachine.GenerateSymbol();
        Assert.IsNotNull(symbol);
    }

    [TestMethod]
    public void TestCalculateWin()
    {
        SlotMachine slotMachine = new();
        double winAmount = slotMachine.CalculateWin("AAA", 1.0);
        Assert.AreEqual(0.4, winAmount);

        winAmount = slotMachine.CalculateWin("AB*", 1.0);
        Assert.AreEqual(1.0, winAmount);
    }

    [TestMethod]
    public void TestIsWinningLine()
    {
        _ = new SlotMachine();
        bool isWinning = SlotMachine.IsWinningLine("AAA");
        Assert.IsTrue(isWinning);

        isWinning = SlotMachine.IsWinningLine("AB*");
        Assert.IsFalse(isWinning);
    }
}

