using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv.Surface.FontData
{
    class MonospacedFontDataProvider : IFontDataProvider
    {
        const int charWidth = 24;
        const int lineHeight = 37;
        const int letterSpacing = 1;
         
        public int LetterSpacing(char left, char right)
        {
            return letterSpacing;
        }

        public int Width(char ch)
        {
            return charWidth;
        }

        public int Width(string str, char lead = '\0')
        {
            return (charWidth + letterSpacing) * str.Length;
        }

        public int Height()
        {
            return lineHeight;
        }
    }
}
