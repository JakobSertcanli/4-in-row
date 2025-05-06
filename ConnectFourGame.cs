using System;
using Gtk;
using Timeout = GLib.Timeout;
using Window = Gtk.Window;
using Label = Gtk.Label;
using Color = Cairo.Color;
using Gdk;
using Atk;

public enum GameState {Tie, Win, Ongoing}

class Game
{
    public GameState gameState { get; private set; } = GameState.Ongoing  ;
    Board board;
    IWinCondition winCheck;
    public bool IsPlayerOne { get; private set; }
    public bool IsGameOver { get; private set; } = false;
    public Board Board => board;
    public delegate void BoardUpdatedHandler();
    public event BoardUpdatedHandler? BoardUpdated;
    public IPlayer p1 { get; }
    public IPlayer p2 { get; }
    public IPlayer? winner { get; private set; }

    public Game(int rows, int cols, IWinCondition winCheck, IPlayer p1, IPlayer p2)
    {
        if(rows <= 0 || cols <= 0)
            throw new ArgumentException("Rows and Columns must be positive integers.");
        Random rnd = new();
        this.board = new Board(rows, cols);
        this.winCheck = winCheck;
        this.p1 = p1; 
        this.p2 = p2;

        IsPlayerOne = rnd.Next(2) == 0;
    }
    public bool MakeMove(IPlayer player)
    {
        if(IsGameOver)
            return false;
        int col = player.Turn(out CellState color);
        if(board.DropPiece(col, color))
        {
            BoardUpdated?.Invoke();
            if(board.IsFull)
            {
                gameState = GameState.Tie;
                IsGameOver = true;
            }
            if(winCheck.WinConditionMeet(col, color, board))
            {
                gameState = GameState.Win;
                winner = player;
                IsGameOver = true;
            }
            else
                SwitchTurn();

            return true;
        }
        return false;
    }
    public void SwitchTurn()
    {
        IsPlayerOne = !IsPlayerOne;
    }
    public void Reset()
    {
        board = new Board(board.rows, board.cols);
        gameState = GameState.Ongoing;
        IsGameOver = false;
        winner = null;

        Random rnd = new();
        IsPlayerOne = rnd.Next() % 2 == 0;
        BoardUpdated?.Invoke(); 
    }
}

class GameWindow : Window
{
    Game game;
    IRenderer renderer;
    bool isProcessingMove;
    public GameWindow(int rows, int cols) : base("game")
    {
        if(rows <= 0 || cols <= 0)
            throw new ArgumentException("Rows and Columns must be positive integers.");
        renderer = new Renderer(rows, cols);
        game = new Game(rows, cols, new FourInRow(), new CPUPlayer(CellState.Yellow), new KeyboardPlayer(CellState.Red, this));
        renderer.SetBoard(game.Board);

        game.BoardUpdated += renderer.Display;
        Add(renderer.grid);
        ShowAll();
        DeleteEvent += (sender, args) => Application.Quit();
        KeyPressEvent += OnKeyPress;
        StartGameLoop();
    }
    private void OnKeyPress(object sender, KeyPressEventArgs args)
    {
        if(args.Event.Key == Gdk.Key.Escape)
            Application.Quit();
    }

    private void GameOverScreen()
    {
        Window window = new("Game over!");
        window.DeleteEvent += (sender, args) => Application.Quit();
        Label label;
        Fixed container = new();
        window.SetSizeRequest(500,500);

        if(game.gameState == GameState.Tie)
            label = new("Its a Tie");
        else 
            label = new($"{game.winner} won the game!");
        Button PlayAgain = new("Reset");
        Button Exit = new("Exit");
        PlayAgain.Clicked += (sender, args) => {game.Reset(); renderer.SetBoard(game.Board); StartGameLoop(); window.Destroy();renderer.Display(); isProcessingMove = false;};
        Exit.Clicked += (sender, args) => Application.Quit();
        container.Put(PlayAgain, 50, 50);
        container.Put(Exit, 50, 100);
        container.Put(label, 200,200);
        window.Add(container);
        window.ShowAll();
    }
    private void StartGameLoop()
    {
        Timeout.Add(100, () => {
            if (isProcessingMove) 
                return true; 

            isProcessingMove = true; 

            if (game.IsPlayerOne)
            {
                if (!game.MakeMove(game.p1))
                {
                    isProcessingMove = false; 
                    return true; 
                }
            }
            else
            {
                if (!game.MakeMove(game.p2))
                {
                    isProcessingMove = false; 
                    return true; 
                }
            }

            if (game.gameState != GameState.Ongoing)
            {
                GameOverScreen();
                return false; 
            }

            isProcessingMove = false; 
            return true; 
        });
    }
}