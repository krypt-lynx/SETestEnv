using BufferizedConsole;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace SETestEnv
{
    public class TestTextSurface : IMyTextSurface
    {
        public string CurrentlyShownImage => throw new NotImplementedException();

        public float FontSize { get; set; }
        public Color FontColor { get; set; }
        public Color BackgroundColor { get; set; }
        public byte BackgroundAlpha { get; set; }
        public float ChangeInterval { get; set; }
        public string Font { get; set; }
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


        public string Name { get; set; } = "test_surface"; // set only

        public string DisplayName { get; set; } = "Test Surface"; // set only

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
            fonts.Add("Debug");
            fonts.Add("Monospace");
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
            var result = Measure(text);
            return new Vector2(result.X * Magic * scale, result.Y * Magic * scale);
        }

        private Vector2I Measure(StringBuilder str)
        {
            var lines = str.ToString().Split('\n');
            return new Vector2I(
                lines.Aggregate(0, (acc, line) => Math.Max(acc, TestMonospacedFontDataProvider.Width(line.Trim('\n')))),
                TestMonospacedFontDataProvider.Height() * lines.Length);
        }

        static class TestMonospacedFontDataProvider
        {
            const int charWidth = 24;
            const int lineHeight = 37;
            const int letterSpacing = 1;

            static public int LetterSpacing(char left, char right)
            {
                return letterSpacing;
            }

            static public int Width(char ch)
            {
                return charWidth;
            }

            static public int Width(string str, char lead)
            {
                if (str.Length == 0)
                {
                    return 0;
                }
                return (charWidth + letterSpacing) * str.Length - letterSpacing;
            }

            static public int Height()
            {
                return lineHeight;
            }

            static public int Width(char ch, char lead)
            {
                return letterSpacing + charWidth;
            }

            static public int Width(string str)
            {
                return Width(str, '\0');
            }
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

        public bool DrawBounds = false;

        public bool WriteText(string value, bool append = false)
        {
            if (!append)
            {
                text.Clear();
            }
            text.Append(value);

            Vector2I visible = (Vector2I)((SurfaceSize + new Vector2(1, 0)) / (new Vector2(25, 37) * Magic * FontSize));

            ConsoleColor fg = BufConsole.ForegroundColor;
            ConsoleColor bg = BufConsole.BackgroundColor;

            BufConsole.ForegroundColor = ConsoleColor.Gray;
            BufConsole.WriteLine("LCD:");


            string[] lines = text.ToString().Split('\n').Select(x => x.Trim('\r')).ToArray();
            var index = 0;
            foreach (string line in lines)
            {
                string visiblePart = line.Substring(0, Math.Min(line.Length, visible.X));
                string hiddenPart = line.Substring(Math.Min(line.Length, visible.X));
                if (index < visible.Y || !DrawBounds)
                {
                    BufConsole.ForegroundColor = ConsoleColor.Cyan;
                    BufConsole.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    BufConsole.ForegroundColor = ConsoleColor.DarkBlue;
                    BufConsole.BackgroundColor = ConsoleColor.DarkGray;
                }
                if (visiblePart.Length != 0)
                {
                    BufConsole.Write(visiblePart);
                }
                else
                {
                    BufConsole.Write(new String(' ', visible.X));
                }

                if (DrawBounds)
                {
                    BufConsole.ForegroundColor = ConsoleColor.DarkBlue;
                    BufConsole.BackgroundColor = ConsoleColor.DarkGray;
                }

                if (hiddenPart.Length > 0)
                {
                    BufConsole.Write(hiddenPart);
                }
                else
                {
                    BufConsole.CursorLeft = visible.X;
                    BufConsole.Write(" ");
                }
                BufConsole.BackgroundColor = ConsoleColor.Black;
                BufConsole.WriteLine();

                index++;
            }

            BufConsole.ForegroundColor = fg;
            BufConsole.BackgroundColor = bg;

            return true;
            //throw new NotImplementedException();
        }


        public bool WriteText(StringBuilder value, bool append = false) =>
            WriteText(value.ToString(), append);
    }
}
