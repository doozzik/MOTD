using System;

namespace MOTD
{
    class LineColor
    {
        private float _r, _g, _b;

        private void set(byte r, byte g, byte b)
        {
            _r = (1f / 255f) * r;
            _g = (1f / 255f) * g;
            _b = (1f / 255f) * b;
        }
        public float get(char channel)
        {
            switch (channel)
            {
                case 'r': return _r;
                case 'g': return _g;
                case 'b': return _b;
                default: return 0;
            }
        }

        public LineColor(string color)
        {
            string oldColor = color; // we need it to throw in Exception message

            color = color.ToLower();
            color = color.Replace(" ", "");

            string colorType = RecognizeColorType(color);
            
            if(colorType == "string") { ConvertString(color); }
            if(colorType == "hex") { ConvertHex(color); }
            if(colorType == "rgb") { ConvertRGB(color); }
            if(colorType == "none") { throw new Exception("Cant define color: " + oldColor); }
        }

        private string RecognizeColorType(string color)
        {
            int trueAmount = 0;
            string result = "none";

            if (ColorIsInString(color))
            {
                trueAmount++;
                result = "string";
            }
            if (ColorIsInHex(color))
            {
                trueAmount++;
                result = "hex";
            }
            if (ColorIsInRGB(color))
            {
                trueAmount++;
                result = "rgb";
            }

            if (trueAmount == 1)
            {
                return result;
            }

            return "none";
        }

        private void ConvertString(string stringColor)
        {
            if (stringColor == "white") { set(255, 255, 255); }
            if (stringColor == "silver") { set(192, 192, 192); }
            if (stringColor == "gray") { set(128, 128, 128); }
            if (stringColor == "grey") { set(128, 128, 128); }
            if (stringColor == "black") { set(0, 0, 0); }
            if (stringColor == "red") { set(255, 0, 0); }
            if (stringColor == "maroon") { set(128, 0, 0); }
            if (stringColor == "yellow") { set(255, 255, 0); }
            if (stringColor == "olive") { set(128, 128, 0); }
            if (stringColor == "lime") { set(0, 255, 0); }
            if (stringColor == "green") { set(0, 128, 0); }
            if (stringColor == "aqua") { set(0, 255, 255); }
            if (stringColor == "teal") { set(0, 128, 128); }
            if (stringColor == "blue") { set(0, 0, 255); }
            if (stringColor == "navy") { set(0, 0, 128); }
            if (stringColor == "fuchsia") { set(255, 0, 255); }
            if (stringColor == "purple") { set(128, 0, 128); }

        }
        private void ConvertHex(string hexColor)
        {
            hexColor = hexColor.Remove(0, 1); // remove '#' on first position

            if (hexColor.Length == 3) // if hex is like #ABC, convert it to #AABBCC
            {
                hexColor = hexColor.Insert(1, Convert.ToString(hexColor[0]));
                hexColor = hexColor.Insert(3, Convert.ToString(hexColor[2]));
                hexColor = hexColor.Insert(5, Convert.ToString(hexColor[4]));
            }

            byte r, g, b;

            // We have B60024
            r = ConvertHexPartToRGB(hexColor.Substring(0, 2)); // B6 goes to 182
            g = ConvertHexPartToRGB(hexColor.Substring(2, 2)); // 00
            b = ConvertHexPartToRGB(hexColor.Substring(4, 2)); // 24

            set(r, g, b);
        }
        private void ConvertRGB(string rgbColor)
        {
            int commaPos1 = 0;
            int commaPos2 = 0;

            for (int i = 0; i < rgbColor.Length; i++)
            {
                if (rgbColor[i] == ',')
                {
                    if (commaPos1 == 0)
                    {
                        commaPos1 = i;
                    }
                    else
                    {
                        commaPos2 = i;
                    }
                }
            }

            byte r, g, b;

            r = Convert.ToByte(rgbColor.Substring(0, commaPos1));
            g = Convert.ToByte(rgbColor.Substring(commaPos1 + 1, commaPos2 - commaPos1 - 1));
            b = Convert.ToByte(rgbColor.Substring(commaPos2 + 1));

            set(r, g, b);
        }

