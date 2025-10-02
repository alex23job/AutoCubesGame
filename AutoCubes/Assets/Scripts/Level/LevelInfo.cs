using System;
using System.Collections.Generic;

[Serializable]
public class LevelInfo
{
    private static List<LevelInfo> _levels = new List<LevelInfo>();
    private static int _nextLevelNumber = 1;
    public static LevelInfo GetLevel(int index)
    {
        if (_levels.Count == 0) CreateLevels();
        if ((index >= 0) && (index < _levels.Count)) return _levels[index];
        else return _levels[0];
    }

    private static void CreateLevels()
    {
        _levels.Clear();
        _nextLevelNumber = 1;
        _levels.Add(new LevelInfo(5, 30));
        _levels.Add(new LevelInfo(8, 50));
        _levels.Add(new LevelInfo(10, 70));
        _levels.Add(new LevelInfo(5, 40, true));
        _levels.Add(new LevelInfo(8, 60, true));
        _levels.Add(new LevelInfo(10, 70, true));
        _levels.Add(new LevelInfo(12, 80, true));
        _levels.Add(new LevelInfo(15, 100, true));
        _levels.Add(new LevelInfo(15, 120, true));
        _levels.Add(new LevelInfo(20, 150, true));
    }

    private int levelNumber;
    private int maxCars;
    private int maxOrders;
    private int countCars = 0;
    private int countOrders = 0;
    private int countMany = 0;
    private int countExp = 0;
    private bool isMarkering = false;

    public bool IsMarkering { get => isMarkering; }
    public int Exp { get => countExp; }
    public int Many { get => countMany; }

    public int LevelNumber { get => levelNumber; }

    public LevelInfo() { }
    public LevelInfo(int maxCars, int maxOrders, bool isMarkering = false)
    {
        levelNumber = LevelInfo._nextLevelNumber++;
        this.maxCars = maxCars;
        this.maxOrders = maxOrders;
        this.isMarkering = isMarkering;
    }

    public void AddCars(int count, UI_Control ui_Control)
    {
        countCars += count;
        ui_Control.ViewCars(countCars, maxCars);
    }

    public void AddOrders(int count, UI_Control ui_Control)
    {
        countOrders += count;
        ui_Control.ViewOrders(countOrders, maxOrders);
    }

    public void AddExp(int count, UI_Control ui_Control)
    {
        countExp += count;
        ui_Control.ViewExp(countExp);
    }

    public void AddMany(int count, UI_Control ui_Control)
    {
        countMany += count;
        ui_Control.ViewMany(countMany);
    }

    public void ViewStartCarsAndOrders(UI_Control ui_Control)
    {
        ui_Control.ViewCars(countCars, maxCars);
        ui_Control.ViewOrders(countOrders, maxOrders);
    }

    public bool TestFinish()
    {
        return ((countCars >= maxCars) && (countOrders >= maxOrders));
    }
}
