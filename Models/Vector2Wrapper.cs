using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public struct Vector2Wrapper
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2Wrapper(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2Wrapper(Vector2 vector)
        {
            X = vector.x;
            Y = vector.y;
        }

        public static implicit operator Vector2Wrapper(Vector2 vector)
        {
            return new Vector2Wrapper(vector.x, vector.y);
        }

        public static implicit operator Vector2(Vector2Wrapper vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2Wrapper operator +(Vector2Wrapper vector1, Vector2Wrapper vector2)
        {
            return new Vector2Wrapper(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2Wrapper Create(Vector2 vector)
        {
            return new Vector2Wrapper(vector);
        }

        public Vector2 ToVector2() => new(X, Y);
        
        public override string ToString()
        {
            return string.Format("{0:F3}, {1:F3}", new object[]
            {
                X, Y
            });
        }

        public static bool TryParse(string? s, out Vector2Wrapper result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(s))
                return false;
            
            s = s.Replace(" ", string.Empty);
            var split = s.Split(',');
            if (!float.TryParse(split.ElementAtOrDefault(0), out var x))
                return false;
            
            if (!float.TryParse(split.ElementAtOrDefault(1), out var y))
                return false;

            result = new Vector2Wrapper(x, y);
            return true;
        }
    }
    
    
    public class Vector2WrapperConverter : JsonConverter<Vector2Wrapper>
    {
        public override void WriteJson(JsonWriter writer, Vector2Wrapper value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override Vector2Wrapper ReadJson(JsonReader reader, Type objectType, Vector2Wrapper existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Vector2Wrapper.TryParse(reader.Value?.ToString(), out var result);
            return result;
        }
    }
}