        private bool ColorIsInString(string color)
        {
            string[] possibleValues = { "white", "silver", "gray", "grey", "black", "red", "maroon", "yellow", "olive", "lime", "green", "aqua", "teal", "blue", "navy", "fuchsia", "purple" };

            if (!StringConsistOnlyOfThisArray(color, possibleValues))
            {
                return false;
            }

            return true;
        }
        private bool ColorIsInHex(string color)
        {
            if (color[0] == '#')
            {
                color = color.Remove(0, 1); // remove '#' on first position, if exist
            }
            else
            {
                return false;
            }
            
            if (color.Length == 3) // if hex is like #ABC, convert it to #AABBCC
            {
                color = color.Insert(1, Convert.ToString(color[0]));
                color = color.Insert(3, Convert.ToString(color[2]));
                color = color.Insert(5, Convert.ToString(color[4]));
            }

            if (color.Length != 6)
            {
                return false;
            }

            char[] possibleValues = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            if (!StringConsistOnlyOfThisArray(color, possibleValues))
            {
                return false;
            }

            return true; // if everything ok
        }
        private bool ColorIsInRGB(string color)
        {
            if (color.Length < 5 || color.Length > 11) // min "0,0,0" max "255,255,255"
            {
                return false;
            }

            if (CommasAmount(color) != 2)
            {
                return false;
            }

            if (color[0] == ',' || color[color.Length - 1] == ',') // if sting is like ",255,"
            {
                return false;
            }

            int commaPos1 = 0;
            int commaPos2 = 0;
            bool colorR = false; // will check, if string can be convertered to byte
            bool colorG = false;
            bool colorB = false;

            for (int i = 0; i < color.Length; i++)
            {
                if (color[i] == ',')
                {
                    if (commaPos1 == 0)
                    {
                        commaPos1 = i;
                    }
                    else
                    {
                        commaPos2 = i;
                    }
                }
            }

            if (commaPos1 == commaPos2 - 1) // if string is like "255,,7"
            {
                return false;
            }

            colorR = ConvertStringToByte(color.Substring(0, commaPos1)); // true, if can convert
            colorG = ConvertStringToByte(color.Substring(commaPos1 + 1, commaPos2 - commaPos1 - 1));
            colorB = ConvertStringToByte(color.Substring(commaPos2 + 1));

            if (!colorR || !colorG || !colorB) // if one of color cant be converted to byte (to have size of 0..255)
            {
                return false;
            }

            return true;
        }

        private bool StringConsistOnlyOfThisArray(string str, string[] stringsArray)
        {
            for (int i = 0; i < stringsArray.Length; i++)
            {
                if (str == stringsArray[i])
                {
                    return true;
                }
            }

            return false;
        }
        private bool StringConsistOnlyOfThisArray(string str, char[] charArray)
        {
            int temp = 0;

            for (int i = 0; i < str.Length; i++)
            {
                for (int k = 0; k < charArray.Length; k++)
                {
                    if (str[i] == charArray[k]) // AB4C 0123456789ABC
                    {
                        temp++;
                        break;
                    }
                }
            }

            if (temp == str.Length)
            {
                return true;
            }

            return false;
        }

        private bool ConvertStringToByte(string str)
        {
            byte byteVal = 0;
            try
            {
                byteVal = Convert.ToByte(str);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private int CommasAmount(string str)
        {
            int commasAmount = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',')
                {
                    commasAmount++;
                }
            }
            return commasAmount;
        }

        private byte ConvertHexPartToRGB(string str)
        {
            char[] lettersArray = { 'a', 'b', 'c', 'd', 'e', 'f' };
            byte[] digitsArray = { 10, 11, 12, 13, 14, 15 };
            byte hex1 = 0;
            byte hex2 = 0;
            
            for(byte i = 0; i < 10; i++)
            {
                if (str[0] == i) { hex1 = i; }
                if (str[1] == i) { hex2 = i; }
            }

            for(byte i = 0; i < 6; i++)
            {
                if (str[0] == lettersArray[i]) { hex1 = digitsArray[i]; }
                if (str[1] == lettersArray[i]) { hex2 = digitsArray[i]; }
            }

            return Convert.ToByte(hex1 * 16 + hex2 * 1);
        }
    }
}
