using Godot;
using System.Collections.Generic;

public class Credits : Control
{
    public static Credits instance;

    [Export] private NodePath animPath;

    public AnimationPlayer anim;
    private const float section_time = 1.8f;
    private float line_time = 1f;
    private const float base_speed = 150f;
    private const float speed_up_multiplier = 3.0f;

    private float scrollSpeed;
    private bool speed_up = false;

    private Label line;
    private bool started = false;
    private bool finished = false;

    private List<List<string>> credits = new List<List<string>>
    {
        new List<string>
        {
            "SummonInc"
        },
        new List<string>
        {
            "Programming",
            "Baptiste Simon",
            "Yoann Tsitionis",
            "Alexis Martin",
            "Baptiste Vu Liem",
            "Louis Peyrat",
            "Killian Ferreira Da Costa"
        },
        new List<string>
        {
            "Game Design",
            "Baptiste Simon",
            "Yoann Tsitionis",
            "Alexis Martin",
            "Baptiste Vu Liem",
            "Louis Peyrat",
            "Killian Ferreira Da Costa"
        },
        new List<string>
        {
            "Art",
            "Julia Feng",
            "Helene Feng"
        },
        new List<string>
        {
            "Music & Sound Design",
            "Renaud Gaillardon",
            "Fleur Breussin"
        },
    };

    private string titleThemePath = "res://Ressource/Font/titleTheme.tres";
    private string textThemePath = "res://Ressource/Font/textTheme.tres";

    private List<string> section;

    private bool isFirst = true;
    private bool section_next = true;
    private float section_timer = 0.0f;
    private float line_timer = 0.0f;
    private int curr_line = 0;
    private List<Label> lines = new List<Label>();

    public override void _Ready()
    {
        if (instance != null)
        {
            QueueFree();
            GD.Print(nameof(Credits) + " Instance already exist, destroying the last added.");
            return;
        }
        instance = this;

        anim = GetNode<AnimationPlayer>(animPath);
        line = GetNode<Label>("CreditsContainer/Line");

        anim.Connect(GlobalReferences.Signals.ANIMATION_FINISHED, this, nameof(AnimFinished));
        SetProcess(true);
    }

    public override void _Process(float delta)
    {
        scrollSpeed = base_speed * delta;

        if (section_next)
        {
            section_timer += speed_up ? delta * speed_up_multiplier : delta;
            if (section_timer >= section_time)
            {
                section_timer -= section_time;

                if (credits.Count > 0)
                {
                    started = true;
                    section = credits[0]; // Assign the new section
                    credits.RemoveAt(0);
                    curr_line = 0;
                    AddLine();
                }
            }
        }
        else
        {
            line_timer += speed_up ? delta * speed_up_multiplier : delta;
            if (line_timer >= line_time)
            {
                line_timer -= line_time;
                AddLine();
            }
        }

        if (speed_up)
            scrollSpeed *= speed_up_multiplier;

        if (lines.Count > 0)
        {
            foreach (Label l in lines)
            {
                l.RectPosition -= new Vector2(0, scrollSpeed);
                if (l.RectPosition.y < -l.RectSize.y)
                {
                    lines.Remove(l);
                    l.QueueFree();
                    break;
                }
            }
        }
        else if (started)
        {
            Finish();
        }
    }

    private void AnimFinished(string pName)
    {
        if (pName == "Spawn")
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else QueueFree();
        } 
    }

    private void Finish()
    {
        if (!finished)
        {
            anim.PlayBackwards("Spawn");
        }
    }

    private void AddLine()
    {
        if (section == null || section.Count == 0)
        {
            section_next = true;
            return;
        }
        section_next = false;
        Label newLine = (Label)line.Duplicate();
        if (curr_line == 0)
        {
            newLine.Theme = GD.Load<Theme>(titleThemePath);
            line_time = 1f;
        }
        else
        {
            newLine.Theme = GD.Load<Theme>(textThemePath);
            line_time = 0.5f;
        }

        newLine.Text = section[curr_line];
        lines.Add(newLine);
        GetNode<Control>("CreditsContainer").AddChild(newLine);

        curr_line++;
        if (curr_line >= section.Count)
        {
            section_next = true;
            curr_line = 0;
            section = null;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Finish();
        }
        if (@event.IsActionPressed("ui_down") && !@event.IsEcho())
        {
            speed_up = true;
        }
        if (@event.IsActionReleased("ui_down") && !@event.IsEcho())
        {
            speed_up = false;
        }
    }
}
