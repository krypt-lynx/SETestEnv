using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv.Surface.FontData
{
    public interface IFontDataProvider
    {
        int Width(char ch);
        int Width(string str, char lead = '\0');
        int Height();
        int LetterSpacing(char left, char right);
    }
}
