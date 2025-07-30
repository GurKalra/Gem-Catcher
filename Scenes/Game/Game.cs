using Godot;
using System;

public partial class Game : Node2D
{
	const double GEM_MARGIN = 50.0;
	private float _gemSpeedMultiplier = 1.0f;
	private const float _speedFactor = 1.12f;
	[Export] private PackedScene _gemScene;
	[Export] private Timer _spawnTimer;
	[Export] private Label _scoreLabel;
	[Export] private AudioStreamPlayer _music;
	[Export] private AudioStreamPlayer2D _effects;
	[Export] private AudioStream _explodeSound;
	private int _score = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTimer.Timeout += SpawnGem;
		SpawnGem();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnGem()
	{
		Rect2 vpr = GetViewportRect();
		Gem gem = (Gem)_gemScene.Instantiate();

		float rX = (float)GD.RandRange(vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN);

		AddChild(gem);
		gem.Position = new Vector2(rX, -100);
		gem.OnScored += OnScored;
		gem.OnGemOffScreen += GameOver;

		gem.SpeedMultiplier = _gemSpeedMultiplier;
	}

	//EVENTS
	private void OnScored()
	{
		_score += 1;
		_scoreLabel.Text = $"{_score:0000}";
		_effects.Play();
		if (_score % 7 == 0)
		{
			_gemSpeedMultiplier *= _speedFactor;
		}
	}

	private void GameOver()
	{
		GD.Print("Game Over!!");
		foreach (Node node in GetChildren())
		{
			node.SetProcess(false);

		}
		_spawnTimer.Stop();
		_music.Stop();

		_effects.Stop();
		_effects.Stream = _explodeSound;
		_effects.Play();
	}
}
