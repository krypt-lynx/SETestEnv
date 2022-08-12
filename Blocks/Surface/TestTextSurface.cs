using Sandbox.ModAPI.Ingame;
using SETestEnv.Surface.FontData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace SETestEnv
{
    public class TestTextSurface : IMyTextSurface, ISimulationElement
    {
        public TestTextSurface(string name = null, string displayName = null)
        {
            Name = name;
            DisplayName = displayName ?? name;
        }

        public string CurrentlyShownImage => throw new NotImplementedException();

        public float FontSize { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }
        public byte BackgroundAlpha { get; set; }
        public float ChangeInterval { get; set; }

        private string font = fonts.First().Key;
        public string Font {
            get => font;
            set
            {
                if (fonts.ContainsKey(value))
                {
                    font = value;
                } 
                else
                {
                    throw new Exception("Unsupported font name");
                }
            }
        }
        public TextAlignment Alignment { get; set; }
        public string Script { get; set; }
        public ContentType ContentType { get; set; } = ContentType.NONE;

        public Vector2 SurfaceSize => new Vector2(512, 512);
        public Vector2 TextureSize => new Vector2(512, 512);
        public bool PreserveAspectRatio { get; set; } = true;
        public float TextPadding { get; set; } = 0.2f;
        public Color ScriptBackgroundColor { get; set; }
        public Color ScriptForegroundColor { get; set; }

        private StringBuilder text = new StringBuilder();


        public string Name { get; private set; } = "test_surface"; // set only
        public string DisplayName { get; private set; } = "Test Surface"; // set only
        private static Dictionary<string, IFontDataProvider> fonts = new Dictionary<string, IFontDataProvider>
            {
                { "Debug", new DebugFontDataProvider() },
                { "Monospace", new MonospacedFontDataProvider() },
            };

        const float Magic = 28.8f / 37;

        public void AddImagesToSelection(List<string> ids, bool checkExistence = false)
        {
            throw new NotImplementedException();
        }

        public void AddImageToSelection(string id, bool checkExistence = false)
        {
            throw new NotImplementedException();
        }

        public void ClearImagesFromSelection()
        {
            throw new NotImplementedException();
        }

        public MySpriteDrawFrame DrawFrame()
        {
            throw new NotImplementedException();
        }

        public void GetFonts(List<string> fonts)
        {
            fonts.Clear();
            fonts.AddRange(TestTextSurface.fonts.Keys);
        }

        public void GetScripts(List<string> scripts)
        {
            scripts.Clear();
        }

        public void GetSelectedImages(List<string> output)
        {
            throw new NotImplementedException();
        }

        public void GetSprites(List<string> sprites)
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            return text.ToString();
        }


        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            var result = Measure(text, font);
            return new Vector2(result.X * Magic * scale, result.Y * Magic * scale);
        }

        private Vector2I Measure(StringBuilder str, string font)
        {
            var fontData = fonts[font];

            var lines = str.ToString().Split('\n');
            return new Vector2I(
                lines.Aggregate(0, (acc, line) => Math.Max(acc, fontData.Width(line.Trim('\n')))),
                fontData.Height() * lines.Length);
        }

        public void ReadText(StringBuilder buffer, bool append = false)
        {
            if (!append)
            {
                buffer.Clear();
            }
            buffer.Append(text);
        }

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        public bool surfaceChanged = false;
        public bool WriteText(string value, bool append = false)
        {
            surfaceChanged = true;

            if (!append)
            {
                text.Clear();
            }
            text.Append(value);

            return true;
        }

        public bool DrawBounds = false;
        private void RenderText()
        {
            Vector2I visible = (Vector2I)((SurfaceSize + new Vector2(1, 0)) / (new Vector2(25, 37) * Magic * FontSize));

            ConsoleColor fg = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Surface \"{Name}\":");


            string[] lines = text.ToString().Split('\n').Select(x => x.Trim('\r')).ToArray();
            var index = 0;
            foreach (string line in lines)
            {
                string visiblePart = line.Substring(0, Math.Min(line.Length, visible.X));
                string hiddenPart = line.Substring(Math.Min(line.Length, visible.X));
                if (index < visible.Y || !DrawBounds)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                if (visiblePart.Length != 0)
                {
                    Console.Write(visiblePart);
                }
                else
                {
                    Console.Write(new String(' ', visible.X));
                }

                if (DrawBounds)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }

                if (hiddenPart.Length > 0)
                {
                    Console.Write(hiddenPart);
                }
                else
                {
                    Console.CursorLeft = visible.X;
                    Console.Write(" ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();

                index++;
            }

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            //throw new NotImplementedException();
        }


        public bool WriteText(StringBuilder value, bool append = false) =>
            WriteText(value.ToString(), append);

        public void SimStart() { }

        public void SimEnd() { }

        public void BeforeSimStep() { }

        public void SimStep() { }

        public void AfterSimStep()
        {
            if (surfaceChanged)
            {
                RenderText();
                surfaceChanged = false;
            }
        }

        public void SimSave() { }
    }
}
