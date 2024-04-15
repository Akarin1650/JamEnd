using Godot;
using System;

public static class GlobalReferences
{
    public static class ObjectProperties
    {
		public static readonly string SCALE = "scale";
		public static readonly string RECT_SCALE = "rect_scale";
		public static readonly string RECT_ROTATION = "rect_rotation";
		public static readonly string PERCENT_VISIBLE = "percent_visible";
		public static readonly string WIDTH = "width";
		public static readonly string EMISSION_BOX_EXTENTS = "emission_box_extents";
		public static readonly string DIRECTION = "direction";
		public static readonly string MODULATE = "modulate";
		public static readonly string VALUE = "value";
		public static readonly string FRAME = "frame";
		public static readonly string FRAMES = "frames";
	}

	public static class Signals
    {
		public static readonly string TIMEOUT = "timeout";
		public static readonly string TEXT_CHANGED = "text_changed";
		public static readonly string PRESSED = "pressed";
		public static readonly string AREA_ENTERED = "area_entered";
		public static readonly string AREA_EXITED = "area_exited";
		public static readonly string VALUE_CHANGED = "value_changed";
		public static readonly string ANIMATION_FINISHED = "animation_finished";
		public static readonly string FINISHED = "finished";
		public static readonly string TWEEN_COMPLETED = "tween_completed";
	}

	public static class IndexReferences
    {
		public static readonly int X_DIMENSION = 0;
		public static readonly int Y_DIMENSION = 1;
		public static readonly int ENEMIES_PER_WAVE = 3;
	}

	public static class Names
	{
		public static readonly string IDLE = "Idle";
	}

	public static class Scenes
	{
		public static readonly string MAIN = "res://Scenes/Main.tscn";
		public static readonly string MAIN_MENU = "res://Scenes/UI/ScreenUI/MainMenu.tscn";
		public static readonly string TITLE_CARD = "";
	}

	public static class Inputs
	{
		public static readonly string RIGHT = "right";
		public static readonly string LEFT = "left";
		public static readonly string TOP = "top";
		public static readonly string BOTTOM = "bottom";
		public static readonly string UI_ACCEPT = "ui_accept";
	}
}
