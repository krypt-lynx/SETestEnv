using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Utils;
using Sandbox.Common.ObjectBuilders;

namespace SETestEnv
{

    public class TestTextPanel : TestFunctionalBlock, IMyTextPanel, IMyTextSurface
    {
        public string CurrentlyShownImage { get; set; }

        public ShowTextOnScreenFlag ShowOnScreen { get; set; }

        public bool ShowText { get; set; }

        public TestTextSurface surface;

        static private Dictionary<long, string> fontHashMap = new Dictionary<long, string> {
            { MyStringHash.GetOrCompute("Debug").GetHashCode(), "Debug" },
            { MyStringHash.GetOrCompute("Monospace").GetHashCode(), "Monospace" },
        };

        public TestTextPanel(string subtype = null) : base(subtype)
        {
            surface = new TestTextSurface(new Vector2(512,512), new Vector2(512, 512), "Text Panel", "Text Panel", this);
            surfaceProvider.AddSurface(surface);            

            InitProperty(new TestProp<IMyTextPanel, long>("Font",
                block => MyStringHash.GetOrCompute(block.Font).GetHashCode(), (block, value) => {
                    fontHashMap.TryGetValue(value, out var fontName);
                    block.Font = fontName;
                }
                ));
            InitProperty(new TestProp<IMyTextPanel, float>("FontSize",
                block => block.FontSize,
                (block, value) => block.FontSize = value
                ));
            InitProperty(new TestProp<IMyTextPanel, Color>("FontColor",
                block => block.FontColor,
                (block, value) => block.FontColor = value
                ));
            InitProperty(new TestProp<IMyTextPanel, Color>("BackgroundColor",
                block => block.BackgroundColor,
                (block, value) => block.BackgroundColor = value
                ));
        }

        public bool WritePublicText(string value, bool append = false) => surface.WriteText(value, append);

        public string GetPublicText() => surface.GetText();

        string title;

        public bool WritePublicTitle(string value, bool append = false)
        {
            title = append ? title + value : value;
            return true;
        }

        public string GetPublicTitle() => title;

        public bool WritePrivateText(string value, bool append = false) => true;

        public string GetPrivateText() => "";

        public bool WritePrivateTitle(string value, bool append = false) => true;

        /// <summary>
        /// Convinience property, not available on ingame interface
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        /// <summary>
        /// Convinience property, not available on ingame interface
        /// </summary>
        public string Text
        {
            get => surface.GetText();
            set => surface.WriteText(value);
        }

        public float FontSize
        {
            get => surface.FontSize;
            set => surface.FontSize = value;
        }

        public Color FontColor
        {
            get => surface.FontColor;
            set => surface.FontColor = value;
        }

        public Color BackgroundColor
        {
            get => surface.BackgroundColor;
            set => surface.BackgroundColor = value;
        }

        public float ChangeInterval
        {
            get => surface.ChangeInterval;
            set => surface.ChangeInterval = value;
        }

        public string Font
        {
            get => surface.Font;
            set => surface.Font = value;
        }

        public byte BackgroundAlpha
        {
            get => surface.BackgroundAlpha;
            set => surface.BackgroundAlpha = value;
        }

        public TextAlignment Alignment
        {
            get => surface.Alignment;
            set => surface.Alignment = value;
        }

        public string Script
        {
            get => surface.Script;
            set => surface.Script = value;
        }

        public ContentType ContentType {
            get => surface.ContentType;
            set => surface.ContentType = value;
        }

        public Vector2 SurfaceSize => surface.SurfaceSize;

        public Vector2 TextureSize => surface.TextureSize;

        public bool PreserveAspectRatio {
            get => surface.PreserveAspectRatio;
            set => surface.PreserveAspectRatio = value;
        }

        public float TextPadding
        {
            get => surface.TextPadding;
            set => surface.TextPadding = value;
        }

        public Color ScriptBackgroundColor
        {
            get => surface.ScriptBackgroundColor;
            set => surface.ScriptBackgroundColor = value;
        }

        public Color ScriptForegroundColor
        {
            get => surface.ScriptForegroundColor;
            set => surface.ScriptForegroundColor = value;
        }

        public string GetPrivateTitle() => "";

        public void AddImageToSelection(string id, bool checkExistence = false) =>
            surface.AddImageToSelection(id, checkExistence);

        public void AddImagesToSelection(List<string> ids, bool checkExistence = false) =>
            surface.AddImagesToSelection(ids, checkExistence);

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false) =>
            surface.RemoveImageFromSelection(id, removeDuplicates);

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false) =>
            surface.RemoveImagesFromSelection(ids, removeDuplicates);

        public void ClearImagesFromSelection() =>
            surface.ClearImagesFromSelection();

        public void GetSelectedImages(List<string> output) =>
            surface.GetSelectedImages(output);

        public void ShowPublicTextOnScreen() { surface.ContentType = ContentType.TEXT_AND_IMAGE; }
        public void ShowPrivateTextOnScreen() { }
        public void ShowTextureOnScreen() { surface.ContentType = ContentType.TEXT_AND_IMAGE; }

        public void SetShowOnScreen(ShowTextOnScreenFlag set) { }

        public bool WritePublicText(StringBuilder value, bool append = false) => surface.WriteText(value, append);

        public void ReadPublicText(StringBuilder buffer, bool append = false) => surface.ReadText(buffer, append);

        public void GetFonts(List<string> fonts) => surface.GetFonts(fonts);

        public bool WriteText(string value, bool append = false) => surface.WriteText(value, append);

        public string GetText() => surface.GetText();

        public bool WriteText(StringBuilder value, bool append = false) => surface.WriteText(value, append);

        public void ReadText(StringBuilder buffer, bool append = false) => surface.ReadText(buffer, append);

        public void GetSprites(List<string> sprites) => surface.GetSprites(sprites);

        public void GetScripts(List<string> scripts) => surface.GetScripts(scripts);

        public MySpriteDrawFrame DrawFrame() => surface.DrawFrame();

        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) => surface.MeasureStringInPixels(text, font, scale);
    }

}
