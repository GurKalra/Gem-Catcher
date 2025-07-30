using Godot;
using System;
using System.IO;

public partial class Gem : Area2D
{
	public float SpeedMultiplier { get; set; } = 1.0f;
	[Export] float _gemSpeed = 150.0f;
	[Signal] public delegate void OnScoredEventHandler();
	[Signal] public delegate void OnGemOffScreenEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(0, _gemSpeed * (float)delta * SpeedMultiplier);
		CheckHitBottom();
	}

	private void OnAreaEntered(Area2D area)
	{
		GD.Print("Scored");
		EmitSignal(SignalName.OnScored);
		QueueFree();
	}

	private void CheckHitBottom()
	{
		Rect2 vpr = GetViewportRect();
		if (Position.Y > vpr.End.Y)
		{
			EmitSignal(SignalName.OnGemOffScreen);
			SetProcess(false);
			QueueFree();
		}
	}
}
