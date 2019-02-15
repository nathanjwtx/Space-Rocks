using Godot;
using System;

public class Global : Node
{
    private static  int _score;
    private static bool _hardcore;
    private static int _hits;
    private static int _highscore;
    private static string ScoreFile = "user://highscore.txt";

    public override void _Ready()
    {
        GD.Print(OS.GetUserDataDir());
        Setup();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustReleased("exit"))
        {
            GetTree().Quit();
        }
    }

    public static int Hits { get => _hits; set => _hits += value; }
    
    public static int Score { get => _score; set => _score += value; }

    public static bool Hardcore { get => _hardcore; set => _hardcore = value; }
    public static int HighScore { get => _highscore; set => _highscore = value; }

    public static void SetHighscore()
    {
        if (Score <= HighScore) return;
        HighScore = Score;
        SaveScore();
    }
    
    public void Setup()
    {
        File f = new File();
        if (f.FileExists(ScoreFile))
        {
            f.Open(ScoreFile, 1);
            string content = f.GetAsText();
            HighScore = Convert.ToInt32(content);
            f.Close();
        }
    }

    public static void SaveScore()
    {
        File f = new File();
        f.Open(ScoreFile, 2);
        f.StoreString(Convert.ToString(HighScore));
        f.Close();
    }
}
