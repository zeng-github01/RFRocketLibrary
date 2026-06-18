using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public struct Color32Wrapper
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color32Wrapper(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color32Wrapper(Color32 color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public static implicit operator Color32Wrapper(Color32 color)
        {
            return new Color32Wrapper(color.r, color.g, color.b, color.a);
        }

        public static implicit operator Color32(Color32Wrapper wrapper)
        {
            return new Color32(wrapper.R, wrapper.G, wrapper.B, wrapper.A);
        }

        public static Color32Wrapper Create(Color32 color)
        {
            return new Color32Wrapper(color);
        }

        public Color32 ToColor32() => new Color32(R, G, B, A);

        public override string ToString()
        {
            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A);
        }

        public static bool TryParse(string? s, out Color32Wrapper result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(s))
                return false;

            s = s.Trim().Replace("#", string.Empty);

            if (s.Length == 6 || s.Length == 8)
            {
                if (uint.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out uint hex))
                {
                    if (s.Length == 6)
                    {
                        byte r = (byte)((hex >> 16) & 0xFF);
                        byte g = (byte)((hex >> 8) & 0xFF);
                        byte b = (byte)(hex & 0xFF);
                        result = new Color32Wrapper(r, g, b, 255);
                    }
                    else
                    {
                        byte r = (byte)((hex >> 24) & 0xFF);
                        byte g = (byte)((hex >> 16) & 0xFF);
                        byte b = (byte)((hex >> 8) & 0xFF);
                        byte a = (byte)(hex & 0xFF);
                        result = new Color32Wrapper(r, g, b, a);
                    }
                    return true;
                }
            }

            var split = s.Split(',');
            if (split.Length >= 3)
            {
                if (!byte.TryParse(split[0].Trim(), out var r)) return false;
                if (!byte.TryParse(split[1].Trim(), out var g)) return false;
                if (!byte.TryParse(split[2].Trim(), out var b)) return false;

                byte a = 255;
                if (split.Length == 4 && !byte.TryParse(split[3].Trim(), out a)) return false;

                result = new Color32Wrapper(r, g, b, a);
                return true;
            }

            return false;
        }
    }

    public class Color32WrapperConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            Color32Wrapper.TryParse(reader.Value?.ToString(), out var result);
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Color32Wrapper).IsAssignableFrom(objectType);
        }
    }
}
